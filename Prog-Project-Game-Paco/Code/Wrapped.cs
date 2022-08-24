namespace Game;
//Esta clase se utiliza para guardar la mano del jugador
public class GamePlayerHand<TToken> : ICloneable<GamePlayerHand<TToken>>, IEquatable<GamePlayerHand<TToken>>, IEquatable<int> where TToken : IToken
{

    public int PlayerId { get; protected set; }
    public bool IPlayed { get { return FichasJugadas.Count < 1 ? true : false; } }//Dice si ha jugado
    private HashSet<TToken> handPrivate = new HashSet<TToken>();
    public List<TToken> hand { get => this.handPrivate.ToList<TToken>(); }//Mano
    public Stack<TToken> FichasJugadas { get; protected set; } = new Stack<TToken>() { };//Fichas jugadas

    public bool ContainsToken(TToken itoken)
    {
        return handPrivate.Contains(itoken);
    }

    public GamePlayerHand(int PlayerId, HashSet<TToken> itoken)
    {
        this.handPrivate = itoken;
        this.PlayerId = PlayerId;
    }

    public void AddLastPlay(TToken itoken)
    {
        FichasJugadas.Push(itoken);
        handPrivate.Remove(itoken);
    }

    public bool LastPlay(out TToken temp)
    {
        if (FichasJugadas.Count < 1) { temp = handPrivate.ElementAt(0); return false; }//Devuelve un valor inecesari
        temp = FichasJugadas.Peek();
        return true;
    }

    public GamePlayerHand<TToken> Clone() => new GamePlayerHand<TToken>(this.PlayerId, this.handPrivate);

    public bool Equals(GamePlayerHand<TToken>? other)
    {
        if (other == null || other.handPrivate.Count != this.handPrivate.Count || !other.PlayerId.Equals(this.PlayerId)) { return false; }
        foreach (var item in other.hand)
        {
            if (!this.handPrivate.Contains(item)) { return false; }
        }
        return true;
    }

    public bool Equals(int other)
    {
        return other.Equals(this.PlayerId);
    }

    public override string ToString()
    {
        string temp = string.Empty;
        foreach (var item in this.handPrivate)
        {
            temp += " " + item.ToString() + " ";
        }
        return temp;
    }
}


public class PlayersCoach
{
    public List<IPlayer> AllPlayers { get; protected set; }
    public List<List<IPlayer>> players { get; protected set; }
    public List<List<IPlayer>> LastPlayersPlays { get; protected set; }

    public PlayersCoach(List<IPlayer> AllPLayers)
    {
        this.AllPlayers = AllPLayers;
        this.players = new List<List<IPlayer>>() { };
        this.LastPlayersPlays = new List<List<IPlayer>>() { };
    }

    public void AddPlayers(int idGame, List<IPlayer> players)
    {
        this.players.Add(players);
    }

    public bool CloneLastGame(int id)
    {
        List<IPlayer> last = ClonePlayers(this.players[id]);
        if (last == null) return false;
        this.players.Add(last);
        return true;
    }

    protected List<IPlayer> ClonePlayers(List<IPlayer> list)
    {
        List<IPlayer> temp = new List<IPlayer>() { };
        foreach (var item in list)
        {
            temp.Add(item.Clone());
        }
        return temp;
    }

    public List<IPlayer> GetNextPlayers()
    {
        if (this.players.Count < 1) return null!;
        List<IPlayer> temp = (ClonePlayers(this.players.First()));
        this.LastPlayersPlays.Add(this.players.First());
        this.players.RemoveAt(0);
        return temp;
    }

}



public class PlayerStats : IEquatable<PlayerStats> //Da la informacion de cada jugador a pantalla
{
    public IPlayer player { get; protected set; }
    public double punctuation { get; protected set; } = -1;

    public PlayerStats(IPlayer player)
    {
        this.player = player;
    }

    public void AddPuntuation(double punctuation)
    {
        if (this.punctuation < 0) this.punctuation = punctuation;
    }

    public bool Equals(PlayerStats? other)
    {
        if (other == null) return false;
        return this.player.Equals(other.player);
    }

    public override string ToString()
    {
        return string.Format($"{this.player} : {this.punctuation}");
    }
}



public class WatchPlayer : IWatchPlayer
//Tiene toda la informacion que es necesaria por un jugador para poder seleccionar un IToken
{
    public IGetScore<IToken> howtogetscore { get; protected set; }
    public IStopGame<IPlayer, IToken> stopCondition { get; protected set; }
    public IValidPlay<IBoard, IToken, IChooseStrategyWrapped> validPlay { get; protected set; }
    public IWinCondition<(IPlayer player, List<IToken> hand), IToken> winCondition { get; protected set; }
    public IBoard board { get; protected set; }

    public WatchPlayer(IGetScore<IToken> howtogetscore, IStopGame<IPlayer, IToken> stopCondition, IValidPlay<IBoard, IToken, IChooseStrategyWrapped> validPlay, IWinCondition<(IPlayer player, List<IToken> hand), IToken> winCondition, IBoard board)
    {
        this.howtogetscore = howtogetscore;
        this.stopCondition = stopCondition;
        this.validPlay = validPlay;
        this.winCondition = winCondition;
        this.board = board;
    }
}





