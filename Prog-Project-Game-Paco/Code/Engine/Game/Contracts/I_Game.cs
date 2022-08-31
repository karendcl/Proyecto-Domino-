namespace Game;

/// <summary>
///  It is in charge of being an intermediary object between the judge and the players, in addition to knowing how to distribute the tokens and start a game.
/// </summary>
/// <param name=""></param>
/// <returns></returns>
public interface IGame<TStatus> : ICloneable<IGame<TStatus>>
{
    /// <summary>
    ///  Returns all the public information of a game: The hand of the players, the current state of the board
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    event Action<TStatus>? GameStatus; //Evento sobre acciones del juego

    /// <summary>
    ///  This event waits for the response to continue the game after a player's turn.
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    event Predicate<Orders> CanContinue;
    IBoard? board { get; }
    List<IPlayer>? GamePlayers { get; }
    /// <summary>
    ///  Returns the score of the players during the game
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    double PlayerScore(IPlayer player);
    new IGame<TStatus> Clone();
    /// <summary>
    ///  The game is initialized, receives the board to play and the players
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    TStatus PlayAGame(IBoard board, List<IPlayer> players);
    /// <summary>
    ///  returns the Score of all players
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    List<IPlayerScore> PlayerScores();

    string ToString();
    /// <summary>
    ///  Returns the list of winners in descending order by position
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    List<IPlayer> Winner();
}
/// <summary>
///  Contains the information about whether a token can be added to a specific region of the board
/// </summary>
/// <param name=""></param>
/// <returns></returns>
