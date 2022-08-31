namespace Game;

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