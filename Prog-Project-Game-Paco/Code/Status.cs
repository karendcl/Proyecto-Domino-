namespace Game;

public class ChampionStatus //Esta clase muestra en pantalla todos los sucesos del a nivel de torneo y juego
{
    public Stack<GameStatus> FinishGame { get; protected set; } = new Stack<GameStatus>() { };
    public List<PlayerStats> PlayerStats { get; protected set; }
    public bool HaveAWinner { get; protected set; }
    public List<IPlayer> Winners { get; protected set; }//Ganadores a nivel de torneo
    public bool ItsAnGameStatus { get; protected set; } = false;

    public bool ItsAFinishGame
    {
        get
        {
            if (this.gameStatus == null) { return false; }
            return this.gameStatus.ItsAFinishGame;
        }
    }
    public GameStatus gameStatus { get; protected set; }
    public bool FinishChampion { get; protected set; }

    public ChampionStatus(Stack<GameStatus> FinishGame, List<PlayerStats> Players, bool HaveAWinner, List<IPlayer> Winners, bool FinishChampion)
    {
        this.FinishGame = FinishGame;
        this.HaveAWinner = HaveAWinner;
        this.Winners = Winners;
        this.PlayerStats = Players;
        this.FinishChampion = FinishChampion;
        this.gameStatus = new GameStatus(new List<PlayerStats>() { }, new List<GamePlayerHand<IToken>>() { }, new Board());
    }

    public void AddGameStatus(GameStatus Know)
    {
        this.gameStatus = Know;
        this.ItsAnGameStatus = true;
    }


    public GameStatus PullLastFinishGame()
    {
        return this.FinishGame.Peek();
    }


}


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