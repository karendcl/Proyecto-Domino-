namespace Game;
#region  Game
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

