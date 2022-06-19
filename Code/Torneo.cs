using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace Game;

//Torneo Se encarga de crear los juegos y procesarlos
public class Championship
{
    public int Champions { get; private set; }

    public ChampionJudge judge { get; private set; }
    public IWinCondition<Game, IPlayer> winChampion { get; private set; }
    public IStopGame<Game, IPlayer> stopChampion { get; private set; }
    public IValidPlay validChampion { get; private set; }
    public IGetScore<IPlayer> Score { get; private set; }



    private Game[] Games;
    //jugadores a nivel de torneo 
    List<IPlayer> Players;


    Observer observer = new Observer();
    public Championship(int cantTorneos, ChampionJudge judge, List<IPlayer> players)
    {
        this.Champions = cantTorneos;
        this.Games = new Game[cantTorneos];
        this.Players = players;
        this.judge = judge;
        // this.winChampion = winChampion;
        // this.stopChampion = stopChampion;
        // this.validChampion = validChampion;
        // this.Score = Score;
        Run();
    }



    public void Run()
    {

        Games = observer.SelectGameTypes(Games);

        for (int i = 0; i < Games.Length; i++)
        {
            Game game = Games[i];

            game.PlayAGame();

            System.Console.WriteLine("Termino el juego {0}", i + 1);
            Thread.Sleep(1000);

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


}









public class Msg
{

}

