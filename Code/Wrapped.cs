namespace Game;

public class GamePlayerHand<TToken> : ICloneable<GamePlayerHand<TToken>>, IEquatable<GamePlayerHand<TToken>> where TToken : Token
{
    public bool IPlayed { get { return FichasJugadas.Count < 1 ? true : false; } }
    public List<TToken> hand { get; private set; }

    public Stack<TToken> FichasJugadas { get; private set; } = new Stack<TToken>() { };

    public bool ContainsToken(TToken token)
    {
        return hand.Contains(token);
    }

    public GamePlayerHand(List<TToken> token)
    {
        this.hand = token;

    }

    public void AddLastPlay(TToken token)
    {
        FichasJugadas.Push(token);
        hand.Remove(token);
    }

    public bool LastPlay(out TToken temp)
    {
        if (FichasJugadas.Count < 1) { temp = hand[0]; return false; }//Devuelve un valor inecesari
        temp = FichasJugadas.Peek();
        return true;
    }

    public GamePlayerHand<TToken> Clone() => new GamePlayerHand<TToken>(this.hand);

    public bool Equals(GamePlayerHand<TToken>? other)
    {
        if (other == null || other.hand.Count != this.hand.Count) { return false; }
        for (int i = 0; i < other.hand.Count; i++)
        {
            if (!other.hand[i].Equals(this.hand[i])) { return false; }

        }
        return true;
    }
}

