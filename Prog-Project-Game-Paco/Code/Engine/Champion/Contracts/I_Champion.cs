namespace Game;

/// <summary>
///  It is a set of games which has internal rules
/// </summary>
/// <param name=""></param>
/// <returns></returns>
public interface IChampionship<TEstatus>
{
    /// <summary>
    ///  Returns the number of games
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    int CountOfGames { get; }

    /// <summary>
    ///  True if the tournament is over false if it continues
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>

    bool ItsChampionOver { get; }
    /// <summary>
    ///  Decides if a game can be continued
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    event Predicate<Orders> CanContinue;

    /// <summary>
    ///  Returns a wrapper containing the latest tournament update and its subsets
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    event Action<TEstatus> status;

    /// <summary>
    ///  True if the tournament can be continued false otherwise
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    bool Continue(Orders orders);

    /// <summary>
    ///  start the champion
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    void Run();
}

