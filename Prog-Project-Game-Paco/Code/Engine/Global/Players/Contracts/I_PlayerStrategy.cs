namespace Game;

/// <summary>
///  Contains the way to evaluate an action by a player
/// </summary>
/// <param name=""></param>
/// <returns></returns>
public interface IPlayerStrategy : IDescriptible
{
    /// <summary>
    ///  Returns an integer based on the entered criteria
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    int Evaluate(IToken itoken, List<IToken> hand, IWatchPlayer watch);
    /// <summary>
    ///  Choose a position
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    int ChooseSide(IChooseStrategyWrapped choose, IWatchPlayer watch);
}
