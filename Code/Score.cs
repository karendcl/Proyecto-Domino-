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



public class ScoreChampionNormal : IGetScore<IPlayer>, ICloneable<ScoreChampionNormal, Game>
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



    public ScoreChampionNormal Clone(Game item)
    {
        this.game = item;
        return this;
    }

    public ScoreChampionNormal Clone()
    {
        return new ScoreChampionNormal(this.game.Clone());
    }
}



