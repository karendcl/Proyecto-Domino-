using System.Diagnostics.CodeAnalysis;

namespace Game;


public class Player : ICloneable<Player>, IEquatable<Player>, IEqualityComparer<Player>, IDescriptible
{
    public virtual List<IToken> hand { get; protected set; } = new List<IToken>() { };
    public virtual int Id { get; }
    public virtual double TotalScore { get; set; }
    public virtual IPlayerStrategy strategy { get { return ChooseStrategy(); } }

    public virtual List<IPlayerStrategy> strategias { get; protected set; } = new List<IPlayerStrategy>() { };

    public virtual string Description => "Computer Player";

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
        string a = "\n Player " + this.Id + "\n";

        foreach (var item in this.hand)
        {
            a += item.ToString();
        }

        return a;
    }

    public void AddScore(double score)
    {
        this.TotalScore += score;

    }

    protected virtual List<IToken> PossiblePlays(WatchPlayer watchPlayer)// Plays posibles
    {

        var CanPlay = new List<IToken>();

        foreach (var item in hand)
        {   //Cambie aca porque el jugador no debe saber si el juez es corrupto

            if (watchPlayer.validPlay.ValidPlay(watchPlayer.board, item).CanMatch) CanPlay.Add(item);
        }

        return CanPlay;
    }

    public virtual IToken BestPlay(WatchPlayer watchPlayer)
    {
        var posibles = PossiblePlays(watchPlayer);
        if (posibles.Count == 0) return null!;

        int[] scores = new int[posibles.Count];

        for (int i = 0; i < scores.Length; i++)
        {
            scores[i] += strategy.Evaluate(posibles[i], hand, watchPlayer);
        }

        int index = Array.IndexOf(scores, scores.Max());
        return posibles[index];
    }

    protected virtual IPlayerStrategy ChooseStrategy()
    {

        int count = this.strategias.Count;
        if (count == 1) return strategias[0];
        Random random = new Random();
        int index = random.Next(0, count - 1);
        return this.strategias[index];
    }
    public virtual int ChooseSide(ChooseStrategyWrapped choose, WatchPlayer watchPlayer)
    {
        return strategy.ChooseSide(choose, watchPlayer);
    }

    public virtual Player Clone()
    {
        Player temp = new Player(this.Id);
        foreach (var item in strategias)
        {
            temp.AddStrategy(item);
        }
        return temp;
    }

    public virtual bool Equals(Player? other)
    {
        if (other is null) return false;
        return (this.Id == other.Id);
    }






    public virtual bool Equals(Player? x, Player? y)
    {
        if (x == null || y == null) return false;
        if (x.Id == y.Id) return true;

        return false;
    }

    public virtual int GetHashCode([DisallowNull] Player obj)
    {
        return obj.GetHashCode();
    }


}

public class CorruptionPlayer : Player, ICorruptible
{
    public CorruptionPlayer(int id) : base(id) { }

    public override string Description => " Computer corruption player";

    public override IToken BestPlay(WatchPlayer watchPlayer)
    {
        int[] scores = new int[this.hand.Count];

        for (int i = 0; i < scores.Length; i++)
        {
            scores[i] += strategy.Evaluate(this.hand[i], hand, watchPlayer);
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



