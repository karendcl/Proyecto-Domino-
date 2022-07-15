namespace Game;
#region  Game
public class ClassicScore : IGetScore<IToken>
{
    public static string Description => "Clasico. El valor de la ficha es la suma de sus partes";

    public double Score(IToken token)
    {
        //le da un valor a la ficha
        int score = 0;
        score = (int)(token.Part1.ComponentValue + token.Part2.ComponentValue);
           if(score<0){score=score*-1;}
        return score;
    }
}

public class Double : ClassicScore, IGetScore<IToken>
{
    public static string Description => "Doble. Si una ficha es doble, su valor se duplica";
    public double Score(IToken itoken)
    {
        //le da un valor a la ficha
        double result = base.Score(itoken);

        if (itoken.ItsDouble()) result *= 2;
        if( result<0){result=result*-1;}
        return result;
    }

}

#endregion
//Champion
#region  Champion

public class ScoreChampionNormal : IGetScore<List<IPlayerScore>>
{
    public static string Description => "De la forma usual en un juego";

    public double Score(List<IPlayerScore> score)
    {
        int countNegativePoints = 0;
        double temp = 0;
        foreach (var item in score)
        {
            if (item.Score > 0)
            {
                temp += item.Score;
            }
            else
            {
                countNegativePoints++;
            }
        }

        if (countNegativePoints == 0)
        {
            return temp;
        }
        temp = temp / countNegativePoints;
        return temp;
    }

}

#endregion

