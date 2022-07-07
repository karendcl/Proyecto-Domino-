namespace Game;
//Esta clase se utiliza para guardar la mano del jugador
public class GamePlayerHand<TToken> : ICloneable<GamePlayerHand<TToken>>, IEquatable<GamePlayerHand<TToken>> where TToken : IToken
{

    public int PlayerId { get; protected set; }
    public bool IPlayed { get { return FichasJugadas.Count < 1 ? true : false; } }//Dice si ha jugado
    public List<TToken> hand { get; protected set; }//Mano
    public Stack<TToken> FichasJugadas { get; protected set; } = new Stack<TToken>() { };//Fichas jugadas

    public bool ContainsToken(TToken itoken)
    {
        return hand.Contains(itoken);
    }

    public GamePlayerHand(int PlayerId, List<TToken> itoken)
    {
        this.hand = itoken;
        this.PlayerId = PlayerId;
    }

    public void AddLastPlay(TToken itoken)
    {
        FichasJugadas.Push(itoken);
        hand.Remove(itoken);
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
        this.players.Add(players);
    }




    public bool CloneLastGame(int id)
    {

        //fIX 
        List<Player> last = ClonePlayers(this.players[id]);
        if (last == null) return false;
        this.players.Add(last);
        return true;
    }

    protected List<Player> ClonePlayers(List<Player> list)
    {
        List<Player> temp = new List<Player>() { };
        foreach (var item in list)
        {
            temp.Add(item.Clone());
        }
        return temp;
    }

    public List<Player> GetNextPlayers()
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
        this.gameStatus = new GameStatus(new List<PlayerStrats>() { }, new List<GamePlayerHand<IToken>>() { }, new Board());
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
    public List<GamePlayerHand<IToken>> Hands { get; protected set; }
    public Board board { get; protected set; }
    public Player actualPlayer { get { return SetActualPlayer(); } }
    public GamePlayerHand<IToken> PlayerActualHand { get { return SetActualPlayerHand(); } }

    public GameStatus(List<PlayerStrats> playerStrats, List<GamePlayerHand<IToken>> hands, Board board, bool ItsAFinishGame = false)
    {
        this.board = board;
        this.Hands = hands;
        this.playerStrats = playerStrats;
        this.ItsAFinishGame = ItsAFinishGame;
    }
    protected Player SetActualPlayer()
    {
        int count = this.playerStrats.Count;
        if (count < 1) return null!;
        return this.playerStrats[count - 1].player.Clone();
    }

    protected GamePlayerHand<IToken> SetActualPlayerHand()
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

    public double punctuation { get; protected set; } = -1;



    public PlayerStrats(Player player)
    {
        this.player = player;


    }

    public void AddPuntuation(double punctuation)
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




public class WatchPlayer //Tiene toda la informacion que es necesaria por un jugador para poder seleccionar un IToken
{
    public IGetScore<IToken> howtogetscore { get; protected set; }
    public IStopGame<Player, IToken> stopCondition { get; protected set; }
    public IValidPlay<Board, IToken, ChooseStrategyWrapped> validPlay { get; protected set; }
    public IWinCondition<(Player player, List<IToken> hand), IToken> winCondition { get; protected set; }

    public Board board { get; protected set; }




    //Despues hacer una interfaz tipo Ipasable 
    public WatchPlayer(IGetScore<IToken> howtogetscore, IStopGame<Player, IToken> stopCondition, IValidPlay<Board, IToken, ChooseStrategyWrapped> validPlay, IWinCondition<(Player player, List<IToken> hand), IToken> winCondition, Board board)
    {
        this.howtogetscore = howtogetscore;
        this.stopCondition = stopCondition;
        this.validPlay = validPlay;
        this.winCondition = winCondition;
        this.board = board;
    }

}


/*
public static class Extensor
{
    public static List<T> Clone(this List<T> list)
    {
        List<T> temp = new List<T>(list.Count);

        foreach (var item in temp)
        {
            temp.Add(item.Clone());
        }
        return temp;
    }

}
*/


