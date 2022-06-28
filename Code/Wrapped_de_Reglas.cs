
namespace Game;

public sealed class RulesGame<TPlayer, TToken, TBoard>
{
    public IStopGame<TPlayer, TToken> stopcriteria { get; private set; }
    public IGetScore<TToken> howtogetscore { get; private set; }
    public IWinCondition<TPlayer, TToken> winCondition { get; private set; }
    public IValidPlay<TBoard, TToken, ChooseStrategyWrapped> valid { get; private set; }

    public RulesGame(IStopGame<TPlayer, TToken> stop, IGetScore<TToken> getscore, IWinCondition<TPlayer, TToken> winCondition, IValidPlay<TBoard, TToken, ChooseStrategyWrapped> valid)


    {
        this.stopcriteria = stop;
        this.howtogetscore = getscore;
        this.winCondition = winCondition;
        this.valid = valid;
    }

}

public class WatchPlayer
{
    public IGetScore<Token> howtogetscore { get; private set; }
    public IStopGame<IPlayer, Token> stopCondition { get; private set; }
    public IValidPlay<IBoard, Token, ChooseStrategyWrapped> validPlay { get; private set; }
    public IWinCondition<(IPlayer player, List<Token> hand), Token> winCondition { get; private set; }
    public IBoard board { get; private set; }
    //Despues hacer una interfaz tipo Ipasable 
    public WatchPlayer(IGetScore<Token> howtogetscore, IStopGame<IPlayer, Token> stopCondition, IValidPlay<IBoard, Token, ChooseStrategyWrapped> validPlay, IWinCondition<(IPlayer player, List<Token> hand), Token> winCondition, IBoard board)
    {
        this.howtogetscore = howtogetscore;
        this.stopCondition = stopCondition;
        this.validPlay = validPlay;
        this.winCondition = winCondition;
        this.board = board;
    }

}