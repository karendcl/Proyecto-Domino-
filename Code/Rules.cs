namespace Game;


public class ControlPlayer
{
    public IPlayer player { get; protected set; }

    //public List<HistorialPlayer> SancionHistorial { get; protected set; }

    public bool CanPlay { get; protected set; } = false;

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








