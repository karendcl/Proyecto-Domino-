
namespace Game;


public class MinScore : IWinCondition<(IPlayer player, List<IToken> hand, double score), IToken>
{

    public static new string Description => "Gana el jugador que tenga menos puntos";

    public List<IPlayer> Winner(List<(IPlayer player, List<IToken> hand, double score)> criterios, IGetScore<IToken> howtogetscore)
    {
        List<IPlayer> result = new List<IPlayer>();

        double min = double.MaxValue;

        foreach (var item in criterios)
        {
            if (item.score < min)
            min = item.score;
        }

        foreach (var item in criterios)
        {
            if (item.score==min)
            result.Add(item.player);
        }

        return result;
    }
}

public class MaxScore : IWinCondition<(IPlayer player, List<IToken> hand, double score), IToken>
{
    public new static string Description => "Gana el jugador que tenga mas puntos";

    public List<IPlayer> Winner(List<(IPlayer player, List<IToken> hand, double score)> criterios, IGetScore<IToken> howtogetscore)
    {
        List<IPlayer> result = new List<IPlayer>();

        double max = double.MinValue;
    

        foreach (var item in criterios)
        {
            if (item.score > max)
            max = item.score;
        }

        foreach (var item in criterios)
        {
            if (item.score==max)
            result.Add(item.player);
        }

        return result;
    }
}

public class MiddleScore : IWinCondition<(IPlayer player, List<IToken> hand, double score), IToken>
{
    public List<IPlayer> Winner(List<(IPlayer player, List<IToken> hand, double score)> criterios, IGetScore<IToken> howtogetscore)
    {
        List<IPlayer> result = new List<IPlayer>();

        int mid = criterios.Count /2;
        double sc = criterios[mid].score;

      
        foreach (var item in criterios)
        {
            if (item.score==sc)
            result.Add(item.player);
        }

        return result;
    }


public new static string Description { get { return "Gana el jugador que tenga la media de puntos"; } }

}   


