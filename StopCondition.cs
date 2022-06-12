
namespace Juego;

public class Classic : IStopGame{

    public Classic()
    {
    }
    public bool MeetsCriteria(IPlayer player, IGetScore score){
        return (player.hand.Count==0)? true : false;
    }
}   

public class CertainScore: IStopGame{
    public int Score{get;set;}

    public CertainScore(int score){
        this.Score=score;
    }

    public bool MeetsCriteria(IPlayer player, IGetScore howtogetscore){
        int result = 0;

        foreach (var token in player.hand)
        {
            result += howtogetscore.Score(token);
        }

        return (result == Score);
    }
}
