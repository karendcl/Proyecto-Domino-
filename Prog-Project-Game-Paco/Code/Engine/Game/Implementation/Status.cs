namespace Game;



public class GameStatus //Actualiza en la pantalla todo lo que ocurre en cada juego
{
    public List<IPlayer> winners { get; protected set; } = new List<IPlayer>() { };
    public List<PlayerStats> PlayerStats { get; protected set; }
    public bool ItsAFinishGame { get; protected set; }
    public List<GamePlayerHand<IToken>> Hands { get; protected set; }
    public IBoard board { get; protected set; }
    public IPlayer actualPlayer { get { return SetActualPlayer(); } }
    public GamePlayerHand<IToken> PlayerActualHand { get { return SetActualPlayerHand(); } }

    public GameStatus(List<PlayerStats> PlayerStats, List<GamePlayerHand<IToken>> hands, IBoard board, bool ItsAFinishGame = false)
    {
        this.board = board;
        this.Hands = hands;
        this.PlayerStats = PlayerStats;
        this.ItsAFinishGame = ItsAFinishGame;
    }
    protected IPlayer SetActualPlayer()
    {
        int count = this.PlayerStats.Count;
        if (count < 1) return null!;
        return this.PlayerStats[count - 1].player.Clone();
    }

    protected GamePlayerHand<IToken> SetActualPlayerHand()
    {
        int count = this.Hands.Count;
        if (count < 0) return null!;
        if (this.Hands is not null)
            foreach (var item in this.Hands)
            {
                if (item is not null)
                    if (item.Equals(this.actualPlayer.Id))
                    {
                        return item;
                    }
            }
        return null!;
    }


    public void AddWinners(List<IPlayer> winners) => this.winners.AddRange(winners);

}