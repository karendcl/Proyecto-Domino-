namespace Game;



public class Events
{
    public event Func<string, int>? Asksmint;

    public event Predicate<string>? BooleanAsk;

    public event Action<ChampionStatus>? Status;

    public void Run(ChampionStatus status) => Status(status);

    public int IntermediarioInt(string msg) => Asksmint(msg);

    public bool IntermediarioBool(string msg) => BooleanAsk(msg);



}



public class ChampionStart
{
    public observador obs { get; protected set; }

    public ChampionStart(observador obs)
    {
        this.obs = obs;
    }

    int CantTokenPerPerson = 0;
    int cantPlayers = 0;
    int MaxDouble = 0;
    public event Func<string, int>? Asksint;

    public event Func<string, bool>? BooleanAsk;
    public event Action<ChampionStatus>? PrintChampionStatus;


    public bool Run()
    {
        Asksint += IntermediarioInt;
        BooleanAsk += IntermediarioBool;

        Championship champion = this.CreateAChampion();
        champion.status += Intermediario;
        champion.Choose += IntermediarioBool;
        //  PrintChampionStatus += this.Intermediario;
        champion.Run();
        return true;
    }

    protected void Intermediario(ChampionStatus status) => obs.PrintChampionStatus(status);
    protected bool IntermediarioBool(string msg) => obs.BoolResponses(msg);
    protected int IntermediarioInt(string msg) => obs.IntResponses(msg);

    #region Create a Champion

    public Championship CreateAChampion()
    {
        int cantPartidas = CantPartidas();
        ChampionJudge judge = ChooseChampionJudge();
        (PlayersCoach players, List<Game> games) games = SelectGameTypes(cantPartidas);
        return new Championship(cantPartidas, judge, games.players, games.games);
    }




    protected ChampionJudge ChooseChampionJudge()

    {
        IStopGame<Game, (Game, IPlayer)> stop = ChooseStopChampion();
        IWinCondition<Game, (Game, IPlayer)> win = ChooseWinCondition();
        IValidPlay<List<GameStatus>, IPlayer, bool> valid = ChooseValidChampion();
        IGetScore<(Game, IPlayer)> score = ChooseChampionGetScore();
        return new ChampionJudge(stop, win, valid, score);
    }



    protected IValidPlay<List<GameStatus>, IPlayer, bool> ChooseValidChampion()
    {
        //1 Forma por puntuacion especifica de % de derrortas 
        string Porcent = "Por un % de veces que se queda por debajo del 50% en la lista ";

        //2 Forma por perder x veces consecutivas 
        string consecutivas = "Por perder n veces consecutivas ";
        string msg = "Bajo que criterio desea que el jugador sea descalificado del torneo " + '\n' + "1 => " + Porcent + "\n" + "2 => " + consecutivas;
        int x = 0;
        do
        {
            x = this.Asksint(msg);
        } while (x < 1 || x > 2);
        if (x == 1)
        {
            double z = 0;
            do
            {
                z = this.Asksint("Que % desea en int ");
            } while (z < 0 || z > 100);
            z = z / 100;
            return new ValidChampion(z);
        }
        int y = 0;
        do
        {
            y = this.Asksint("Número de veces consecutivas ");
        } while (x < 1);

        return new ValidChampionPerdidasConsecutivas(y);


    }
    protected IWinCondition<Game, (Game, IPlayer)> ChooseWinCondition()
    {
        int x = -1;
        do
        {
            x = this.Asksint("Que porcentaje desea que se acabe el torneo");
        } while (x < 1);

        return new WinChampion(x);
    }
    protected IGetScore<(Game, IPlayer)> ChooseChampionGetScore()
    {
        // System.Console.WriteLine("Se crea por defecto");
        return new ScoreChampionNormal();
    }

    protected IStopGame<Game, (Game, IPlayer)> ChooseStopChampion()
    {
        int x = -1;
        if (this.BooleanAsk("Desea que se acabe el campeonato puntuación máxima? "))
        {
            x = this.Asksint("Escriba la cantidad de puntos maximos a tener por un jugador");
            //En caso de -1 no hay limite
            return new StopChampion(x);
        }
        return new StopChampion(x);

    }
    protected int CantPartidas()
    {
        int x = 0;
        do
        {
            x = this.Asksint("Cuantas partidas desea jugar");
        } while (x < 1);

        return x;

    }
    protected List<IPlayer> ChampionPlayers(int cantPlayers)
    {


        List<IPlayer> players = new List<IPlayer>() { };

        for (int i = 0; i < cantPlayers; i++)
        {
            int r = i + 1;
            if (this.BooleanAsk("Desea que el jugador numero" + " " + r.ToString() + " " + "juegue? Si/no"))
            {
                players.Add(ChoosePlayers(i + 1));
            }
        }


        return players;

    }
    protected IPlayer ChoosePlayers(int id)
    {//Devuelve los players

        IPlayer player = new Player(id);

        int a = -1;

        do
        {
            a = this.Asksint("Elija la estrategia para el Player {0}. \n \n ➤ Escriba 0 para un jugador semi inteligente. \n ➤ Escriba 1 para jugador botagorda. \n ➤ Escriba 2 para jugador random. " + id.ToString());
        } while (a > 2 || a < 0);

        switch (a)
        {
            case 0:
                player.AddStrategy(new SemiSmart());
                break;

            case 1:
                player.AddStrategy(new BGStrategy());
                break;

            case 2:
                player.AddStrategy(new RandomStrategy());
                break;
        }


        return player;
    }

    #endregion

    #region  Create Game
    //Elegir tipo de juego
    protected IStopGame<IPlayer, Token> ChooseStopGame(bool ConfGame = false)
    {
        IStopGame<IPlayer, Token> stopcondition = new Classic();

        int stop = 0;
        if (ConfGame)
        {
            stop = 1;
        }

        else
        {

            do
            {
                stop = this.Asksint("Que tipo de criterio quiere para terminar el juego? \n\n 1. Clasico (se tranque o alguien se pegue) \n 2. Que alguien tenga un score especifico");
            } while (stop < 1 || stop > 2);

        }

        switch (stop)
        {
            case 1:
                stopcondition = new Classic();
                break;

            case 2:

                int specificscore = this.Asksint("Que score especifico desea");
                stopcondition = new CertainScore(specificscore);
                break;
        }


        return stopcondition;
    }

    public IGetScore<Token> ChooseGetScore(bool ConfGame = false)
    {
        IGetScore<Token> HowTogetScore = new ClassicScore();

        int score = 0;

        while (score < 1 || score > 2)
        {
            score = 1;
            if (!ConfGame)
            {
                do
                {
                    score = this.Asksint("Que manera quiere para contar las fichas? \n\n 1. Clasico (la suma del valor de sus partes) \n 2. Si la ficha es doble, vale el doble de puntos");
                } while (score < 1 || score > 2);

            }

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

    public IWinCondition<(IPlayer player, List<Token> hand), Token> ChooseWinCondition(bool ConfGame = false)
    {
        IWinCondition<(IPlayer player, List<Token> hand), Token> winCondition = new MinScore();

        int winConditionn = 0;
        if (!ConfGame)
        {
            do
            {
                winConditionn = this.Asksint("Una vez acabe el juego, quien ganaria? \n \n 1. El que tenga mas puntos \n 2. El que tenga menos puntos \n 3. El que tenga una catidad de puntos especificos");
            } while (winConditionn < 1 || winConditionn > 3);
        }

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

        return winCondition;
    }

    public IValidPlay<IBoard, Token, ChooseStrategyWrapped> ChooseValidPlay(IEqualityComparer<Token> equalityComparer, IComparer<Token> Comparer, bool ConfGame = false)
    {
        IValidPlay<IBoard, Token, ChooseStrategyWrapped> validPlay = new ClassicValidPlay(equalityComparer, Comparer);
        int val = 0;

        while (val < 1 || val > 3)
        {
            if (!ConfGame)
            {
                val = this.Asksint("Que jugada seria valida? \n\n 1. Clasico  \n 2. Solo si el nuemero es menor \n 3. Solo si el numero es mayor");

            }
            else
            {
                val = 1;
            }



            switch (val)
            {
                case 1:
                    validPlay = new ClassicValidPlay(equalityComparer, Comparer);
                    break;

                case 2:
                    validPlay = new SmallerValidPlay(equalityComparer, Comparer);
                    break;

                case 3:
                    validPlay = new BiggerValidPlay(equalityComparer, Comparer);
                    break;
            }
        }

        return validPlay;
    }


    protected Judge ChooseJugde(IStopGame<IPlayer, Token> stopcondition, IGetScore<Token> HowTogetScore, IWinCondition<(IPlayer player, List<Token> hand), Token> winCondition, IValidPlay<IBoard, Token, ChooseStrategyWrapped> validPlay, bool ConfGame = false)
    {

        Judge judge = new Judge(stopcondition, HowTogetScore, winCondition, validPlay);
        bool changedirection = false;
        if (ConfGame) { changedirection = this.BooleanAsk("Desea que cambie la direccion del juego cada vez que alguien se pase"); }

        while (!this.ValidSettings(CantTokenPerPerson, MaxDouble, cantPlayers))
        {
            this.MaxDouble = this.Asksint("Escriba el doble maximo de las Tokens");

            this.CantTokenPerPerson = this.Asksint("Escriba cuantas Tokens se van a repartir a cada jugador");

            this.cantPlayers = this.Asksint("Escriba cuantos jugadores van a jugar");
        }

        return judge;
    }

    protected TokensManager ChooseATokenManager(int TokensForEach, IComparer<Token> comparer, IEqualityComparer<Token> equalityComparer)
    {
        string ask = "Que tipo de ficha desea jugar ";
        string normalToken = " 1=>Tokens clasicos";
        string EnergyGenerator = " 2=> Termoelectricas ";
        int choose = 0;
        do
        {
            choose = this.Asksint(ask + normalToken + EnergyGenerator);
        } while (choose > 2 && choose < 1);

        IntTokenGenerator generator = new IntTokenGenerator();
        List<Token> tokens = generator.CreateTokens(this.MaxDouble);

        return new TokensManager(TokensForEach, comparer, equalityComparer, tokens);

    }

    protected IComparer<Token> ChooseATokenComparerCriteria()
    {
        return new ComparerTokens();
    }
    protected IEqualityComparer<Token> ChooseATokenEqualityCriteria()
    {

        return new IEquatablePorPedazos();
    }

    protected Game ChooseAGame(bool ConfGame = false, bool ChampionPlayers = false)//Seleccionar un modo de juego
    {
        if (ConfGame) { ConfGame = true; }//Sleccionar si se quiere modo de juego normal

        IStopGame<IPlayer, Token> stopcondition = ChooseStopGame(ConfGame);

        IGetScore<Token> HowTogetScore = ChooseGetScore(ConfGame);

        IWinCondition<(IPlayer player, List<Token> hand), Token> winCondition = ChooseWinCondition(ConfGame);

        IComparer<Token> tokenComparer = ChooseATokenComparerCriteria();

        IEqualityComparer<Token> equalityComparer = ChooseATokenEqualityCriteria();

        IValidPlay<IBoard, Token, ChooseStrategyWrapped> validPlay = ChooseValidPlay(equalityComparer, tokenComparer, ConfGame);

        Judge judge = ChooseJugde(stopcondition, HowTogetScore, winCondition, validPlay, ConfGame);

        TokensManager tokensManager = ChooseATokenManager(this.CantTokenPerPerson, tokenComparer, equalityComparer);

        return new Game(false, this.MaxDouble, this.CantTokenPerPerson, judge, tokensManager);
    }

    //Crea los modos de juego
    public (PlayersCoach, List<Game>) SelectGameTypes(int CantPartidas)
    {
        List<IPlayer> tt = new List<IPlayer>() { };
        for (int i = 0; i < this.cantPlayers; i++)
        {
            tt.Add(new Player(i));
        }
        PlayersCoach coach = new PlayersCoach(tt);

        Game[] Games = new Game[CantPartidas];
        bool ConfGame = false;
        if (this.BooleanAsk("Quiere Jugar todos los partidos con las configuraciones predeterminadas"))
        { ConfGame = true; }
        for (int i = 0; i < Games.Length; i++)
        {
            if (i > 0 && this.BooleanAsk("Desea que el juego tenga las mismas configuaraciones que el anterior"))
            {
                int x = i - 1;
                Games[i] = Games[x].Clone();//Clona la partida para que no existan problemas de referencia
                coach.CloneLastGame(i);

            }
            else
            {
                Games[i] = ChooseAGame(ConfGame);
                coach.AddPlayers(i, ChampionPlayers(this.cantPlayers));
            }
        }
        return (coach, Games.ToList<Game>());



        // return (coach, Games.ToList<Game>());
    }
    #endregion

    protected virtual bool ValidSettings(int TokensForEach, int MaxDoble, int players)
    {
        int totalamount = 0;

        if (TokensForEach == 0 || MaxDoble == 0 || players == 0) return false;

        for (int i = 0; i <= MaxDoble + 1; i++)
        {
            totalamount += i;
        }

        return (TokensForEach * players > totalamount) ? false : true;
    }
}


public class observador
{
    public observador() { }

    public int IntResponses(string arg)
    {
        Console.Clear();
        System.Console.WriteLine(arg);
        int x = -1;
        int.TryParse(Console.ReadLine(), out x);
        Console.Clear();
        return x;
    }

    public bool BoolResponses(string arg)
    {
        Console.Clear();
        string x = string.Empty;
        bool b = (x == string.Empty || x == null);
        bool a = true;
        do
        {
            Console.Clear();
            System.Console.WriteLine(arg + "  ?" + " Sí/No");
            x = Console.ReadLine()!.ToLower();
            if(x!=null)
            {
                a = (x[0] == 's' || x[0] == 'n');
            }
            
        } while (!a);
        Console.Clear();
        if (x[0] == 's') return true;
        return false;
    }

    public void PrintChampionStatus(ChampionStatus championStatus)
    {
        GameStatus gameStatus = championStatus.gameStatus;
        if (championStatus.ItsAnGameStatus)
        {

            PrintGameChange(gameStatus);

        }
        else if (championStatus.ItsAFinishGame)
        {
            PrintFinishGame(gameStatus);
        }
        if (championStatus.FinishChampion)
        {

        }

    }

    protected void PrintFinishChampion(ChampionStatus championStatus)
    {
        foreach (var item in championStatus.playerStrats)
        {
            System.Console.WriteLine(item.player + " Puntuacion " + item.punctuation);
        }


        System.Console.WriteLine("Ganadores del Torneo");

        foreach (var item in championStatus.Winners)
        {
            System.Console.WriteLine(item);
        }
    }

    protected void PrintGameChange(GameStatus gameStatus)
    {

        System.Console.WriteLine(gameStatus.board);
        // Thread.Sleep(1500);
        Console.ReadKey();
        Console.Clear();
        System.Console.WriteLine(gameStatus.actualPlayer);
        //  Thread.Sleep(2000);
        // Console.ReadKey();
        Console.Clear();
        System.Console.WriteLine(gameStatus.PlayerActualHand);
        //  Thread.Sleep(1500);
        // Console.ReadKey();
        Console.Clear();


    }

    protected void PrintFinishGame(GameStatus gameStatus)
    {
        foreach (var item in gameStatus.playerStrats)
        {
            System.Console.WriteLine(item);
            //  Thread.Sleep(1500);
            // Console.ReadKey();
            Console.Clear();
        }

        System.Console.WriteLine("Ganadores de la partida");

        foreach (var item in gameStatus.winners)
        {
            System.Console.WriteLine(item);
            //   Console.ReadKey();
            //   Thread.Sleep(500);
            Console.Clear();
        }

    }


}
