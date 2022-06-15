using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace Juego;


public class Observer
{


    public bool Msg(string msg)
    {
        Console.WriteLine(msg);
        string playing = Console.ReadLine()!;
        if (playing[0] == 's')
        {
            return true;
        }
        return false;
    }

    public void PaintInConsole(Msg mgs)
    {

    }
    public bool InteractConsole(string Msg)
    {
        System.Console.WriteLine(Msg);
        string temp = Console.ReadLine()!;

        if (temp[0] == 's' || temp[0] == 'y') return true;
        return false;
    }
    public void WriteStats(Game game)
    {
        Console.BackgroundColor = ConsoleColor.Green;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.Clear();
        Console.WriteLine("Game Over");

        Console.WriteLine(" {0} \n \n Scores of this game:", game.board.ToString());

        foreach (var play in game.player)
        {
            Console.WriteLine(play.ToString() + " \n Score in this game: " + game.judge.PlayerScore(play) + "\n Total Score: " + play.TotalScore);
        }

        Console.WriteLine("\n Winner(s): ");

        foreach (var winner in game.Winner())
        {
            Console.WriteLine("Player {0}", winner.Id);
        }
    }

    public void Clean()
    {
        Console.Clear();
    }
    public void PaintPlayerInConsole(IPlayer player)
    {
        Clean();
        System.Console.WriteLine(player.ToString());
        Thread.Sleep(1000);
    }

    public void PaintBord(IBoard board)
    {
        Clean();
        Console.WriteLine(board.ToString());
        Thread.Sleep(1000);
    }

    public Game PedirTipoJuego()
    {
        #region Setting Up The Game  


        int cadauno = 0;
        int cantplay = 0;
        int max = 0;


        IStopGame stopcondition = new Classic();

        int stop = 0;

        while (stop < 1 || stop > 2)
        {
            Console.WriteLine("Que tipo de criterio quiere para terminar el juego? \n\n 1. Clasico (se tranque o alguien se pegue) \n 2. Que alguien tenga un score especifico");
            stop = 1;
            // int.TryParse(Console.ReadLine(),out stop);
            stop = 1;
            switch (stop)
            {
                case 1:
                    stopcondition = new Classic();
                    break;

                case 2:
                    Console.WriteLine("Que score especifico desea?");
                    int specificscore = int.Parse(Console.ReadLine()!);
                    stopcondition = new CertainScore(specificscore);
                    break;
            }
        }

        Console.Clear();

        IGetScore HowTogetScore = new ClassicScore();

        int score = 0;

        while (score < 1 || score > 2)
        {
            Console.WriteLine("Que manera quiere para contar las fichas? \n\n 1. Clasico (la suma del valor de sus partes) \n 2. Si la ficha es doble, vale el doble de puntos");
            //  score = int.Parse(Console.ReadLine()!);
            score = 1;
            switch (score)
            {
                case 1:
                    HowTogetScore = new ClassicScore();
                    break;

                case 2:
                    HowTogetScore = new DoubleScore();
                    break;
            }
        }

        Console.Clear();

        IWinCondition winCondition = new MinScore();

        int winConditionn = 0;

        while (winConditionn < 1 || winConditionn > 3)
        {
            Console.WriteLine("Una vez acabe el juego, quien ganaria? \n \n 1. El que tenga mas puntos \n 2. El que tenga menos puntos \n 3. El que tenga una catidad de puntos especificos");
            winConditionn = int.Parse(Console.ReadLine()!);
            winConditionn = 1;
            switch (winConditionn)
            {
                case 1:
                    winCondition = new MaxScore();
                    break;

                case 2:
                    winCondition = new MinScore();
                    break;

                case 3:
                    int spscore = 0;
                    Console.WriteLine("Que score especifico?");
                    spscore = int.Parse(Console.ReadLine()!);
                    winCondition = new Specificscore(spscore);
                    break;
            }
        }

        Console.Clear();

        IValidPlay validPlay = new ClassicValidPlay();
        int val = 0;

        while (val < 1 || val > 3)
        {
            Console.WriteLine("Que jugada seria valida? \n\n 1. Clasico  \n 2. Solo si el nuemero es menor \n 3. Solo si el numero es mayor");
            val = int.Parse(Console.ReadLine()!);
            val = 1;
            switch (val)
            {
                case 1:
                    validPlay = new ClassicValidPlay();
                    break;

                case 2:
                    validPlay = new SmallerValidPlay();
                    break;

                case 3:
                    validPlay = new BiggerValidPlay();
                    break;
            }
        }


        IJudge judge = new Judge(stopcondition, HowTogetScore, winCondition, validPlay);

        Console.WriteLine("Desea que cambie la direccion del juego cada vez que alguien se pase?, si o no");
        char change = Console.ReadLine()![0];
        bool changedirection = (change == 's' || change == 'S') ? true : false;

        while (!judge.ValidSettings(cadauno, max, cantplay))
        {
            Console.Clear();
            Console.WriteLine("Escriba el doble maximo de las Tokens");
            // max = int.Parse(Console.ReadLine()!);
            max = 9;
            Console.WriteLine("Escriba cuantas Tokens se van a repartir a cada jugador");
            // cadauno = int.Parse(Console.ReadLine()!);
            cadauno = 5;
            Console.WriteLine("Escriba cuantos jugadores van a jugar");
            // int.TryParse(Console.ReadLine(),out cantplay);
            cantplay = 5;
        }

        IPlayer[] players = new Player[cantplay];

        for (int i = 0; i < cantplay; i++)
        {
            Console.Clear();

            int a = -1;

            while (a > 3 || a < 0)
            {
                Console.WriteLine("Elija la estrategia para el Player {0}. \n \n ➤ Escriba 0 para un jugador semi inteligente. \n ➤ Escriba 1 para jugador botagorda. \n ➤ Escriba 2 para jugador random. ", i + 1);


                //a = int.Parse(Console.ReadLine()!);
                a = 2;
            }

            switch (a)
            {
                case 0:
                    players[i] = new Player(new List<Token>(), i + 1, new SemiSmart());
                    break;

                case 1:
                    players[i] = new Player(new List<Token>(), i + 1, new BGStrategy());
                    break;

                case 2:
                    players[i] = new Player(new List<Token>(), i + 1, new RandomStrategy());
                    break;
            }
        }

        #endregion

        return new Game(new Board(new List<Token>()), players, false, max, cantplay, cadauno, judge, true);
    }


}




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

    private void SelectGameTypes()
    {
        for (int i = 0; i < Games.Length; i++)
        {


            if (i > 0 && observer.Msg("Desea que el juego tenga las mismas configuaraciones que el anterior? si/no"))
            {
                int x = i - 1;
                Games[i] = Games[x].Clone();//Clona la partida para que no existan 
                //conflictos al pasarse a 
            }
            else
            {
                Games[i] = observer.PedirTipoJuego();
            }


            Debug.Assert(Games[i] != null, "No es null");
        }
    }

    public void run()
    {
        SelectGameTypes();
        for (int i = 0; i < Games.Length; i++)
        {
            Game game = Games[i];

            PlayAGame(game);
            System.Console.WriteLine("Se finalizo el juego");

            if (!ContinueGames()) observer.WriteStats(game); break;
            observer.WriteStats(game);
        }

    }

    public bool ContinueGames()
    {
        return observer.Msg("Desea seguir jugando?, si / no");
    }

    private bool PlayAGame(Game game)
    {
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

                Thread.Sleep(100);
            }


        }
        return true;

    }

    private Token Turno(IPlayer player, Game game)
    {
        return player.BestPlay(game);
    }

  

}


public class Msg
{
    public IPlayer player { private set; get; }

    public void PlayerMsg(IPlayer player)
    {
        this.player = player;
    }
}

public interface ICloneable<T>: ICloneable{
    new T Clone();
    Object ICloneable.Clone() => Clone();
}

