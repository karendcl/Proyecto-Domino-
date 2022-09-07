
namespace Game;
public class WatchPlayer : IWatchPlayer
//Tiene toda la informacion que es necesaria por un jugador para poder seleccionar un IToken
{
    public IGetScore<IToken> howtogetscore { get; protected set; }
    public IStopGame<IPlayer, IToken> stopCondition { get; protected set; }
    public IValidPlay<IBoard, IToken, IChooseStrategyWrapped> validPlay { get; protected set; }
    public IWinCondition<(IPlayer player, List<IToken> hand, double score), IToken> winCondition { get; protected set; }
    public IBoard board { get; protected set; }

    public WatchPlayer(IGetScore<IToken> howtogetscore, IStopGame<IPlayer, IToken> stopCondition, IValidPlay<IBoard, IToken, IChooseStrategyWrapped> validPlay, IWinCondition<(IPlayer player, List<IToken> hand, double score), IToken> winCondition, IBoard board)
    {
        this.howtogetscore = howtogetscore;
        this.stopCondition = stopCondition;
        this.validPlay = validPlay;
        this.winCondition = winCondition;
        this.board = board;
    }
}
