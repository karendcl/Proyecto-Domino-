namespace Game;

public class Player : IPlayer
{
    public List<Token> hand { get; set; }
    public int Id { get; set; }
    public int TotalScore { get; set; }

    public IPlayerStrategy strategy { get; set; }

    public Player(List<Token> Tokens, int id, IPlayerStrategy strategy)
    {
        this.hand = Tokens;
        this.Id = id;
        this.TotalScore = 0;
        this.strategy = strategy;
    }

    public virtual Token FirstToken(Game game)
    {
        var r = new Random();
        return hand.ElementAt(r.Next(0, hand.Count));
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

    public List<Token> PossiblePlays(Game game)
    {
        var CanPlay = new List<Token>();

        foreach (var item in hand)
        {   //Cambie aca porque el jugador no debe saber si el juez es corrupto
            if (game.validPlay.ValidPlay(game.board, item)) CanPlay.Add(item);
        }

        return CanPlay;
    }

    public virtual Token BestPlay(Game game)
    {
        var posibles = PossiblePlays(game);
        if (posibles.Count == 0) return null!;

        int[] scores = new int[posibles.Count];

        for (int i = 0; i < scores.Length; i++)
        {
            scores[i] += strategy.Evaluate(posibles[i], hand, game);
        }

        int index = Array.IndexOf(scores, scores.Max());
        return posibles[index];
    }

    public virtual int ChooseSide(Game game)
    {
        return strategy.ChooseSide(game);
    }

    public virtual IPlayer Clone()
    {
        return new Player(new List<Token>(), this.Id, this.strategy);
    }

    public bool Equals(IPlayer? other)
    {
        return (this.Id == other.Id);
    }

    public bool Equals(Player? other)
    {
        return (this.Id == other.Id && this.strategy == other.strategy);
    }
}


public class HumanPlayer : Player
{
    public HumanPlayer(List<Token> Tokens, int id, IPlayerStrategy strategy) : base(Tokens, id, strategy)
    {
    }

    public bool OutRange(int index)
    {
        return index < -1 || index >= hand.Count;
    }

    public override Token BestPlay(Game game)
    {
        int ToPlay = -2;

        while (OutRange(ToPlay))
        {
            Console.WriteLine("Escriba el indice de la Token a jugar. Comenzando desde 0. Si no lleva escriba -1");
            ToPlay = int.Parse(Console.ReadLine()!);
            if (ToPlay == -1) return null!;

            while (!game.judge.ValidPlay(game.board, this.hand[ToPlay]))
            {
                Console.WriteLine("No haga trampa! Escriba otra Token");
                ToPlay = int.Parse(Console.ReadLine()!);
            }
        }
        return this.hand[ToPlay];
    }

    public override int ChooseSide(Game game)
    {
        Console.WriteLine("Escriba 0 para jugar alante y 1 para jugar atras");

        return int.Parse(Console.ReadLine()!);
    }

    public override IPlayer Clone()
    {
        return new HumanPlayer(this.hand, this.Id, this.strategy);
    }
}
