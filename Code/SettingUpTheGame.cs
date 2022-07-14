using System.Reflection;

namespace Game;

public class ChampionStart
{
    public observador obs { get; protected set; }
    int CantTokenPerPerson = 0;
    int cantPlayers = 0;
    int MaxDouble = 0;

    public ChampionStart(observador obs)
    {
        this.obs = obs;
    }

    public event Func<string, int>? Asksint;
    public event Action<ChampionStatus>? PrintChampionStatus;
    public event Predicate<string>? BooleanAsk;
    public bool ChooseBool(string msg)
    {
        return BooleanAsk(msg);
    }

    public bool ChooseEnum(Orders orders)  //???????????????
    {
        System.Console.WriteLine(orders);
        bool x;

        bool.TryParse(Console.ReadLine()!, out x);
        return x;
    }

    protected int ChooseInt(string msg, int min, int max)
    {
        //va a devlver un int, en el rango dado
        Console.Clear();
        int x = -1;

        try
        {
            do
            {
                x = this.Asksint(msg);
            } while (x < min || x > max);
        }
        catch (System.Exception)
        {
            return ChooseInt(msg, min, max);
        }

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
        // Thread.Sleep(500);
        return true;
    }
    protected bool IntermediarioBool(string msg) => obs.BoolResponses(msg);
    protected int IntermediarioInt(string msg) => obs.IntResponses(msg);

    #region Create a Champion

    public Championship CreateAChampion()
    {
        // retorna un nuevo torneo
        int cantPartidas = CantPartidas();
        ChampionJudge judge = ChooseChampionJudge();
        (PlayersCoach players, List<Game> games) games = SelectGameTypes(cantPartidas);
        return new Championship(cantPartidas, judge, games.players, games.games);
    }

    protected ChampionJudge ChooseChampionJudge()
    {
        //retorna un juez del torneo
        IWinCondition<Game, List<IPlayerScore>> win = ChooseWinCondition();
        IStopGame<List<Game>, List<IPlayerScore>> stop = ChooseStopChampion(win);
        IValidPlay<List<Game>, Player, bool> valid = ChooseValidChampion();
        IGetScore<List<IPlayerScore>> score = ChooseChampionGetScore();

        string words = "Que tipo de juez desea para el torneo? ";
        Type[] a = Utils.TypesOfChampionJudge();
        var x = a[GetChoice(a, words)];

        return Activator.CreateInstance(x, stop, win, valid, score) as ChampionJudge;
    }


    protected IValidPlay<List<Game>, Player, bool> ChooseValidChampion()
    {
        //retorna la calidad de descalificacion del torneo
        string words = "Bajo que criterio desee que se descalifique a alguien del torneo \n ";
        Type[] a = Utils.TypesofValidChampion();
        var x = a[GetChoice(a, words)];
        string ask = "Escriba el numero";
        return Activator.CreateInstance(x, ChooseInt(ask, 0, 100)) as IValidPlay<List<Game>, Player, bool>;

    }
    protected IWinCondition<Game, List<IPlayerScore>> ChooseWinCondition()
    {
        //retorna la condicion de ganada del torneo
        string msg = "Que porcentaje de partidas ganadas son necesarias para que un jugador gane el torneo";
        return new WinChampion(ChooseInt(msg, 0, 100));

    }
    protected IGetScore<List<IPlayerScore>> ChooseChampionGetScore()
    {
        //retorna como se selecciona el score de un torneo
        return new ScoreChampionNormal();
    }

    protected IStopGame<List<Game>, List<IPlayerScore>> ChooseStopChampion(IWinCondition<Game, List<IPlayerScore>> winCondition)
    {
        //retorna el criterio de parada del torneo
        string words = "Bajo que criterio desea que se pare el torneo \n ";
        Type[] a = Utils.TypesOfStopConditionTorneo();
        var x = a[GetChoice(a, words)];
        string ask = "Escriba la cantidad";
        if (x.GetConstructors().SelectMany(t => t.GetParameters()).Count() == 2)
            return Activator.CreateInstance(x, winCondition, ChooseInt(ask, 0, 10000)) as IStopGame<List<Game>, List<IPlayerScore>>;
        else return Activator.CreateInstance(x, ChooseInt(ask, 0, 10000)) as IStopGame<List<Game>, List<IPlayerScore>>;

    }
    protected int CantPartidas()
    {
        return ChooseInt("Cuantas partidas desea jugar?", 1, 10000);

    }
    protected List<Player> ChampionPlayers(int cantPlayers)
    {
        //se eligen si los jugadores van a jugar
        List<Player> players = new List<Player>() { };

        for (int i = 0; i < cantPlayers; i++)
        {
            int r = i + 1;
            if (this.BooleanAsk($"Desea que el jugador numero {r} juegue"))
            {
                players.Add(ChoosePlayers(i + 1));
            }
        }

        return players;

    }

    protected IPlayerStrategy ChoosePlayerStrategy(int id)
    {
        //devuelve la estrategia del jugador 
        string words = $"Elija la estrategia para el Player {id}\n ";
        Type[] a = Utils.TypesOfPlayerStrategies();
        var x = a[GetChoice(a, words)];
        return Activator.CreateInstance(x) as IPlayerStrategy;
    }
    protected Player ChoosePlayers(int id)
    {
        //devuelve el tipo de jugador 
        string words = $"Elija el tipo de jugador para el Player {id}\n ";
        Type[] a = Utils.TypesOfPlayer();
        var x = a[GetChoice(a, words)];
        Player player = Activator.CreateInstance(x, id) as Player;
        player.AddStrategy(ChoosePlayerStrategy(id));
        return player;
    }

    #endregion

    #region  Create Game
    //Elegir tipo de juego
    protected IStopGame<Player, IToken> ChooseStopGame(bool ConfGame = false)
    {
        //devuelve el criterio para terminar el juego
        string words = "Que tipo de criterio quiere para terminar el juego? \n ";
        Type[] a = Utils.TypesofStopConditionGame();

        if (ConfGame) return Activator.CreateInstance(a[0]) as IStopGame<Player, IToken>;

        var x = a[GetChoice(a, words)];
        string ask = "Escriba la cantidad de puntos";
        if (x.GetConstructors().SelectMany(t => t.GetParameters()).Count() != 0)
            return Activator.CreateInstance(x, ChooseInt(ask, 0, 10000)) as IStopGame<Player, IToken>;
        else
            return Activator.CreateInstance(x) as IStopGame<Player, IToken>;

    }

    public int GetChoice(Type[] a, string words)
    {

        //recibe el array de todas las opciones a elegir, y devuelve la posicion de la eleccion
        for (int i = 0; i < a.Length; i++)
        {
            PropertyInfo property = a[i].GetProperty("Description", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            string value = (string)property.GetValue(null, null);
            words += $"\n {i + 1}. {value} ";
        }

        return ChooseInt(words, 1, a.Length + 1) - 1;

    }
    public IGetScore<IToken> ChooseGetScore(bool ConfGame = false)
    {
        //devuelve como se calcula el score de la ficha en el juego
        string words = "Como desea que se cuente el score de la ficha \n ";
        Type[] a = Utils.TypesOfScoreGame();

        if (ConfGame) return Activator.CreateInstance(a[0]) as IGetScore<IToken>;
        var x = a[GetChoice(a, words)];
        return Activator.CreateInstance(x) as IGetScore<IToken>;

    }

    public IWinCondition<(Player player, List<IToken> hand), IToken> ChooseWinCondition(bool ConfGame = false)
    {
        //retorna la condicion de ganar del juego
        string words = "Una vez acabe el juego, quien ganaria? \n ";
        Type[] a = Utils.TypesofWinConditionGame();
        if (ConfGame) return Activator.CreateInstance(a[0]) as IWinCondition<(Player player, List<IToken> hand), IToken>;
        var x = a[GetChoice(a, words)];

        return Activator.CreateInstance(x) as IWinCondition<(Player player, List<IToken> hand), IToken>;

    }

    public IValidPlay<Board, IToken, ChooseStrategyWrapped> ChooseValidPlay(IEqualityComparer<IToken> equalityComparer, IComparer<IToken> Comparer, bool ConfGame = false)
    {
        //retorna la jugada que es valida para el juego
        string words = "Que tipo de jugada valida desea para el juego \n ";
        Type[] a = Utils.TypesofValidPlayGame();
        if (ConfGame) return Activator.CreateInstance(a[0], equalityComparer, Comparer) as IValidPlay<Board, IToken, ChooseStrategyWrapped>;
        var x = a[GetChoice(a, words)];

        return Activator.CreateInstance(x, equalityComparer, Comparer) as IValidPlay<Board, IToken, ChooseStrategyWrapped>;
    }


    protected Judge ChooseJugde(IStopGame<Player, IToken> stopcondition, IGetScore<IToken> HowTogetScore, IWinCondition<(Player player, List<IToken> hand), IToken> winCondition, IValidPlay<Board, IToken, ChooseStrategyWrapped> validPlay, bool ConfGame = false)
    {
        //retorna el tipo de juez para el juego

        string ask = "Que tipo de juez quiere para el juego \n";
        Type[] a = Utils.TypesOfJudgeGame();
        var x = a[GetChoice(a, ask)];
        Judge judge = Activator.CreateInstance(x, stopcondition, HowTogetScore, winCondition, validPlay) as Judge;


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
        //retorna un tokensManager creado con las fichas, en dependencia del tipo de ficha con la que se quiera jugar
        string ask = "Con que tipo de ficha desea jugar \n";
        Type[] a = Utils.TypesofFichas();

        var x = a[GetChoice(a, ask)];
        IGenerator generator = Activator.CreateInstance(x) as IGenerator;
        List<IToken> tokens = generator.CreateTokens(this.MaxDouble);
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

    protected Game ChooseAGame(bool ConfGame = false, bool ChampionPlayers = false)
    {
        //Crea el juego
        if (ConfGame) { ConfGame = true; }//Sleccionar si se quiere modo de juego normal

        IStopGame<Player, IToken> stopcondition = ChooseStopGame(ConfGame);

        IGetScore<IToken> HowTogetScore = ChooseGetScore(ConfGame);

        IWinCondition<(Player player, List<IToken> hand), IToken> winCondition = ChooseWinCondition(ConfGame);

        IComparer<IToken> tokenComparer = ChooseATokenComparerCriteria();

        IEqualityComparer<IToken> equalityComparer = ChooseATokenEqualityCriteria();

        IValidPlay<Board, IToken, ChooseStrategyWrapped> validPlay = ChooseValidPlay(equalityComparer, tokenComparer, ConfGame);

        Judge judge = ChooseJugde(stopcondition, HowTogetScore, winCondition, validPlay, ConfGame);

        TokensManager tokensManager = ChooseATokenManager(this.CantTokenPerPerson, tokenComparer, equalityComparer);

        return new Game(this.MaxDouble, judge, tokensManager);
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

        var Games = new IGame<GameStatus>[CantPartidas];
        bool ConfGame = false;
        if (this.BooleanAsk("Quiere Jugar todos los partidos con las configuraciones predeterminadas"))
        { ConfGame = true; }
        for (int i = 0; i < Games.Length; i++)
        {
            if (i > 0 && this.BooleanAsk(string.Format($"Desea que el juego {i + 1} tenga las mismas configuraciones que el anterior")))
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
    }
    #endregion

    protected virtual bool ValidSettings(int TokensForEach, int MaxDoble, int players)
    {
        //chequea que son validos los settings elegidos
        int totalamount = 0;

        if (TokensForEach == 0 || MaxDoble == 0 || players == 0) return false;

        for (int i = 0; i <= MaxDoble + 1; i++)
        {
            totalamount += i;
        }

        return (TokensForEach * players > totalamount) ? false : true;
    }
}