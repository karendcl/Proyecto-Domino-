namespace Game;
//Esta clase se utiliza para guardar la mano del jugador
public class GamePlayerHand<TToken> : ICloneable<GamePlayerHand<TToken>>, IEquatable<GamePlayerHand<TToken>> where TToken : Token
{

    public int PlayerId { get; protected set; }
    public bool IPlayed { get { return FichasJugadas.Count < 1 ? true : false; } }//Dice si ha jugado
    public List<TToken> hand { get; protected set; }//Mano
    public Stack<TToken> FichasJugadas { get; protected set; } = new Stack<TToken>() { };//Fichas jugadas

    public bool ContainsToken(TToken token)
    {
        return hand.Contains(token);
    }

    public GamePlayerHand(int PlayerId, List<TToken> token)
    {
        this.hand = token;
        this.PlayerId = PlayerId;
    }

    public void AddLastPlay(TToken token)
    {
        FichasJugadas.Push(token);
        hand.Remove(token);
    }

    public bool LastPlay(out TToken temp)
    {
        if (FichasJugadas.Count < 1) { temp = hand[0]; return false; }//Devuelve un valor inecesari
        temp = FichasJugadas.Peek();
        return true;
    }

    public GamePlayerHand<TToken> Clone() => new GamePlayerHand<TToken>(this.PlayerId, this.hand);

    public bool Equals(GamePlayerHand<TToken>? other)
    {
        if (other == null || other.hand.Count != this.hand.Count || !other.PlayerId.Equals(this.PlayerId)) { return false; }
        for (int i = 0; i < other.hand.Count; i++)
        {
            if (!other.hand[i].Equals(this.hand[i])) { return false; }

        }
        return true;
    }

    public override string ToString()
    {
        string temp = string.Empty;
        foreach (var item in this.hand)
        {
            temp += " " + item.ToString() + " ";
        }
        return temp;
    }
}


public class PlayersCoach
{
    public List<Player> AllPlayers { get; protected set; }
    public List<List<Player>> players { get; protected set; }

    public List<List<Player>> LastPlayersPlays { get; protected set; }

    public PlayersCoach(List<Player> AllPLayers)
    {
        this.AllPlayers = AllPLayers;
        this.players = new List<List<Player>>() { };

        this.LastPlayersPlays = new List<List<Player>>() { };

    }

    public void AddPlayers(int idGame, List<Player> players)
    {
        if (players.Count > 0) this.players.Add(players);
    }




    public bool CloneLastGame(int id)
    {
        List<Player> last = ClonePlayers(players.Last());
        if (last == null) return false;
        this.players.Add(last);
        return true;
    }

    protected List<Player> ClonePlayers(List<Player> list)
    {
        List<Player> temp = new List<Player>() { };
        foreach (var item in list)
        {
            temp.Add(item);
        }
        return temp;
    }

    public List<Player> GetNextTeam()
    {
        if (this.players.Count < 1) return null!;
        List<Player> temp = (ClonePlayers(this.players.First()));
        this.LastPlayersPlays.Add(this.players.First());
        this.players.RemoveAt(0);
        return temp;
    }

}


public class ChampionStatus //Esta clase muestra en pantalla todos los sucesos del a nivel de torneo y juego
{
    public Stack<GameStatus> FinishGame { get; protected set; } = new Stack<GameStatus>() { };
    public List<PlayerStrats> playerStrats { get; protected set; }
    public bool HaveAWinner { get; protected set; }
    public List<Player> Winners { get; protected set; }//Ganadores a nivel de torneo
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

    public ChampionStatus(Stack<GameStatus> FinishGame, List<PlayerStrats> Players, bool HaveAWinner, List<Player> Winners, bool FinishChampion)
    {
        this.FinishGame = FinishGame;
        this.HaveAWinner = HaveAWinner;
        this.Winners = Winners;
        this.playerStrats = Players;
        this.FinishChampion = FinishChampion;
        this.gameStatus = new GameStatus(new List<PlayerStrats>() { }, new List<GamePlayerHand<Token>>() { }, new Board());
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
    public List<Player> winners { get; protected set; } = new List<Player>() { };
    public List<PlayerStrats> playerStrats { get; protected set; }
    public bool ItsAFinishGame { get; protected set; }
    public List<GamePlayerHand<Token>> Hands { get; protected set; }
    public Board board { get; protected set; }
    public Player actualPlayer { get { return SetActualPlayer(); } }
    public GamePlayerHand<Token> PlayerActualHand { get { return SetActualPlayerHand(); } }

    public GameStatus(List<PlayerStrats> playerStrats, List<GamePlayerHand<Token>> hands, Board board, bool ItsAFinishGame = false)
    {
        this.board = board;
        this.Hands = hands;
        this.playerStrats = playerStrats;
        this.ItsAFinishGame = ItsAFinishGame;
    }
    protected Player SetActualPlayer()
    {
        int count = this.playerStrats.Count;
        if (count < 0) return null!;
        return this.playerStrats[count - 1].player.Clone();
    }

    protected GamePlayerHand<Token> SetActualPlayerHand()
    {
        int count = this.Hands.Count;
        if (count < 0) return null!;
        return this.Hands[count - 1].Clone();
    }

    public void AddWinners(List<Player> winners) => this.winners.AddRange(winners);

}


public class PlayerStrats : IEquatable<PlayerStrats> //Da la informacion de cada jugador a pantalla
{
    public Player player { get; protected set; }

    public int punctuation { get; protected set; } = -1;



    public PlayerStrats(Player player)
    {
        this.player = player;


    }

    public void AddPuntuation(int punctuation)
    {
        if (this.punctuation < 0) this.punctuation = punctuation;
    }

    public bool Equals(PlayerStrats? other)
    {
        if (other == null) return false;
        return this.player.Equals(other.player);
    }

    public override string ToString()
    {
        string temp = string.Empty;
        temp += this.player + "   " + this.punctuation;
        return temp;
    }
}




public class WatchPlayer //Tiene toda la informacion que es necesaria por un jugador para poder seleccionar un Token
{
    public IGetScore<Token> howtogetscore { get; protected set; }
    public IStopGame<Player, Token> stopCondition { get; protected set; }
    public IValidPlay<Board, Token, ChooseStrategyWrapped> validPlay { get; protected set; }
    public IWinCondition<(Player player, List<Token> hand), Token> winCondition { get; protected set; }
    public Board board { get; protected set; }
    //Despues hacer una interfaz tipo Ipasable 
    public WatchPlayer(IGetScore<Token> howtogetscore, IStopGame<Player, Token> stopCondition, IValidPlay<Board, Token, ChooseStrategyWrapped> validPlay, IWinCondition<(Player player, List<Token> hand), Token> winCondition, Board board)
    {
        this.howtogetscore = howtogetscore;
        this.stopCondition = stopCondition;
        this.validPlay = validPlay;
        this.winCondition = winCondition;
        this.board = board;
    }

}


