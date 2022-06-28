using System.Diagnostics.CodeAnalysis;

namespace Game;

public class Player : IPlayer
{
    public virtual List<Token> hand { get; protected set; } = new List<Token>() { };
    public virtual int Id { get; }
    public virtual int TotalScore { get; set; }
    public virtual IPlayerStrategy strategy { get { return ChooseStrategy(); } }

    public virtual List<IPlayerStrategy> strategias { get; protected set; } = new List<IPlayerStrategy>() { };
    public Player(int id)
    {
        this.Id = id;
        this.TotalScore = 0;
    }

    public void AddStrategy(IPlayerStrategy strategy)
    {
        this.strategias.Add(strategy);

    }



    public void AddHand(List<Token> Tokens)
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

    protected List<Token> PossiblePlays(WatchPlayer watchPlayer)// Plays posibles
    {

        var CanPlay = new List<Token>();

        foreach (var item in hand)
        {   //Cambie aca porque el jugador no debe saber si el juez es corrupto

            if (watchPlayer.validPlay.ValidPlay(watchPlayer.board, item).CanMatch) CanPlay.Add(item);
        }

        return CanPlay;
    }

    public virtual Token BestPlay(WatchPlayer watchPlayer)
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

    public IPlayerStrategy ChooseStrategy()
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

    public virtual IPlayer Clone()
    {
        IPlayer temp = new Player(this.Id);
        foreach (var item in strategias)
        {
            temp.AddStrategy(item);
        }
        return temp;
    }

    public bool Equals(IPlayer? other)
    {
        if (other is null) return false;
        return (this.Id == other.Id);
    }

    public bool Equals(Player? other)
    {
        if (other is null) return false;

        return (this.Id == other.Id && this.strategy == other.strategy);
    }


    public int Evaluate(Token token, List<Token> hand, WatchPlayer watch)
    {
        return this.strategy.Evaluate(token, hand, watch);
    }

    public bool Equals(IPlayer? x, IPlayer? y)
    {
        if (x == null || y == null) return false;
        if (x.Id == y.Id) return true;

        return false;
    }

    public int GetHashCode([DisallowNull] IPlayer obj)
    {
        return obj.GetHashCode();
    }
}

/*
public class HumanPlayer : Player,IJugeable
{
    public HumanPlayer(List<Token> Tokens, int id, IPlayerStrategy strategy) : base(Tokens, id, strategy)
    {
    }

    public bool OutRange(int index)
    {
        return index < -1 || index >= hand.Count;
    }

    public override Token BestPlay(WatchPlayer watchPlayer)
    {
        int ToPlay = -2;

        while (OutRange(ToPlay))
        {
            Console.WriteLine("Escriba el indice de la Token a jugar. Comenzando desde 0. Si no lleva escriba -1");
            ToPlay = int.Parse(Console.ReadLine()!);
            if (ToPlay == -1) return null!;
            ChooseStrategyWrapped choose = watchPlayer.validPlay.ValidPlay(watchPlayer.board, this.hand[ToPlay]);
            while (!choose.CanMatch)
            {
                Console.WriteLine("No haga trampa! Escriba otra Token");
                ToPlay = int.Parse(Console.ReadLine()!);
            }
        }
        return this.hand[ToPlay];
    }

    public override int ChooseSide(ChooseStrategyWrapped choose, WatchPlayer watchPlayer)//Modificar
    {
        Console.WriteLine("Escriba 0 para jugar alante y 1 para jugar atras");

        return int.Parse(Console.ReadLine()!);
    }

    public override IPlayer Clone()
    {
        return new HumanPlayer(this.hand, this.Id, this.strategy);
    }
}
*/