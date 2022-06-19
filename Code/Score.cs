namespace Game;

public class ClassicScore : IGetScore<Token>
{

    public int Score(Token token)
    {
        return token.Part1.GetHashCode() + token.Part2.GetHashCode();
    }
}

public class DoubleScore : IGetScore<Token>
{
    public int Score(Token token)
    {
        int result = token.Part1 + token.Part2;
        if (token.IsDouble()) result *= 2;
        return result;
    }
}


//Champion

public class ScoreChampionNormal : IGetScore<IPlayer>, IRellenable<IGetScore<IPlayer>, Game>
{
    private Game game { get; set; }
    public ScoreChampionNormal(Game game)
    {
        this.game = game;
    }
    public int Score(IPlayer item)
    {
        return game.judge.PlayerScore(item);
    }

    public IGetScore<IPlayer> Rellenar(Game item)
    {
        return new ScoreChampionNormal(item);
    }
}