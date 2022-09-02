namespace Game;

/// <summary>
///  He is in charge as referee of designating if a tournament is over and if it is valid for the players to continue playing
/// </summary>
/// <param name=""></param>
/// <returns></returns>
public interface IChampionJudge<TEstatus>
{
    /// <summary>
    ///  Add the last finished game
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    void AddFinishGame(IGame<TEstatus> game);
    /// <summary>
    ///  True if the tournament meets the completion criteria false if it doesn't
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    bool EndGame(List<IGame<TEstatus>> game);
    /// <summary>
    ///  Returns the score of a player during the tournament
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    double PlayerScore(int playerId);
    /// <summary>
    ///  The functions of the judge are initialized, a list with the players must be passed
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    void Run(List<IPlayer> players);
    /// <summary>
    ///  true if the player can continue playing false otherwise
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    bool ValidPlay(IPlayer player);
    /// <summary>
    ///  Descending list of tournament winners
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    List<IPlayer> Winners();
}