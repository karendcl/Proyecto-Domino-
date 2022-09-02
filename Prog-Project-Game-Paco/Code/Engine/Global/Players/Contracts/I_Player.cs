namespace Game;

/// <summary>
///  The player must give an answer based on their strategies
/// </summary>
/// <param name=""></param>
/// <returns></returns>

public interface IPlayer : ICloneable<IPlayer>, IEquatable<IPlayer>, IEqualityComparer<IPlayer>, IDescriptible, IEquatable<int>
{
    /// <summary>
    ///  Return the player's hand
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    List<IToken> hand { get; }
    /// <summary>
    ///  The id is unique for each player
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    int Id { get; }
    /// <summary>
    ///  Contains the Accumulated Score
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    int TotalScore { get; set; }
    /// <summary>
    ///  current strategy
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    IPlayerStrategy strategy { get; }
    /// <summary>
    ///  List of possible strategies
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    List<IPlayerStrategy> strategias { get; }
    /// <summary>
    ///  Update the player's hand
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    void AddHand(List<IToken> Tokens);
    /// <summary>
    ///  Add a strategy
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    void AddStrategy(IPlayerStrategy strategy);
    /// <summary>
    ///  Returns a token under its best play criteria
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    IToken BestPlay(IWatchPlayer watchPlayer);
    /// <summary>
    ///  Decides under a criterion a number that indicates a choice
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    int ChooseSide(IChooseStrategyWrapped choose, IWatchPlayer watchPlayer);
    new IPlayer Clone();
    new bool Equals(IPlayer? other);
    new bool Equals(IPlayer? x, IPlayer? y);
    new bool Equals(int otherId);
    new int GetHashCode(IPlayer obj);
    string ToString();
}
