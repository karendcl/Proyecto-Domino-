
namespace Juego;

public abstract class WinCondition : IWinCondition<IPlayer, Token>
{

    public virtual List<IPlayer> Winner(List<IPlayer> players, IJudge<IPlayer, Token> judge)
    {

        int count = 0;
        var result = new List<IPlayer>();
        int[] scores = new int[players.Count];

        foreach (var player in players)
        {
            if (player.hand.Count == 0)
            {
                result.Add(player);
            }

            foreach (var Token in player.hand)
            {
                scores[count] += judge.howtogetscore.Score(Token);
            }
            count++;
        }

        if (result.Count != 0) return result;

        return FinalWinner(scores, players);



    }

    public abstract List<IPlayer> FinalWinner(int[] scores, List<IPlayer> players);




}
public class MinScore : WinCondition
{
    public override List<IPlayer> Winner(List<IPlayer> players, IJudge<IPlayer, Token> judge)
    {

        int count = 0;
        var result = new List<IPlayer>();
        int[] scores = new int[players.Count];

        foreach (var player in players)
        {
            if (player.hand.Count == 0)
            {
                result.Add(player);
            }

            foreach (var Token in player.hand)
            {
                scores[count] += judge.howtogetscore.Score(Token);
            }
            count++;
        }

        if (result.Count != 0) return result;

        return FinalWinner(scores, players);




    }

    public override List<IPlayer> FinalWinner(int[] scores, List<IPlayer> players)
    {
        var result = new List<IPlayer>();

        int score = scores.Min();

        for (int i = 0; i < scores.Length; i++)
        {
            if (scores[i] == score)
            {
                result.Add(players[i]);
            }
        }

        return result;
    }
}

public class MaxScore : WinCondition
{

    public override List<IPlayer> FinalWinner(int[] scores, List<IPlayer> players)
    {
        var result = new List<IPlayer>();

        int score = scores.Min();

        for (int i = 0; i < scores.Length; i++)
        {
            if (scores[i] == score)
            {
                result.Add(players[i]);
            }
        }

        return result;
    }
}

public class Specificscore : WinCondition
{
    int score { get; set; }
    public Specificscore(int score)
    {
        this.score = score;
    }

    public override List<IPlayer> FinalWinner(int[] scores, List<IPlayer> players)
    {
        var result = new List<IPlayer>();

        int score = scores.Min();

        for (int i = 0; i < scores.Length; i++)
        {
            if (scores[i] == score)
            {
                result.Add(players[i]);
            }
        }

        return result;
    }
}
