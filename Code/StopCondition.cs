
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

#region  Champion
//Champion

public class StopChampion : IStopGame<Game, (Game, IPlayer)>
{
    private int Point { get; set; }
    public List<IPlayer> Players { get; set; }
    public List<int> acc { get; set; }

    public bool CheckCriteria()
    {
        return Point < 0 ? true : false;
    }
    public StopChampion(int porcentOfpoints = -1)//No lleve acumulado mas puntos que x cantidad
    {
        this.Players = new List<IPlayer>() { };
        this.Point = porcentOfpoints;
        this.acc = new List<int>() { };
    }
    //Cada juego se comprueba que no exceda de puntos
    //Se asume que estan todos los jugadores desde un inicio en caso contrario se añade 
    public bool MeetsCriteria(Game game, IGetScore<(Game, IPlayer)> howtogetscore)
    {
        if (CheckCriteria()) return true;
        this.score(game, howtogetscore);
        foreach (var item in acc) { if (item > Point) { return false; } }
        return true;
    }

    private void score(Game game, IGetScore<(Game, IPlayer)> howtogetscore)
    {
        List<IPlayer> temp = game.player;
        foreach (var item in temp)
        {
            int cant = howtogetscore.Score((game, item));
            if (!Players.Contains(item)) { Players.Add(item); acc.Add(cant); }
            else { int i = Players.IndexOf(item); acc[i] += cant; }
        }
    }


}
#endregion