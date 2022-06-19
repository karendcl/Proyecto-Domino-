namespace Game;

//El observer es el encargado de establecer el contacto entre el backend y el frontend
public class Observer
{
    #region Variables globales 

    int cadauno = 0;
    int cantplay = 0;
    int max = 0;


    #endregion
    //Se encarga de publicar en consola los msg
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

    #region Create a Champion

    public Championship CreateAChampion()
    {
        int cantPartidas = CantPartidas();
        ChampionJudge judge = ChooseChampionJudge();
        List<IPlayer> players = ChampionPlayers();

        return new Championship(cantPartidas, judge, players);
    }

    private ChampionJudge ChooseChampionJudge()

    {
        IStopGame<Game, IPlayer> stop = ChooseStopChampion();
        IWinCondition<Game, IPlayer> win = ChooseWinCondition();
        IValidPlayChampion<Game, IPlayer> valid = ChooseValidChampion();
        IGetScore<IPlayer> score = ChooseChampionGetScore();
        return new ChampionJudge(stop, win, valid, score);
    }



    private IValidPlayChampion<Game, IPlayer> ChooseValidChampion()
    {
        return new ValidChampion();
    }
    private IWinCondition<Game, IPlayer> ChooseWinCondition()
    {
        System.Console.WriteLine("Que porcentaje desea que se acabe el torneo");
        int x = -1;
        int.TryParse(Console.ReadLine()!, out x);
        return new WinChampion(x);
    }
    private IGetScore<IPlayer> ChooseChampionGetScore()
    {
        System.Console.WriteLine("Se crea por defecto");

        return new ScoreChampionNormal(null);


    }

    private IStopGame<Game, IPlayer> ChooseStopChampion()
    {
        int x = -1;
        if (Msg("Desea que se acabe el campeonato por una que un jugador tiene mas que una cant de puntos? "))
        {
            System.Console.WriteLine("Escriba la cantidad de puntos maximos a tener por un jugador");

            int.TryParse(Console.ReadLine()!, out x);
            return new StopChampion(x);
        }
        return new StopChampion(x);

    }
    private int CantPartidas()
    {
        int x = 0;
        while (x < 1)
        {
            System.Console.WriteLine("Cuantas partidas desea jugar");
            x = int.Parse(Console.ReadLine()!);
        }
        return x;
    }
    private List<IPlayer> ChampionPlayers()
    {
        if (Msg("Desea tener jugadores a nivel de torneo"))
        {
            return ChoosePlayers().ToList<IPlayer>();
        }
        return new List<IPlayer>() { };

    }
    private IPlayer[] ChoosePlayers()
    {
        IPlayer[] players = new Player[cantplay];

        for (int i = 0; i < cantplay; i++)
        {
            Console.Clear();

            int a = -1;

            while (a > 3 || a < 0)
            {
                Console.WriteLine("Elija la estrategia para el Player {0}. \n \n ➤ Escriba 0 para un jugador semi inteligente. \n ➤ Escriba 1 para jugador botagorda. \n ➤ Escriba 2 para jugador random. ", i + 1);


                int.TryParse(Console.ReadLine()!, out a);
                //a = 2;
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

        return players;
    }

    #endregion

    #region  Create Game
    //Elegir tipo de juego
    private IStopGame<IPlayer, Token> ChooseStopGame(bool ConfGame = false)
    {
        IStopGame<IPlayer, Token> stopcondition = new Classic();

        int stop = 0;

        while (stop < 1 || stop > 2)
        {
            if (ConfGame) { Console.WriteLine("Que tipo de criterio quiere para terminar el juego? \n\n 1. Clasico (se tranque o alguien se pegue) \n 2. Que alguien tenga un score especifico"); }
            stop = 1;
            if (ConfGame) int.TryParse(Console.ReadLine(), out stop);
            //stop = 1;
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

        return stopcondition;
    }

    public IGetScore<Token> ChooseGetScore(bool ConfGame = false)
    {
        IGetScore<Token> HowTogetScore = new ClassicScore();

        int score = 0;

        while (score < 1 || score > 2)
        {
            if (ConfGame) { Console.WriteLine("Que manera quiere para contar las fichas? \n\n 1. Clasico (la suma del valor de sus partes) \n 2. Si la ficha es doble, vale el doble de puntos"); }

            score = 1;

            if (ConfGame) { score = int.Parse(Console.ReadLine()!); }

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
        return HowTogetScore;
    }

    public IWinCondition<IPlayer, Token> ChooseWinCondition(bool ConfGame = false)
    {
        IWinCondition<IPlayer, Token> winCondition = new MinScore();

        int winConditionn = 0;

        while (winConditionn < 1 || winConditionn > 3)
        {
            Console.WriteLine("Una vez acabe el juego, quien ganaria? \n \n 1. El que tenga mas puntos \n 2. El que tenga menos puntos \n 3. El que tenga una catidad de puntos especificos");
            winConditionn = 1;
            if (ConfGame) winConditionn = int.Parse(Console.ReadLine()!);
            // winConditionn = 1;
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
        return winCondition;
    }

    public IValidPlay ChooseValidPlay(bool ConfGame = false)
    {
        IValidPlay validPlay = new ClassicValidPlay();
        int val = 0;

        while (val < 1 || val > 3)
        {
            Console.WriteLine("Que jugada seria valida? \n\n 1. Clasico  \n 2. Solo si el nuemero es menor \n 3. Solo si el numero es mayor");

            val = 1;
            if (ConfGame) val = int.Parse(Console.ReadLine()!);

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

        return validPlay;
    }


    private IJudge<IPlayer, Token> ChooseJugde(IStopGame<IPlayer, Token> stopcondition, IGetScore<Token> HowTogetScore, IWinCondition<IPlayer, Token> winCondition, IValidPlay validPlay, bool ConfGame = false)
    {
        IJudge<IPlayer, Token> judge = new Judge(stopcondition, HowTogetScore, winCondition, validPlay);

        Console.WriteLine("Desea que cambie la direccion del juego cada vez que alguien se pase?, si o no");
        char change = 's';
        if (ConfGame) { change = Console.ReadLine()![0]; }
        bool changedirection = (change == 's' || change == 'S') ? true : false;

        while (!judge.ValidSettings(cadauno, max, cantplay))
        {
            Console.Clear();
            Console.WriteLine("Escriba el doble maximo de las Tokens");
            int.TryParse(Console.ReadLine()!, out max);
            // max = 9;
            Console.WriteLine("Escriba cuantas Tokens se van a repartir a cada jugador");
            int.TryParse(Console.ReadLine()!, out cadauno);
            //cadauno = 5;
            Console.WriteLine("Escriba cuantos jugadores van a jugar");
            int.TryParse(Console.ReadLine(), out cantplay);
            //cantplay = 5;
        }

        return judge;
    }



    private Game ChooseAGame(bool ConfGame = false)//Seleccionar un modo de juego
    {
        if (ConfGame && Msg("Quiere Jugar con las configuraciones predeterminadas? Si/No")) { ConfGame = true; }//Sleccionar si se quiere modo de juego normal

        IStopGame<IPlayer, Token> stopcondition = ChooseStopGame(ConfGame);
        IGetScore<Token> HowTogetScore = ChooseGetScore(ConfGame);
        IWinCondition<IPlayer, Token> winCondition = ChooseWinCondition(ConfGame);
        IValidPlay validPlay = ChooseValidPlay(ConfGame);
        IJudge<IPlayer, Token> judge = ChooseJugde(stopcondition, HowTogetScore, winCondition, validPlay, ConfGame);
        IPlayer[] players = ChoosePlayers();
        return new Game(new Board(new List<Token>()), players, false, max, cantplay, cadauno, judge, true);
    }

    //Crea los modos de juego
    public Game[] SelectGameTypes(Game[] Games)
    {
        if (Msg("Quiere Jugar todos los partidos con las configuraciones predeterminadas? Si/No"))
        {
            for (int i = 0; i < Games.Length; i++)
            {
                if (i > 0 && Msg("Desea que el juego tenga las mismas configuaraciones que el anterior? si/no"))
                {
                    int x = i - 1;
                    Games[i] = Games[x].Clone();//Clona la partida para que no existan problemas de referencia

                    // Debug.Assert(Games[i].board.board.Count == 0, "Se clono correctamente");
                }
                else
                {
                    Games[i] = ChooseAGame();
                }
            }
            return Games;
        }

        for (int i = 0; i < Games.Length; i++)
        {

            if (i > 0 && Msg("Desea que el juego tenga las mismas configuaraciones que el anterior? si/no"))
            {
                int x = i - 1;
                Games[i] = Games[x].Clone();//Clona la partida para que no existan problemas de referencia

                // Debug.Assert(Games[i].board.board.Count == 0, "Se clono correctamente");
            }
            else
            {
                Games[i] = ChooseAGame(true);
            }

            //  Debug.Assert(Games[i] != null, "No es null");
        }

        return Games;
    }
    #endregion 
}


