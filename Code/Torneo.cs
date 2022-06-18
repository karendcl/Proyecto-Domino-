using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace Game;




//Torneo Se encarga de crear los juegos y procesarlos
public class Championship
{
    public int Champions { get; private set; }



    private Game[] Games;
    Observer observer = new Observer();

    public Championship(int cantTorneos)
    {
        this.Champions = cantTorneos;
        this.Games = new Game[cantTorneos];
        run();
    }



    public void run()
    {
        Games = observer.SelectGameTypes(Games);

        for (int i = 0; i < Games.Length; i++)
        {
            Game game = Games[i];

            game.PlayAGame();

            System.Console.WriteLine("Termino el juego {0}", i + 1);
            Thread.Sleep(500);

            if (!ContinueGames()) { observer.WriteStats(game); break; }
            observer.WriteStats(game);
        }
        int o = 1;
        foreach (var gamed in Games) //Esto esta clableado 
        {
            System.Console.WriteLine("partida {0}", o++);
            foreach (var winner in gamed.Winner())
            {
                Console.WriteLine("Player {0}", winner.Id);
            }
            System.Console.WriteLine("Presiona una tecla para ver la sgt partida");
            Console.ReadKey();
            System.Console.WriteLine("////////////////////////////////////");

        }

    }

    public bool ContinueGames()
    {
        return observer.Msg("Desea seguir jugando?, si / no");
    }
    /*
  private bool PlayAGame(Game game)  //Se paso a Game 
  {

      //Control que todo se pase por referencia
      // Debug.Assert(game.board.board.Count>1,"Problemas con valores de referencia");

      IJudge judge = game.judge;
      List<IPlayer> player = game.player;
      IBoard board = game.board;

      while (!judge.EndGame(game)) //mientras no se acabe el juego
      {
          for (int i = 0; i < game.player.Count; i++) //turno de cada jugador
          {
              if (judge.EndGame(game)) break;


              // Console.WriteLine(player[i].ToString());
              observer.PaintPlayerInConsole(player[i]);
              // Console.WriteLine(game.board.ToString());

              observer.PaintBord(board);

              observer.Clean();

              Token Token1 = Turno(player[i], game);  //la ficha que se va a jugar                     

              if (Token1 is null || !game.judge.ValidPlay(game.board, Token1))
              { //si es nulo, el jugador se ha pasado
                  game.SwapDirection(i);
              }

              if (Token1 != null) //si no es nulo, entonces si lleva
              {
                  if (game.judge.ValidPlay(game.board, Token1))
                  { //si es valido
                      int index = -1;

                      if (game.judge.PlayAmbigua(Token1, game.board))
                      {  //si se puede jugar por ambos lados, se le pide que escoja un lado
                          index = player[i].ChooseSide(game);
                      }

                      game.judge.AddTokenToBoard(Token1, game.board, index);
                      player[i].hand.Remove(Token1); //se elimina la ficha de la mano del jugador
                  }
              }

              Thread.Sleep(500);
          }


      }
      return true;

  }

  private Token Turno(IPlayer player, Game game)
  {
      return player.BestPlay(game);
  }

   */




}
public class ScoreChampionNormal : IGetScore<IPlayer>
{
    private Game game { get; set; }
    public ScoreChampionNormal(Game game)
    {
        this.game = game;
    }
    public int Score(IPlayer item)
    {
        return game.judge.PlayerScore(item);
    }
}

public class StopChampion : IStopGame<Game, IPlayer>
{
    private int Point { get; set; }
    public List<IPlayer> Players { get; set; }
    public List<int> acc { get; set; }
    public StopChampion(int porcentOfpoints)
    {
        this.Players = new List<IPlayer>() { };
        this.Point = porcentOfpoints;
    }
    //Cada juego se comprueba que no exeda de puntos
    //Se asume que estan todos los jugadores desde un inicio en caso contrario se añade 
    public bool MeetsCriteria(Game game, IGetScore<IPlayer> howtogetscore)
    {
        this.score(game, howtogetscore);
        foreach (var item in acc) { if (item > Point) { return false; } }
        return true;
    }

    private void score(Game game, IGetScore<IPlayer> howtogetscore)
    {
        List<IPlayer> temp = game.player;
        foreach (var item in temp)
        {
            int cant = howtogetscore.Score(item);
            if (!Players.Contains(item)) { Players.Add(item); acc.Add(cant); }
            else { int i = Players.IndexOf(item); acc[i] += cant; }
        }
    }
}

public class WinChampion : IWinCondition<Game, IPlayer>



{// Espresar en fraccion el porcentaje de ganadas del total del torneo

    public double Porcent { get; private set; }

    public List<WIPlayer<IPlayer>> players { get; private set; }
    public List<int> cantwins { get; private set; }
    public WinChampion(double porcentWins)
    {
        this.players = new List<WIPlayer<IPlayer>>() { };
        this.cantwins = new List<int>() { };
        this.Porcent = porcentWins;
    }
    public List<IPlayer> Winner(List<Game> games, IJudge<Game, IPlayer> judge)
    {
        List<IPlayer> winners = new List<IPlayer>() { };
        foreach (var game in games)
        {
            winners = game.Winner();

            for (int i = 0; i < winners.Count; i++)
            {
                WIPlayer<IPlayer> temp = new WIPlayer<IPlayer>(winners[i], i);
                if (!players.Contains(temp)) { players.Add(temp); }
                else { int x = players.IndexOf(temp); players[x].Puntuation += temp.Puntuation; }
            }
        }
        players.Sort(new WIPlayer_Comparer());

        players.Reverse();

        List<IPlayer> list = new List<IPlayer>() { };
        for (int i = 0; i < players.Count; i++)
        {
            list.Add(players[i].player);
        }

        return list;
    }

    public class WIPlayer<T> : IEquatable<WIPlayer<T>> where T : IEquatable<T>
    {
        public T player { get; private set; }
        public int Puntuation { get; set; }

        public WIPlayer(T player, int Puntuation)
        {
            this.player = player;
            this.Puntuation = Puntuation;
        }



        public bool Equals(WIPlayer<T>? other)
        {
            if (other == null || this == null) { return false; }
            return this.player.Equals(other.player);
        }
    }

    public class WIPlayer_Comparer : IComparer<WIPlayer<IPlayer>>
    {
        public int Compare(WIPlayer<IPlayer>? x, WIPlayer<IPlayer>? y)
        {
            if (x == null || y == null) { throw new NullReferenceException("null"); }
            if (x?.Puntuation < y?.Puntuation)
            {
                return -1;
            }
            if (x?.Puntuation > y?.Puntuation)
            {
                return 1;
            }

            return 0;
        }
    }
}



public class ValidChampion : IValidPlayChampion<Game, IPlayer>
{
    public bool ValidPlay(Game game, IPlayer players)
    {
        foreach (var player in game.player)
        {
            if (player.hand.Count > 0) return true;
        }
        return false;
    }
}

public class ChampionJudge
{
    public IStopGame<Game, Token> stopcriteria { get; set; }
    public IWinCondition<Game, Player> winCondition { get; set; }
    public IValidPlayChampion<Game, IPlayer> valid { get; set; }
    public IGetScore<Token> howtogetscore { get; set; }

    public ChampionJudge(IStopGame<Game, Token> stopcriteria, IWinCondition<Game, Player> winCondition, IValidPlayChampion<Game, IPlayer> valid, IGetScore<Token> howtogetscore)
    {
        this.howtogetscore = howtogetscore;
        this.stopcriteria = stopcriteria;
        this.valid = valid;
        this.winCondition = winCondition;
    }
    public bool EndGame(Game game)
    {
        if (stopcriteria.MeetsCriteria(game, howtogetscore)) return true;
        return true;
    }

    public bool ValidPlay(Game game, IPlayer player)
    {
        return valid.ValidPlay(game, player);
    }
}

public class Msg
{
    public IPlayer? player { private set; get; }

    public void PlayerMsg(IPlayer player)
    {
        this.player = player;

    }
}

