
namespace Game;

public class Classic : IStopGame<Player, IToken>
{
    public static string Description => "Clasico. Cuando alguien se pegue o se tranque el juego";
    public bool MeetsCriteria(Player player, IGetScore<IToken> score)
    {
        return (player.hand.Count == 0) ? true : false;
    }
}

public class CertainScore : IStopGame<Player, IToken>
{
    public static string Description => "Se acaba cuando un jugador tenga un score especifico";
    public int Score { get; set; }

    public CertainScore(int score)
    {
        this.Score = score;
    }

    public bool MeetsCriteria(Player player, IGetScore<IToken> howtogetscore)
    {
        int result = 0;

        foreach (var itoken in player.hand)
        {
            result += howtogetscore.Score(itoken);
        }

        return (result == Score);
    }
}

#region  Champion
//Champion

public class StopChampionPerPoints : IStopGame<List<Game>, (Game, Player)>
{
    public static string Description => "Se acaba cuando un jugador acumule x cantidad de puntos";
    protected int Point { get; set; }
    public List<Player> Players { get; set; }
    public List<int> acc { get; set; }

    public bool CheckCriteria()
    {
        return this.Point < 0 ? true : false;
    }
    public StopChampionPerPoints(int porcentOfpoints = -1)//No lleve acumulado mas puntos que x cantidad
    {
        this.Players = new List<Player>() { };
        this.Point = porcentOfpoints;
        this.acc = new List<int>() { };
    }
    //Cada juego se comprueba que no exceda de puntos
    //Se asume que estan todos los jugadores desde un inicio en caso contrario se añade 
    public bool MeetsCriteria(List<Game> games, IGetScore<(Game, Player)> howtogetscore)
    {
        if (CheckCriteria()) return false;

        foreach (var game in games)
        {
            this.score(game, howtogetscore);
        }

        if (Point == -1) return true;
        foreach (var item in acc) { if (item > Point) { return true; } }
        return false;
    }

    protected void score(Game game, IGetScore<(Game, Player)> howtogetscore)
    {
        List<Player> temp = game.player!;
        foreach (var item in temp)
        {
            int cant = howtogetscore.Score((game, item));
            if (!Players.Contains(item)) { Players.Add(item); acc.Add(cant); }
            else { int i = Players.IndexOf(item); acc[i] += cant; }
        }
    }


}


public class StopChampionPerHaveAWinner : IStopGame<List<Game>, (Game, Player)>
{
    public static string Description => "Se acaba cuando haya una cantidad x de ganadores ";
    protected IWinCondition<Game, (Game, Player)> winCondition { get; set; }
    protected int CantGanadores { get; set; } = 3;
    public StopChampionPerHaveAWinner(IWinCondition<Game, (Game, Player)> winCondition, int CantGanadores)
    {
        this.winCondition = winCondition;
        if (CantGanadores > 0) { this.CantGanadores = CantGanadores; }

    }
    public bool MeetsCriteria(List<Game> criterio, IGetScore<(Game, Player)> howtogetscore)
    {

        List<Player> players = this.winCondition.Winner(criterio, howtogetscore);
        if (players.Count > CantGanadores) return true;
        return false;
    }
}
#endregion