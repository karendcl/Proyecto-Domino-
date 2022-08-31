
namespace Game;

public class Classic : IStopGame<IPlayer, IToken>
{
    public static string Description => "Clasico. Cuando alguien se pegue o se tranque el juego";
    public bool MeetsCriteria(IPlayer player, IGetScore<IToken> score)
    {
        return (player.hand.Count == 0) ? true : false;
    }
}

public class CertainScore : IStopGame<IPlayer, IToken>
{
    public static string Description => "Se acaba cuando un jugador tenga un score especifico";
    public int Score { get; set; }

    public CertainScore(int score)
    {
        this.Score = score;
    }

    public bool MeetsCriteria(IPlayer player, IGetScore<IToken> howtogetscore)
    {
        double result = 0;

        foreach (var itoken in player.hand)
        {
            result += howtogetscore.Score(itoken);
        }

        return (result == Score);
    }
}

