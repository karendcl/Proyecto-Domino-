
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
        double result = 0;

        foreach (var itoken in player.hand)
        {
            result += howtogetscore.Score(itoken);
        }

        return (result == Score);
    }
}

#region  Champion
//Champion

public class StopChampionPerPoints : IStopGame<List<Game>, List<IPlayerScore>>
{
    public static string Description => "Se acaba cuando un jugador acumule x cantidad de puntos";
    protected int Point { get; set; }
    public List<int> Players { get; set; }
    public Dictionary<int, double> acc { get; set; }

    public bool CheckCriteria()
    {
        return this.Point < 0 ? true : false;
    }
    public StopChampionPerPoints(int porcentOfpoints = int.MaxValue)//No lleve acumulado mas puntos que x cantidad
    {
        this.Players = new List<int>() { };
        this.Point = porcentOfpoints;
        this.acc = new Dictionary<int, double>() { };

    }
    //Cada juego se comprueba que no exceda de puntos
    //Se asume que estan todos los jugadores desde un inicio en caso contrario se añade 
    public bool MeetsCriteria(List<Game> games, IGetScore<List<IPlayerScore>> howtogetscore)
    {
        if (CheckCriteria()) return false;

        this.score(games, howtogetscore);

        if (Point == -1) return true;
        foreach (var item in acc.Values) { if (item > Point) { return true; } }
        return false;
    }

    protected void score(List<Game> games, IGetScore<List<IPlayerScore>> howtogetscore)
    {
        var PlayersScore = Organize(games);
        foreach (var playerScore in PlayersScore.Values)
        {

            int id = playerScore[0].PlayerId;
            double cant = howtogetscore.Score((playerScore));
            if (!Players.Contains(id)) { Players.Add(id); acc.TryAdd(id, cant); }
            else { int i = Players.IndexOf(id); acc[id] += cant; }
        }

    }


    protected virtual Dictionary<int, List<IPlayerScore>> Organize(List<Game> games)
    {
        Dictionary<int, List<IPlayerScore>> temp = new Dictionary<int, List<IPlayerScore>>();
        foreach (var game in games)
        {
            foreach (var PlayerScore in game.PlayerScores())
            {
                int id = PlayerScore.PlayerId;
                if (!temp.ContainsKey(id))
                {
                    temp.TryAdd(id, new List<IPlayerScore>() { PlayerScore });

                }
                else
                {
                    temp[id].Add(PlayerScore);
                }
            }
        }
        return temp;
    }


}


public class StopChampionPerHaveAWinner : IStopGame<List<Game>, List<IPlayerScore>>
{
    public static string Description => "Se acaba cuando haya una cantidad x de ganadores ";
    protected IWinCondition<Game, List<IPlayerScore>> winCondition { get; set; }
    protected int CantGanadores { get; set; } = 3;
    public StopChampionPerHaveAWinner(IWinCondition<Game, List<IPlayerScore>> winCondition, int CantGanadores)
    {
        this.winCondition = winCondition;
        if (CantGanadores > 0) { this.CantGanadores = CantGanadores; }

    }
    public bool MeetsCriteria(List<Game> criterio, IGetScore<List<IPlayerScore>> howtogetscore)
    {

        List<Player> players = this.winCondition.Winner(criterio, howtogetscore);
        if (players.Count > CantGanadores) return true;
        return false;
    }
}
#endregion