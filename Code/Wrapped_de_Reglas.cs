
namespace Game;

public class WatchPlayer
{
    public IGetScore<Token> howtogetscore { get; private set; }
    public IStopGame<IPlayer, Token> stopCondition { get; private set; }
    public IValidPlay<IBoard, Token, ChooseStrategyWrapped> validPlay { get; private set; }
    public IWinCondition<IPlayer, Token> winCondition { get; private set; }
    public IBoard board { get; private set; }
    //Despues hacer una interfaz tipo Ipasable 
    public WatchPlayer(IGetScore<Token> howtogetscore, IStopGame<IPlayer, Token> stopCondition, IValidPlay<IBoard, Token, ChooseStrategyWrapped> validPlay, IWinCondition<IPlayer, Token> winCondition, IBoard board)
    {
        this.howtogetscore = howtogetscore;
        this.stopCondition = stopCondition;
        this.validPlay = validPlay;
        this.winCondition = winCondition;
        this.board = board;
    }

}