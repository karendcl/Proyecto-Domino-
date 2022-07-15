using System.Diagnostics.CodeAnalysis;

namespace Game;



public class Player : IPlayer
{
    public virtual List<IToken> hand { get; protected set; } = new List<IToken>() { };
    public virtual int Id { get; }
    public virtual int TotalScore { get; set; }
    public virtual IPlayerStrategy strategy { get { return ChooseStrategy(); } }

    public virtual List<IPlayerStrategy> strategias { get; protected set; } = new List<IPlayerStrategy>() { };

    public static string Description => "Jugador Normal";

    public Player(int id)
    {
        this.Id = id;
        this.TotalScore = 0;
    }

    public virtual void AddStrategy(IPlayerStrategy strategy)
    {
        this.strategias.Add(strategy);

    }

    public virtual void AddHand(List<IToken> Tokens)
    {
        this.hand = Tokens;
    }

    public override string ToString()
    {
        string a = $"Player {Id} : \n";

        foreach (var item in this.hand)
        {
            a += item.ToString();
        }

        return a;
    }

    protected virtual List<IToken> PossiblePlays(IWatchPlayer watchPlayer)// Plays posibles
    {
        //devuelve una lista con las posibles fichas a jugar en dependencia de lo que el juez le diga
        var CanPlay = new List<IToken>();

        foreach (var item in hand)
        {
            if (watchPlayer.validPlay.ValidPlay(watchPlayer.board, item).CanMatch) CanPlay.Add(item);
        }

        return CanPlay;
    }

    public virtual IToken BestPlay(IWatchPlayer watchPlayer)
    {
        //de todas las fichas posibles a jugar. el jugador las evalua en dependencia de su estrategia.
        //y la que tenga mas valor, la juega
        var posibles = PossiblePlays(watchPlayer);
        if (posibles.Count == 0) return null!;

        int[] scores = new int[posibles.Count];

        List<IToken> pasar = this.hand.ToList();
        for (int i = 0; i < scores.Length; i++)
        {
            scores[i] += strategy.Evaluate(posibles[i].Clone(), pasar, watchPlayer);
        }

        int index = Array.IndexOf(scores, scores.Max());
        return posibles[index];
    }

    protected virtual IPlayerStrategy ChooseStrategy()
    {
        //en caso de que tenga mas de una estrategia, se selecciona una random
        int count = this.strategias.Count;
        if (count == 1) return strategias[0];
        Random random = new Random();
        int index = random.Next(0, count - 1);
        return this.strategias[index];
    }
    public virtual int ChooseSide(IChooseStrategyWrapped choose, IWatchPlayer watchPlayer)
    {
        return strategy.ChooseSide(choose, watchPlayer);
    }

    public virtual IPlayer Clone()
    {
        Player temp = new Player(this.Id);
        foreach (var item in strategias)
        {
            temp.AddStrategy(item);
        }
        return temp;
    }

    public virtual bool Equals(IPlayer? other)
    {
        if (other is null) return false;
        return (this.Id == other.Id);
    }

    public virtual bool Equals(IPlayer? x, IPlayer? y)
    {
        if (x == null || y == null) return false;
        if (x.Id == y.Id) return true;

        return false;
    }

    public virtual int GetHashCode([DisallowNull] IPlayer obj)
    {
        return obj.GetHashCode();
    }

    public virtual bool Equals(int otherId)
    {
        if (this.Id == otherId) return true;
        return false;
    }
}

public class CorruptionPlayer : Player, ICorruptible, IPlayer
{
    public CorruptionPlayer(int id) : base(id) { }

    public static string Description => "Jugador Corrupto/Tramposo";

    public override IToken BestPlay(IWatchPlayer watchPlayer)
    {
        int[] scores = new int[this.hand.Count];
        List<IToken> pasar = this.hand.ToList();

        for (int i = 0; i < scores.Length; i++)
        {
            scores[i] += strategy.Evaluate(this.hand[i].Clone(), pasar, watchPlayer);
        }

        int index = Array.IndexOf(scores, scores.Max());
        return this.hand[index];

    }

    public bool Corrupt(double ScoreCost)
    {
        Random random = new Random();
        int x = random.Next(0, 4);
        if (x == 1 || x == 3)
        {
            return false;
        }
        return true;

    }

}



