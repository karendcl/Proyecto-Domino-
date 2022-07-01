namespace Game;


public class ControlPlayer
{
    public Player player { get; protected set; }

    //public List<HistorialPlayer> SancionHistorial { get; protected set; }

    public bool CanPlay { get; protected set; } = false;

    protected Func<Player, GamePlayerHand<Token>, Token, Board, int, bool> AddPTokenToBoard { get; set; }
    public ControlPlayer(Player player, Func<Player, GamePlayerHand<Token>, Token, Board, int, bool> AddPTokenToBoard)
    {
        this.player = player;
        this.AddPTokenToBoard = AddPTokenToBoard;
    }

    public class HistorialPlayer
    {

    }

}








