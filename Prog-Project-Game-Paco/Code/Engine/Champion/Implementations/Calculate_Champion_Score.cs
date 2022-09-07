
namespace Game;
public class CalculateChampionScore
{
    protected Dictionary<int, List<IPlayerScore>> players = new Dictionary<int, List<IPlayerScore>>();
    Dictionary<int, double> scores = new Dictionary<int, double>();
    protected List<int> playersId { get; set; }
    public CalculateChampionScore(List<int> playersId)
    {
        this.playersId = playersId;
        Run();
    }

    public void Run()
    {
        foreach (var item in playersId)
        {
            players.Add(item, new List<IPlayerScore>());
            scores.Add(item, 0);
        }
    }

    public void AddPlayerScore(int playerId, IPlayerScore player)
    {
        if (!this.players.ContainsKey(playerId))
        {
            this.players.TryAdd(playerId, new List<IPlayerScore>() { player });
        }
        else
        {
            players[playerId].Add(player);
        }

        CalculateScore(playerId, player);
    }
    protected virtual void CalculateScore(int playerId, IPlayerScore player)
    {
        double score = player.Score;
        if (!this.scores.ContainsKey(playerId))
        {
            this.scores.TryAdd(playerId, score);
        }
        else
        {
            this.scores[playerId] += score;
        }


    }

    public double GetScore(int PlayerId)
    {
        return scores[PlayerId];
    }

    public List<IPlayerScore> GetPlayerScore(int PlayerId)
    {
        return players[PlayerId];
    }

    public void LessPlayerScore(int Playerid, double score)
    {
        scores[Playerid] -= score;
    }


}