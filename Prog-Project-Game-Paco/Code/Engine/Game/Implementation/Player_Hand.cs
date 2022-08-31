namespace Game;

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