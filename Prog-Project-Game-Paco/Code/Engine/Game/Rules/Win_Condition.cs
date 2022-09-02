
namespace Game;

public abstract class WinCondition : IWinCondition<(IPlayer player, List<IToken> hand), IToken>
{
    public static string Description => "Game WinCondition";

    public virtual List<IPlayer> Winner(List<(IPlayer player, List<IToken> hand)> players, IGetScore<IToken> howtogetscore)
    {
        //devuelve una lista de ganadores de la partida
        int count = 0;
        var result = new List<IPlayer>();
        double[] scores = new double[players.Count];

        foreach (var (player, hand) in players)
        {
            if (hand.Count == 0)
            {
                result.Add(player);
            }

            foreach (var IToken in player.hand)
            {
                scores[count] += howtogetscore.Score(IToken);
            }
            count++;
        }

        if (result.Count != 0) return result;

        return FinalWinner(scores, players);
    }

    public abstract List<IPlayer> FinalWinner(double[] scores, List<(IPlayer player, List<IToken> hand)> players);
    //este es el metodo que va a cambiar



}
public class MinScore : WinCondition
{

    public static new string Description => "Gana el jugador que tenga menos puntos";
    public override List<IPlayer> FinalWinner(double[] scores, List<(IPlayer player, List<IToken> hand)> players)
    {
        var result = new List<IPlayer>();

        double score = scores.Min();

        for (int i = 0; i < scores.Length; i++)
        {
            if (scores[i] == score)
            {
                result.Add(players[i].player);
            }
        }

        return result;
    }
}

public class MaxScore : WinCondition
{
    public new static string Description => "Gana el jugador que tenga mas puntos";
    public override List<IPlayer> FinalWinner(double[] scores, List<(IPlayer player, List<IToken> hand)> players)
    {
        var result = new List<IPlayer>();

        double score = scores.Max();

        for (int i = 0; i < scores.Length; i++)
        {
            if (scores[i] == score)
            {
                result.Add(players[i].player);
            }
        }

        return result;
    }
}

public class MiddleScore : WinCondition
{
    public new static string Description { get { return "Gana el jugador que tenga la media de puntos"; } }

    public override List<IPlayer> FinalWinner(double[] scores, List<(IPlayer player, List<IToken> hand)> players)
    {
        var result = new List<IPlayer>();

        double[] temp = new double[scores.Length];
        temp = scores.ToArray();

        Array.Sort(temp);
        int mid = temp.Length / 2;
        double score = temp[mid];


        for (int i = 0; i < scores.Length; i++)
        {
            if (scores[i] == score)
            {
                result.Add(players[i].player);
            }
        }

        return result;
    }
}

