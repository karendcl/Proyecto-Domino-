namespace Game;

public class GamePlayerHand<TToken> : ICloneable<GamePlayerHand<TToken>>, IEquatable<GamePlayerHand<TToken>> where TToken : Token
{
    public int PlayerId { get; protected set; }
    public bool IPlayed { get { return FichasJugadas.Count < 1 ? true : false; } }
    public List<TToken> hand { get; private set; }
    public Stack<TToken> FichasJugadas { get; private set; } = new Stack<TToken>() { };

    public bool ContainsToken(TToken token)
    {
        return hand.Contains(token);
    }

    public GamePlayerHand(int IPlayerId, List<TToken> token)
    {
        this.hand = token;
        this.PlayerId = IPlayerId;
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
    public List<IPlayer> AllPlayers { get; protected set; }
    public List<List<IPlayer>> players { get; protected set; }

    public List<List<IPlayer>> LastPlayersPlays { get; protected set; }

    public PlayersCoach(List<IPlayer> AllPLayers)
    {
        this.AllPlayers = AllPLayers;
        this.players = new List<List<IPlayer>>() { };

        this.LastPlayersPlays = new List<List<IPlayer>>() { };

    }

    public void AddPlayers(int idGame, List<IPlayer> players)
    {
        if (players.Count > 0) this.players.Add(players);
    }




    public bool CloneLastGame(int id)
    {
        List<IPlayer> last = ClonePlayers(players.Last());
        if (last == null) return false;
        this.players.Add(last);
        return true;
    }

    private List<IPlayer> ClonePlayers(List<IPlayer> list)
    {
        List<IPlayer> temp = new List<IPlayer>() { };
        foreach (var item in list)
        {
            temp.Add(item);
        }
        return temp;
    }

    public List<IPlayer> GetNextTeam()
    {
        if (this.players.Count < 1) return null!;
        List<IPlayer> temp = (ClonePlayers(this.players.First()));
        this.LastPlayersPlays.Add(this.players.First());
        this.players.RemoveAt(0);
        return temp;
    }

}


public class ChampionStatus
{
    public Stack<GameStatus> FinishGame { get; protected set; } = new Stack<GameStatus>() { };
    public List<PlayerStrats> playerStrats { get; protected set; }
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

    public ChampionStatus(Stack<GameStatus> FinishGame, List<PlayerStrats> Players, bool HaveAWinner, List<IPlayer> Winners, bool FinishChampion)
    {
        this.FinishGame = FinishGame;
        this.HaveAWinner = HaveAWinner;
        this.Winners = Winners;
        this.playerStrats = Players;
        this.FinishChampion = FinishChampion;
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


public class GameStatus
{
    public List<IPlayer> winners { get; protected set; } = new List<IPlayer>() { };
    public List<PlayerStrats> playerStrats { get; protected set; }
    public bool ItsAFinishGame { get; protected set; }
    public List<GamePlayerHand<Token>> Hands { get; protected set; }
    public IBoard board { get; protected set; }
    public IPlayer actualPlayer { get { return SetActualIplayer(); } }
    public GamePlayerHand<Token> PlayerActualHand { get { return SetActualPlayerHand(); } }

    public GameStatus(List<PlayerStrats> playerStrats, List<GamePlayerHand<Token>> hands, IBoard board, bool ItsAFinishGame = false)
    {
        this.board = board;
        this.Hands = hands;
        this.playerStrats = playerStrats;
        this.ItsAFinishGame = ItsAFinishGame;
    }
    private IPlayer SetActualIplayer()
    {
        int count = this.playerStrats.Count;
        if (count < 0) return null!;
        return this.playerStrats[count - 1].player.Clone();
    }

    private GamePlayerHand<Token> SetActualPlayerHand()
    {
        int count = this.Hands.Count;
        if (count < 0) return null!;
        return this.Hands[count - 1].Clone();
    }

    public void AddWinners(List<IPlayer> winners) => this.winners.AddRange(winners);

}







public class PlayerStrats : IEquatable<PlayerStrats>
{
    public IPlayer player { get; protected set; }

    public int punctuation { get; protected set; } = -1;



    public PlayerStrats(IPlayer player)
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

