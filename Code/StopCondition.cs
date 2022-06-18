
namespace Game;

public class Classic : IStopGame<IPlayer, Token>
{


    public bool MeetsCriteria(IPlayer player, IGetScore<Token> score)
    {
        return (player.hand.Count == 0) ? true : false;
    }
}

public class CertainScore : IStopGame<IPlayer, Token>
{
    public int Score { get; set; }

    public CertainScore(int score)
    {
        this.Score = score;
    }

    public bool MeetsCriteria(IPlayer player, IGetScore<Token> howtogetscore)
    {
        int result = 0;

        foreach (var token in player.hand)
        {
            result += howtogetscore.Score(token);
        }

        return (result == Score);
    }
}
