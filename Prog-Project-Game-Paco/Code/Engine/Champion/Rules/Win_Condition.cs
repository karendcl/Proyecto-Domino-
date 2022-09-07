namespace Game;

#region  Champion

public class WinChampion : IWinCondition<IGame<GameStatus>, List<IPlayerScore>>
{//En total de ganadas 

    public double WinsNeeded { get; protected set; }

    protected virtual Dictionary<int, List<IPlayerScore>> playersScore { get; set; } = new Dictionary<int, List<IPlayerScore>>();
    public static string Description => "Gana el torneo, aquel jugador que haya ganado la mayor cantidad de veces";

    public WinChampion(double porcentWins, int cantGames)
    {

        this.WinsNeeded = porcentWins / 100 * cantGames;
    }

    protected void Run(List<IGame<GameStatus>> games)
    {
        foreach (var game in games)
        {
            foreach (var playersScore in game.PlayerScores())
            {
                int id = playersScore.PlayerId;
                if (!this.playersScore.ContainsKey(id))
                {
                    this.playersScore.TryAdd(id, new List<IPlayerScore>() { playersScore });
                }
                else
                {
                    this.playersScore[id].Add(playersScore);
                }
            }
        }
    }
    public List<IPlayer> Winner(List<IGame<GameStatus>> games, IGetScore<List<IPlayerScore>> howtogetscore)
    {
        Run(games);

        int[] cantWins = new int[playersScore.Count];
        List<IPlayer> res = new();

        foreach (var game in games)
        {
            foreach (var Winner in game.Winner())
            {
                cantWins[Winner.Id - 1] += 1;

                if (cantWins[Winner.Id - 1] >= WinsNeeded)
                    res.Add(Winner);
            }
        }

        return res.DistinctBy(x => x.Id).ToList();

    }

}

#endregion