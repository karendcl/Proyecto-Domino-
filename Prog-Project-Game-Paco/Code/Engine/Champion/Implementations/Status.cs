namespace  Game;

public  class ChampionStatus //Esta clase muestra en pantalla todos los sucesos del a nivel de torneo y juego
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
