namespace Game;
#region  Game
public class ClassicScore : IGetScore<Token>
{

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

public class ScoreChampionNormal : IGetScore<(Game, IPlayer)>
{
    public int Score((Game, IPlayer) item)
    {
        Game game = item.Item1;
        return game.judge.PlayerScore(item.Item2);
    }

}

#endregion

