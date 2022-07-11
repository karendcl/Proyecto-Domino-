namespace Game;
#region  Game
public class ClassicScore : IGetScore<IToken>
{
    public static string Description => "Clasic Score";

    public int Score(IToken token)
    {
        int score = 0;
        score = (int)(token.Part1.ComponentValue + token.Part2.ComponentValue);

        return score;
    }

    public override string ToString()
    {
        return "Clasico. El valor de la ficha es la suma de sus partes";
    }
}

public class Double : ClassicScore, IGetScore<IToken>
{
    public static string Description => "Double Score";
    public int Score(IToken itoken)
    {
        int result = base.Score(itoken);

        if (itoken.ItsDouble()) result *= 2;
        return result;
    }

    public override string ToString()
    {
        return "Doble. Si una ficha es doble, su valor se duplica";
    }
}

#endregion
//Champion
#region  Champion

public class ScoreChampionNormal : IGetScore<(Game, Player)>
{
    public static string Description => "De la forma usual en un juego";

    public int Score((Game, Player) item)
    {
        Game game = item.Item1;
        return game.judge.PlayerScore(item.Item2);
    }

}

#endregion

