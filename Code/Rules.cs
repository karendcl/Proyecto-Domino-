namespace Game;


public class ControlPlayer
{
    public IPlayer player { get; private set; }

    //public List<HistorialPlayer> SancionHistorial { get; private set; }

    public bool CanPlay { get; private set; } = false;

    protected Func<IPlayer, GamePlayerHand<Token>, Token, IBoard, int, bool> AddPTokenToBoard { get; set; }
    public ControlPlayer(IPlayer player, Func<IPlayer, GamePlayerHand<Token>, Token, IBoard, int, bool> AddPTokenToBoard)
    {
        this.player = player;
        this.AddPTokenToBoard = AddPTokenToBoard;
    }

    public class HistorialPlayer
    {

    }

}








