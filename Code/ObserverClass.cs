namespace Game;



public class Events
{

    public event Func<string, int>? Asksmint;

    public event Predicate<Orders>? BooleanAsk;

    public event Action<ChampionStatus>? Status;

    public void Run(ChampionStatus status) => Status(status);

    public int IntermediarioInt(string msg) => Asksmint(msg);

    public bool IntermediarioBool(Orders orders) => BooleanAsk(orders);



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

    public event Action<ChampionStatus>? PrintChampionStatus;
    public event Predicate<string>? BooleanAsk;

    public bool ChooseBool(string msg)
    {
        return this.BooleanAsk(msg);


    }

    public bool ChooseEnum(Orders orders)
    {
        System.Console.WriteLine(orders);
        bool x;

        bool.TryParse(Console.ReadLine()!, out x);
        return x;
    }

    protected int ChooseInt(string msg, int min, int max)
    {
        int x = -1;
        do
        {
            x = this.Asksint(msg);
        } while (x < min && x > max);
        return x;
    }

    public bool Run()
    {
        Asksint += IntermediarioInt;
        BooleanAsk += IntermediarioBool;

        Championship champion = this.CreateAChampion();
        champion.status += Intermediario;
        champion.CanContinue += IntermediarioEnum;
        PrintChampionStatus += this.Intermediario;
        champion.Run();
        return true;
    }

    protected void Intermediario(ChampionStatus status) => obs.PrintChampionStatus(status);

    protected bool IntermediarioEnum(Orders orders)
    {
        Thread.Sleep(500);
        return true;
    }
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

        IWinCondition<Game, (Game, Player)> win = ChooseWinCondition();
        IStopGame<List<Game>, (Game, Player)> stop = ChooseStopChampion(win);
        IValidPlay<List<Game>, Player, bool> valid = ChooseValidChampion();
        IGetScore<(Game, Player)> score = ChooseChampionGetScore();

        string NormalJugde = "1- Juez Honesto";
        string CorruptionChampionJugde = "2- Juez Corrupto";
        string msg = NormalJugde + "\n" + CorruptionChampionJugde;
        int choose = -1;
        choose = ChooseInt(msg, 0, 3);
        ChampionJudge championJudge = new ChampionJudge(stop, win, valid, score);
        if (choose == 2) championJudge = new CorruptionChampionJugde(stop, win, valid, score);

        return championJudge;
    }



    protected IValidPlay<List<Game>, Player, bool> ChooseValidChampion()
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
    protected IWinCondition<Game, (Game, Player)> ChooseWinCondition()
    {
        int x = -1;
        do
        {
            x = this.Asksint("Que porcentaje de partidas desea para que un jugador gane");
        } while (x < 1);

        return new WinChampion(x);
    }
    protected IGetScore<(Game, Player)> ChooseChampionGetScore()
    {
        // System.Console.WriteLine("Se crea por defecto");
        return new ScoreChampionNormal();
    }

    protected IStopGame<List<Game>, (Game, Player)> ChooseStopChampion(IWinCondition<Game, (Game, Player)> winCondition)
    {
        int x = -1;
        string y = " Elija el criterio de finalizacion del juego";
        string a = "1.Cuando un jugador acumule una cantidad de puntos";
        string b = "2. Cuando Existan n Cant de ganadores del Torneo";
        string msg = y + "\n" + a + "\n" + b;


        x = ChooseInt(msg, 0, 3);
        IStopGame<List<Game>, (Game, Player)> stop = new StopChampionPerPoints(-1);
        if (x == 1)
        {
            int s = ChooseInt("Diga la puntuacion", 0, 10000);
            stop = new StopChampionPerPoints(s);
        }
        else
        {
            int s = ChooseInt("Diga la cantidad de ganadores maximos", 0, this.cantPlayers);
            stop = new StopChampionPerHaveAWinner(winCondition, s);
        }
        return stop;

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
    protected List<Player> ChampionPlayers(int cantPlayers)
    {


        List<Player> players = new List<Player>() { };

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
    protected Player ChoosePlayers(int id)
    {//Devuelve los players

        Player player = new Player(id);

        int a = -1;

        do
        {
            a = this.Asksint("$Elija la estrategia para el Player {id.}. \n \n ➤ Escriba 0 para un jugador semi inteligente. \n ➤ Escriba 1 para jugador botagorda. \n ➤ Escriba 2 para jugador random. " + id.ToString());
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
    protected IStopGame<Player, IToken> ChooseStopGame(bool ConfGame = false)
    {
        IStopGame<Player, IToken> stopcondition = new Classic();

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

    public IGetScore<IToken> ChooseGetScore(bool ConfGame = false)
    {
        IGetScore<IToken> HowTogetScore = new ClassicScore();

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

    public IWinCondition<(Player player, List<IToken> hand), IToken> ChooseWinCondition(bool ConfGame = false)
    {
        IWinCondition<(Player player, List<IToken> hand), IToken> winCondition = new MinScore();

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

    public IValidPlay<Board, IToken, ChooseStrategyWrapped> ChooseValidPlay(IEqualityComparer<IToken> equalityComparer, IComparer<IToken> Comparer, bool ConfGame = false)
    {
        IValidPlay<Board, IToken, ChooseStrategyWrapped> validPlay = new ClassicValidPlay(equalityComparer, Comparer);
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


    protected Judge ChooseJugde(IStopGame<Player, IToken> stopcondition, IGetScore<IToken> HowTogetScore, IWinCondition<(Player player, List<IToken> hand), IToken> winCondition, IValidPlay<Board, IToken, ChooseStrategyWrapped> validPlay, bool ConfGame = false)
    {

        Judge judge = new Judge(stopcondition, HowTogetScore, winCondition, validPlay);


        while (!this.ValidSettings(CantTokenPerPerson, MaxDouble, cantPlayers))
        {
            this.MaxDouble = this.Asksint("Escriba el doble maximo de las Tokens");

            this.CantTokenPerPerson = this.Asksint("Escriba cuantas Tokens se van a repartir a cada jugador");

            this.cantPlayers = this.Asksint("Escriba cuantos jugadores van a jugar");
        }

        return judge;
    }

    protected TokensManager ChooseATokenManager(int TokensForEach, IComparer<IToken> comparer, IEqualityComparer<IToken> equalityComparer)
    {
        string ask = "Que tipo de ficha desea jugar ";
        string normalToken = " 1=>Tokens clasicos";
        string EnergyGenerator = " 2=> Termoelectricas ";
        int choose = 0;
        do
        {
            choose = this.Asksint(ask + normalToken + "\n" + EnergyGenerator);
        } while (choose > 2 && choose < 1);
        List<IToken> tokens = new List<IToken>();
        switch (choose)
        {
            case 1:
                IntTokenGenerator generator = new IntTokenGenerator();
                tokens = generator.CreateTokens(this.MaxDouble);
                break;
            case 2:
                ElectricGeneratorGenerate generate = new ElectricGeneratorGenerate();
                tokens = generate.GetToken(this.MaxDouble);
                break;

        }

        return new TokensManager(TokensForEach, comparer, equalityComparer, tokens);

    }

    protected IComparer<IToken> ChooseATokenComparerCriteria()
    {
        return new ComparerTokens();
    }
    protected IEqualityComparer<IToken> ChooseATokenEqualityCriteria()
    {

        return new IEquatablePorCaras();
    }

    protected Game ChooseAGame(bool ConfGame = false, bool ChampionPlayers = false)//Seleccionar un modo de juego
    {
        if (ConfGame) { ConfGame = true; }//Sleccionar si se quiere modo de juego normal

        IStopGame<Player, IToken> stopcondition = ChooseStopGame(ConfGame);

        IGetScore<IToken> HowTogetScore = ChooseGetScore(ConfGame);

        IWinCondition<(Player player, List<IToken> hand), IToken> winCondition = ChooseWinCondition(ConfGame);

        IComparer<IToken> tokenComparer = ChooseATokenComparerCriteria();

        IEqualityComparer<IToken> equalityComparer = ChooseATokenEqualityCriteria();

        IValidPlay<Board, IToken, ChooseStrategyWrapped> validPlay = ChooseValidPlay(equalityComparer, tokenComparer, ConfGame);

        Judge judge = ChooseJugde(stopcondition, HowTogetScore, winCondition, validPlay, ConfGame);

        TokensManager tokensManager = ChooseATokenManager(this.CantTokenPerPerson, tokenComparer, equalityComparer);

        return new Game(false, this.MaxDouble, judge, tokensManager);
    }

    //Crea los modos de juego
    public (PlayersCoach, List<Game>) SelectGameTypes(int CantPartidas)
    {
        List<Player> tt = new List<Player>() { };
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
                coach.CloneLastGame(x);

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
            if (x != null && x.Length > 0)
            {
                a = (x[0] == 's' || x[0] == 'n');
            }

        } while (!a);
        Console.Clear();
        if (x.Length > 0 && x[0] == 's') return true;
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
        Console.ReadKey();
        Console.Clear();
        System.Console.WriteLine(gameStatus.PlayerActualHand);
        //  Thread.Sleep(1500);
        Console.ReadKey();
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
