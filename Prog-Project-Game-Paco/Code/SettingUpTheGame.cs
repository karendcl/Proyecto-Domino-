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

    public event Func<string, string[], int>? Asksint;
    public event Action<ChampionStatus>? PrintChampionStatus;
    public event Predicate<string>? BooleanAsk;
    public bool ChooseBool(string msg)
    {
        return BooleanAsk(msg);
    }

    public bool ChooseEnum(Orders orders)  
    {
        System.Console.WriteLine(orders);
        bool x;

        bool.TryParse(Console.ReadLine()!, out x);
        return x;
    }

    protected int ChooseInt(string msg, int min, int max, string[] options)
    {
        //va a devlver un int, en el rango dado
        Console.Clear();
        int x = -1;

        try
        {
            do
            {
                x = this.Asksint(msg, options);
            } while (x < min || x > max);
        }
        catch (System.Exception)
        {
            return ChooseInt(msg, min, max, options);
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
    
        return true;
    }
    protected bool IntermediarioBool(string msg) => obs.BoolResponses(msg);
    protected int IntermediarioInt(string msg, string[] options) => obs.IntResponses(msg, options);

    #region Create a Champion

    public Championship CreateAChampion()
    {
        // retorna un nuevo torneo
        this.obs.PrintStart();
        int cantPartidas = CantPartidas();
        IChampionJudge<GameStatus> judge = ChooseChampionJudge();
        (PlayersCoach players, List<IGame<GameStatus>> games) games = SelectGameTypes(cantPartidas);
        return new Championship(cantPartidas, judge, games.players, games.games);
    }

    protected IChampionJudge<GameStatus> ChooseChampionJudge()
    {
        //retorna un juez del torneo
        IWinCondition<IGame<GameStatus>, List<IPlayerScore>> win = ChooseWinCondition();
        IStopGame<List<IGame<GameStatus>>, List<IPlayerScore>> stop = ChooseStopChampion(win);
        IValidPlay<List<IGame<GameStatus>>, IPlayer, bool> valid = ChooseValidChampion();
        IGetScore<List<IPlayerScore>> score = ChooseChampionGetScore();

        string words = "Que tipo de juez desea para el torneo? ";
        Type[] a = Utils.TypesofEverything<IChampionJudge<GameStatus>>();
        var x = a[GetChoice(a, words)];

        return Activator.CreateInstance(x, stop, win, valid, score) as IChampionJudge<GameStatus>;
    }


    protected IValidPlay<List<IGame<GameStatus>>, IPlayer, bool> ChooseValidChampion()
    {
        //retorna la calidad de descalificacion del torneo
        string words = "Bajo que criterio desee que se descalifique a alguien del torneo \n ";
        Type[] a = Utils.TypesofEverything<IValidPlay<List<IGame<GameStatus>>, IPlayer, bool>>();
        var x = a[GetChoice(a, words)];
        string ask = "Escriba el numero";
        return Activator.CreateInstance(x, ChooseInt(ask, 1, 100, null)) as IValidPlay<List<IGame<GameStatus>>, IPlayer, bool>;

    }
    protected IWinCondition<IGame<GameStatus>, List<IPlayerScore>> ChooseWinCondition()
    {
        //retorna la condicion de ganada del torneo
        string msg = "Que porcentaje de partidas ganadas son necesarias para que un jugador gane el torneo";
        return new WinChampion(ChooseInt(msg, 1, 100,null)) as IWinCondition<IGame<GameStatus>, List<IPlayerScore>>;

    }
    protected IGetScore<List<IPlayerScore>> ChooseChampionGetScore()
    {
        //retorna como se selecciona el score de un torneo
        return new ScoreChampionNormal();
    }

    protected IStopGame<List<IGame<GameStatus>>, List<IPlayerScore>> ChooseStopChampion(IWinCondition<IGame<GameStatus>, List<IPlayerScore>> winCondition)
    {
        //retorna el criterio de parada del torneo
        string words = "Bajo que criterio desea que se pare el torneo \n ";
        Type[] a = Utils.TypesofEverything<IStopGame<List<IGame<GameStatus>>, List<IPlayerScore>>>();
        var x = a[GetChoice(a, words)];
        string ask = "Escriba la cantidad";
        if (x.GetConstructors().SelectMany(t => t.GetParameters()).Count() == 2)
            return Activator.CreateInstance(x, winCondition, ChooseInt(ask, 1, 10000,null)) as IStopGame<List<IGame<GameStatus>>, List<IPlayerScore>>;
        else return Activator.CreateInstance(x, ChooseInt(ask, 1, 10000,null)) as IStopGame<List<IGame<GameStatus>>, List<IPlayerScore>>;

    }
    protected int CantPartidas()
    {
        return ChooseInt("Cuantas partidas desea jugar?", 1, 10000, null);

    }
    protected List<IPlayer> ChampionPlayers(int cantPlayers)
    {
        //se eligen si los jugadores van a jugar
        List<IPlayer> players = new List<IPlayer>() { };

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
        Type[] a = Utils.TypesofEverything<IPlayerStrategy>();
        var x = a[GetChoice(a, words)];
        return Activator.CreateInstance(x) as IPlayerStrategy;
    }
    protected IPlayer ChoosePlayers(int id)
    {
        //devuelve el tipo de jugador 
        string words = $"Elija el tipo de jugador para el Player {id}\n ";
        Type[] a = Utils.TypesofEverything<IPlayer>();
        var x = a[GetChoice(a, words)];
        IPlayer player;
        if (x == typeof(CorruptionPlayer))  player = Activator.CreateInstance(x,id) as CorruptionPlayer;
        else player = Activator.CreateInstance(x, id) as IPlayer;
        player.AddStrategy(ChoosePlayerStrategy(id));
        return player;
    }

    #endregion

    #region  Create Game
    //Elegir tipo de juego
    protected IStopGame<IPlayer, IToken> ChooseStopGame(bool ConfGame)
    {
         string words = "Que tipo de criterio quiere para terminar el juego? \n ";
        Type[] a = Utils.TypesofEverything<IStopGame<IPlayer, IToken>>();

        if (ConfGame) return Activator.CreateInstance(a[0]) as IStopGame<IPlayer, IToken>;

        var x = a[GetChoice(a, words)];
        string ask = "Escriba la cantidad de puntos";
        if (x.GetConstructors().SelectMany(t => t.GetParameters()).Count() != 0)
            return Activator.CreateInstance(x, ChooseInt(ask, 1, int.MaxValue,null)) as IStopGame<IPlayer, IToken>;
        else
            return Activator.CreateInstance(x) as IStopGame<IPlayer, IToken>;

    }

    public int GetChoice(Type[] a, string words)
    {
        string[] options = new string[a.Length];
        //recibe el array de todas las opciones a elegir, y devuelve la posicion de la eleccion
        for (int i = 0; i < a.Length; i++)
        {
            PropertyInfo property = a[i].GetProperty("Description", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            options[i] = (string)property.GetValue(null, null);
            //words += $"\n [{i + 1}]  {value} ";
        }

        return ChooseInt(words, 0, a.Length -1 ,options) ;

    }
    public IGetScore<IToken> ChooseGetScore(bool ConfGame )
    {
        //devuelve como se calcula el score de la ficha en el juego
        string words = "Como desea que se cuente el score de la ficha \n ";
        Type[] a = Utils.TypesofEverything<IGetScore<IToken>>();

        if (ConfGame) return Activator.CreateInstance(a[0]) as IGetScore<IToken>;
        var x = a[GetChoice(a, words)];
        return Activator.CreateInstance(x) as IGetScore<IToken>;

    }

    public IWinCondition<(IPlayer player, List<IToken> hand), IToken> ChooseWinCondition(bool ConfGame)
    {
        //retorna la condicion de ganar del juego
        string words = "Una vez acabe el juego, quien ganaria? \n ";
        Type[] a = Utils.TypesofEverything<IWinCondition<(IPlayer player, List<IToken> hand), IToken>>();
        if (ConfGame) return Activator.CreateInstance(a[0]) as IWinCondition<(IPlayer player, List<IToken> hand), IToken>;
        var x = a[GetChoice(a, words)];

        return Activator.CreateInstance(x) as IWinCondition<(IPlayer player, List<IToken> hand), IToken>;

    }

    public IValidPlay<IBoard, IToken, IChooseStrategyWrapped> ChooseValidPlay(IEqualityComparer<IToken> equalityComparer, IComparer<IToken> Comparer, bool ConfGame )
    {
        //retorna la jugada que es valida para el juego
        string words = "Que tipo de jugada valida desea para el juego \n ";
        Type[] a = Utils.TypesofEverything<IValidPlay<IBoard, IToken, IChooseStrategyWrapped>>();
        if (ConfGame) return Activator.CreateInstance(a[0], equalityComparer, Comparer) as IValidPlay<IBoard, IToken, IChooseStrategyWrapped>;
        var x = a[GetChoice(a, words)];

        return Activator.CreateInstance(x, equalityComparer, Comparer) as IValidPlay<IBoard, IToken, IChooseStrategyWrapped>;
    }


    protected IJudgeGame ChooseJugde(IStopGame<IPlayer, IToken> stopcondition, IGetScore<IToken> HowTogetScore, IWinCondition<(IPlayer player, List<IToken> hand), IToken> winCondition, IValidPlay<IBoard, IToken, IChooseStrategyWrapped> validPlay, bool ConfGame )
    {
        //retorna el tipo de juez para el juego
        //return new Judge(stopcondition,HowTogetScore,winCondition,validPlay);

        string ask = "Que tipo de juez quiere para el juego \n";
        Type[] a = Utils.TypesofEverything<IJudgeGame>();
        var x = a[GetChoice(a, ask)];
        IJudgeGame judge = Activator.CreateInstance(x, stopcondition, HowTogetScore, winCondition, validPlay) as IJudgeGame;


        while (!this.ValidSettings(CantTokenPerPerson, MaxDouble, cantPlayers))
        {
            this.MaxDouble = this.Asksint("Escriba el doble maximo de las Tokens", null);

            this.CantTokenPerPerson = this.Asksint("Escriba cuantas Tokens se van a repartir a cada jugador",null);

            this.cantPlayers = this.Asksint("Escriba cuantos jugadores van a jugar",null);
        }

        return judge;
    }

    protected TokensManager ChooseATokenManager(int TokensForEach, IComparer<IToken> comparer, IEqualityComparer<IToken> equalityComparer)
    {
        //retorna un tokensManager creado con las fichas, en dependencia del tipo de ficha con la que se quiera jugar
        string ask = "Con que tipo de ficha desea jugar \n";
        Type[] a = Utils.TypesofEverything<IGenerator>();

        Type x = a[GetChoice(a, ask)];
    
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

    protected IGame<GameStatus> ChooseAGame(bool ConfGame , bool ChampionPlayers = false)
    {
        //Crea el juego
        if (ConfGame) { ConfGame = true; }//Sleccionar si se quiere modo de juego normal

        IStopGame<IPlayer, IToken> stopcondition = ChooseStopGame(ConfGame);

        IGetScore<IToken> HowTogetScore = ChooseGetScore(ConfGame);

        IWinCondition<(IPlayer player, List<IToken> hand), IToken> winCondition = ChooseWinCondition(ConfGame);

        IComparer<IToken> tokenComparer = ChooseATokenComparerCriteria();

        IEqualityComparer<IToken> equalityComparer = ChooseATokenEqualityCriteria();

        IValidPlay<IBoard, IToken, IChooseStrategyWrapped> validPlay = ChooseValidPlay(equalityComparer, tokenComparer, ConfGame);

        IJudgeGame judge = ChooseJugde(stopcondition, HowTogetScore, winCondition, validPlay, ConfGame);

        TokensManager tokensManager = ChooseATokenManager(this.CantTokenPerPerson, tokenComparer, equalityComparer);

        return new Game(this.MaxDouble, judge, tokensManager) as IGame<GameStatus>;
    }

    //Crea los modos de juego
    public (PlayersCoach, List<IGame<GameStatus>>) SelectGameTypes(int CantPartidas)
    {
        List<IPlayer> tt = new List<IPlayer>();

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
            if (i > 0 && this.BooleanAsk(string.Format($"Desea que el juego [{i + 1}] tenga las mismas configuraciones que el anterior")))
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
        return (coach, Games.ToList<IGame<GameStatus>>());
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