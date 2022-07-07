namespace Game;

public interface IPlayerScore : IDescriptible, IEquatable<PlayerScore>
{
    string Description { get; }
    double Score { get; }

    int PlayerId { get; }
    void AddScore(double score);
    bool Equals(PlayerScore? other);
    void LessScore(double score);
    void resetScore();
    void SetScore(double score);
    bool AddRange(IPlayerScore player);
}

public class PlayerScore : IPlayerScore
{
    public string Description => "PlayerScore";

    public double Score { get; protected set; }
    public int PlayerId { get; protected set; }

    public PlayerScore(int playerId)
    {
        PlayerId = playerId;
    }

    public void AddScore(double score)
    {
        Score += score;
    }

    public void resetScore()
    {
        Score = 0;
    }

    public void SetScore(double score)
    {
        Score = score;
    }

    public void LessScore(double score)
    {
        Score -= score;
    }

    public bool AddRange(IPlayerScore player)
    {
        if (this.PlayerId != player.PlayerId)
        {
            return false;
        }
        this.Score += player.Score;
        return true;
    }

    public bool Equals(PlayerScore? other)
    {
        if (other == null) return false;
        return this.PlayerId == other.PlayerId;
    }
}

public class CalculatePlayerScore
{




    public IPlayerScore AddPlay(IPlayerScore player, GamePlayerHand<IToken> hand, double score, bool add)
    {


        if (add)
        {
            double x = ((int)hand.FichasJugadas.Count == 0) ? 1.2 : 1;

            player.AddScore(score * x);
        }
        else
        {
            player.LessScore(score);
        }

        return player;

    }








}