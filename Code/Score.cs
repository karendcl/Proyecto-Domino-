namespace Game;
#region  Game
public class ClassicScore : IGetScore<Token>
{
    public virtual string Description => "Clasic Score";

    public int Score(Token token)
    {
        int score = 0;
        foreach (var item in token.Component)
        {
            score += (int)item.ComponentValue;
        }
        return score;
    }
}

public class DoubleScore : ClassicScore
{
    public override string Description => "Double Score";
    public int Score(Token token)
    {
        int result = base.Score(token);

        if (token.IsDouble()) result *= 2;
        return result;
    }
}

#endregion
//Champion
#region  Champion

public class ScoreChampionNormal : IGetScore<(Game, Player)>
{
    public string Description => " Champion Normal Score";

    public int Score((Game, Player) item)
    {
        Game game = item.Item1;
        return game.judge.PlayerScore(item.Item2);
    }

}

#endregion

