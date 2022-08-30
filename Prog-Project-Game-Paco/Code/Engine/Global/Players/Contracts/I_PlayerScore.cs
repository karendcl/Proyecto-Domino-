namespace Game;

/// <summary>
///  Returns the score of a player
/// </summary>
/// <param name=""></param>
/// <returns></returns>
public interface IPlayerScore : IDescriptible, IEquatable<PlayerScore>, IEquatable<int>, ICloneable<IPlayerScore>
{
    double Score { get; }

    int PlayerId { get; }
    void AddScore(double score);

    new bool Equals(PlayerScore? other);
    void LessScore(double score);
    void resetScore();
    void SetScore(double score);
    bool AddRange(IPlayerScore player);


}