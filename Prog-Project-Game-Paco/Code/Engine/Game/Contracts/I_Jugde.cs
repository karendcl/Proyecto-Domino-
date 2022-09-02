namespace Game;

#region  Game

/// <summary>
///  It is in charge of being the referee between the board and processing the rules and regulations of the game
/// </summary>
/// <param name=""></param>
/// <returns></returns>
public interface IJudgeGame
{
    /// <summary>
    ///  It is in charge of processing whether or not it is possible to put the piece on the board
    /// </summary>
    /// <param name=""></param>
    /// <returns> If the token was added, it is true, otherwise it is false.</returns>
    bool AddTokenToBoard(IPlayer player, GamePlayerHand<IToken> hand, IToken token, IBoard board, int side);

    /// <summary>
    ///  Check if the rules of the game dictate the end of this
    /// </summary>
    /// <param name=""></param>
    /// <returns>Return true if it should stop false if it should continue</returns>
    bool EndGame(List<(IPlayer, List<IToken>)> players, IBoard board);

    /// <summary>
    ///  Returns the score of a specific player
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    double PlayerScore(IPlayer player);
    /// <summary>
    ///  Returns a list with all the scores of all the players
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    List<IPlayerScore> PlayersScores();
    /// <summary>
    ///  Returns one of the rules that are used at the moment and an update of the board
    /// </summary>
    /// <param name=""></param>
    /// <returns>The board is returned cloned</returns>
    IWatchPlayer RunWatchPlayer(IBoard board);

    /// <summary>
    ///  Returns a strategy under a game chance
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    IChooseStrategyWrapped ValidPlay(IPlayer player, IBoard board, IToken token);

    /// <summary>
    ///  Returns the list of winners
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    List<IPlayer> Winner(List<(IPlayer player, List<IToken> hand)> players);
}
#endregion