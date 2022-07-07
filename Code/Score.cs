namespace Game;
#region  Game
public class ClassicScore : IGetScore<IToken>
{
    public virtual string Description => "Clasic Score";

    public double Score(IToken token)
    {
        int score = 0;
        score = (int)(token.Part1.ComponentValue + token.Part2.ComponentValue);

        return score;
    }
}

public class DoubleScore : ClassicScore
{
    public override string Description => "Double Score";
    public double Score(IToken itoken)
    {
        double result = base.Score(itoken);

        if (itoken.ItsDouble()) result *= 2;
        return result;
    }
}

#endregion
//Champion
#region  Champion

public class ScoreChampionNormal : IGetScore<(Game, Player)>
{
    public string Description => " Champion Normal Score";

    public double Score((Game, Player) item)
    {
        Game game = item.Item1;
        return game.judge.PlayerScore(item.Item2);
    }

}

#endregion

