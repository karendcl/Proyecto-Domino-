using System.Collections;

namespace Game;
#region  Enums

public enum Orders
{
    NextPartida,

    NextPlay

}

#endregion
#region Rules

public interface IWinCondition<TCriterio, TToken> : IDescriptible
{

    /// <summary>
    ///  Returns the list of winners
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    List<IPlayer> Winner(List<TCriterio> criterios, IGetScore<TToken> howtogetscore);
}

public interface IValidPlay<TGame, TPlayer, TCriterio> : IDescriptible
{      /// <summary>
///     Returns under certain criteria if something is valid or not
/// </summary>
/// <param name=""></param>
/// <returns></returns>
    TCriterio ValidPlay(TGame game, TPlayer player);


}

public interface IStopGame<TCriterio, TToken>
{    /// <summary>
///  Returns true if the stopping premises are met, false if they are not met
/// </summary>
/// <param name=""></param>
/// <returns></returns>
    bool MeetsCriteria(TCriterio criterio, IGetScore<TToken> howtogetscore);
}


public interface IGetScore<TToken> : IDescriptible
{    /// <summary>
///  Returns the score that has a certain criteria
/// </summary>
/// <param name=""></param>
/// <returns></returns>
    double Score(TToken item);
}

#endregion


#region Game

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

/// <summary>
///  Returns the score of a player
/// </summary>
/// <param name=""></param>
/// <returns></returns>
public interface IPlayerScore : IDescriptible, IEquatable<PlayerScore>, IEquatable<int>, ICloneable<IPlayerScore>
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




#endregion

#region  IToken
/// <summary>
///  All objects that implement it must have a component value.
/// </summary>
/// <param name=""></param>
/// <returns></returns>
public interface ITokenizable : IComparable<ITokenizable>, IEquatable<ITokenizable>, IDescriptible
{
    string Paint();
    /// <summary>
    ///  Eigenvalue of object that is due to internal characteristics
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    double ComponentValue { get; }

}
/// <summary>
///  Generates tokens under internal criteria
/// </summary>
/// <param name=""></param>
/// <returns></returns>

public interface IGenerator
{
    /// <summary>
    ///  Returns a list of tokens with a double max between the two parts
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    public List<IToken> CreateTokens(int maxDouble);
}





/// <summary>
///  It is a token that contains two parts where these two parts must be ITokenizable objects
/// </summary>
/// <param name=""></param>
/// <returns></returns>
public interface IToken
{
    ITokenizable Part1 { get; }
    ITokenizable Part2 { get; }

    IToken Clone();
    bool Contains(int a);


    bool ItsDouble();
    bool IsMatch(IToken other);
    void SwapToken();
    string ToString();
}
/// <summary>
///  Manage from the creation to the distribution of the tokens
/// </summary>
/// <param name=""></param>
/// <returns></returns>
public interface ITokensManager
{
    List<IToken> Elements { get; }
    IEqualityComparer<IToken> equalityComparer { get; }
    IComparer<IToken> Comparer { get; }

    /// <summary>
    ///  Distribute the tokens under a given criteria
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    List<IToken> GetTokens();
    bool ItsDouble(IToken itoken);
}


#endregion


#region Auxiliar

public interface IDescriptible
{
    public static string Description { get; }
}
public interface ICloneable<T> : ICloneable
{
    new T Clone();
    Object ICloneable.Clone() => Clone()!;
}

public interface ICloneable<T1, T2> : ICloneable<T1>
{
    T1 Clone(T2 item);

}



#endregion


public interface ICorruptible
{  /// <summary>
///  Receives a proposal under a double and returns true or false at its discretion
/// </summary>
/// <param name=""></param>
/// <returns></returns>
    bool Corrupt(double ScoreCost);

}



#region  Añadir ahora
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

#region  Torneo
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
#endregion

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
    IPlayer Clone();
    bool Equals(IPlayer? other);
    bool Equals(IPlayer? x, IPlayer? y);
    bool Equals(int otherId);
    int GetHashCode(IPlayer obj);
    string ToString();
}

/// <summary>
///  Contains the tokens already played
/// </summary>
/// <param name=""></param>
/// <returns></returns>
public interface IBoard : ICloneable<IBoard>
{
    List<IToken> board { get; }
    IToken First { get; }
    IToken Last { get; }

    void AddTokenToBoard(IToken itoken, int side);
    IBoard Clone();
    IBoard Clone(List<IToken> CopyTokens);
    string ToString();
}

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
    IGame<TStatus> Clone();
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
public interface IChooseSideWrapped
{
    /// <summary>
    ///  board index
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    int index { get; }
    /// <summary>
    ///  True if you can set false otherwise
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    bool canChoose { get; }

    /// <summary>
    ///  Returns by which region part of the token should be matched
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    List<int> WhereCanMacht { get; }
    /// <summary>
    ///  Returns by which region part of the token should be matched
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    void AddSide(int i);
    /// <summary>
    ///  Initialize before viewing properties
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    void Run();
}

/// <summary>
///  Contains the behavior of whether or not a token can be put on the board
/// </summary>
/// <param name=""></param>
/// <returns></returns>
public interface IChooseStrategyWrapped
{
    /// <summary>
    ///   True if you can set false otherwise
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    bool CanMatch { get; }
    /// <summary>
    ///  True if it is the first move of the game false otherwise
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    bool FirstPlay { get; }

    IBoard board { get; }
    /// <summary>
    ///  Proposed token to put
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    IToken itoken { get; }
    /// <summary>
    ///  List for all the possible regions to put the token accepted by the rules of the game
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    List<ChooseSideWrapped> side { get; }
    /// <summary>
    ///  Controla que el lugar a poner sea valido y devuelve el envoltorio para dicho indice
    /// </summary>
    /// <param name=""></param>
    /// <returns>retorna true si es posible y el envoltorio correspondiente,  false si no es posible y null el envoltorio</returns>  
    void AddSide(ChooseSideWrapped side);
    (bool, ChooseSideWrapped) ControlSide(int side);


    bool Equals(ChooseStrategyWrapped? other);
}
/// <summary>
///  It is a container of rules and current state of the game that the player must receive
/// </summary>
/// <param name=""></param>
/// <returns></returns>
public interface IWatchPlayer
{
    IGetScore<IToken> howtogetscore { get; }
    IStopGame<IPlayer, IToken> stopCondition { get; }
    IValidPlay<IBoard, IToken, IChooseStrategyWrapped> validPlay { get; }
    IWinCondition<(IPlayer player, List<IToken> hand), IToken> winCondition { get; }
    IBoard board { get; }
}
#endregion