namespace Game;

public class Game : ICloneable<Game>
{
    internal virtual event Action<GameStatus>? GameStatus;
    internal virtual event Predicate<Orders> CanContinue;
    public Board? board { get; protected set; }
    public List<Player>? player { get; protected set; }
    internal int MaxDouble { get; set; }
    internal int Players { get; set; }
    internal Judge judge { get; set; }
    protected List<GamePlayerHand<IToken>> hands { get { return this.PlayersHands.Values.ToList<GamePlayerHand<IToken>>(); } }
    protected List<PlayerStats> PlayerStats = new List<PlayerStats>() { };
    protected TokensManager Manager { get; set; }

    protected Dictionary<int, GamePlayerHand<IToken>> PlayersHands { get; set; }

    public Game(int max, Judge judge, TokensManager manager)
    {

        this.MaxDouble = max;
        this.Manager = manager;
        this.judge = judge;

    }

    public override string ToString()
    {
        string a = "Resultados del juego:  \n";

        a += board!.ToString();

        return a;
    }

    public virtual List<IPlayerScore> PlayerScores()
    {
        return judge.PlayersScores();
    }


    public virtual List<Player> Winner()
    {
        List<(Player player, List<IToken> hand)> temp = MatchHandAndPlayer();
        return this.judge.Winner(temp);
    }

    protected virtual void AssingTokens(List<Player> players)  //se reparten las fichas
    {
        Dictionary<int, GamePlayerHand<IToken>> PlayersHands = new Dictionary<int, GamePlayerHand<IToken>>() { };

        foreach (var item in players)
        {
            List<IToken> temp = this.Manager.GetTokens();
            item.AddHand(temp.ToList());
            GamePlayerHand<IToken> hand = new GamePlayerHand<IToken>(item.Id, temp);
            PlayersHands.TryAdd(item.Id, hand);
        }
        this.PlayersHands = PlayersHands;
    }

    protected virtual List<(Player player, List<IToken> hand)> MatchHandAndPlayer()
    {
        List<(Player player, List<IToken> hand)> temp = new List<(Player player, List<IToken> hand)>() { };
        foreach (var item in this.player!)
        {
            int id = item.Id;
            if (PlayersHands.ContainsKey(id))
            {
                GamePlayerHand<IToken> hands = PlayersHands[id];

                (Player player, List<IToken> hand) Player = (item, hands.hand);
                temp.Add(Player);
            }


        }

        return temp;
    }

    protected virtual GamePlayerHand<IToken> PullAPlayerAndHand(Player player)
    {
        if (PlayersHands.ContainsKey(player.Id)) { return PlayersHands[player.Id]; }
        return null!;
    }
    public GameStatus PlayAGame(Board board, List<Player> players)
    {
        
        this.AssingTokens(players);
        this.board = board;
        this.player = players;//Añadir al juego los jugadores y el tablero

        Judge judge = this.judge;
       
        List<Player> player = this.player;



        Func<List<(Player, List<IToken>)>, Board, bool> EndGame = (player, board) => judge.EndGame(player, board.Clone(this.board.board));

        while (!EndGame(MatchHandAndPlayer(), board)) //mientras no se acabe el juego
        {
            for (int i = 0; i < this.player.Count; i++) //turno de cada jugador
            {

                List<(Player player, List<IToken> hand)> Match = MatchHandAndPlayer();

                Player playerNow = player[i];

                GamePlayerHand<IToken> playerHand = PullAPlayerAndHand(playerNow);

                if (playerHand == null) { continue; }

                if (EndGame(Match, board)) break;

                WatchPlayer watch = judge.RunWatchPlayer(board.Clone(this.board.board));

                this.Print(playerNow, this.board, playerHand);//

                IToken Token1 = Turno(playerNow, watch);  //la ficha que se va a jugar                     

                ChooseStrategyWrapped valid = this.judge.ValidPlay(playerNow, board, Token1);


                if (Token1 is not null) //si no es nulo, entonces si lleva
                {
                    if (valid.CanMatch)
                    { //si es valido
                        int index = 0;

                        index = player[i].ChooseSide(valid, watch);

                        Func<Player, GamePlayerHand<IToken>, IToken, Board, int, bool> AddPTokenToBoard = (player, playerHand, Token1, board, index) => judge.AddTokenToBoard(player, playerHand, Token1, board, index);
                        bool control = this.judge.AddTokenToBoard(playerNow, playerHand, Token1, this.board, index);
                        playerNow.AddHand(playerHand.hand);
                        //Actualizar la mano del jugador
                        this.Print(playerNow, this.board, playerHand);
                    }
                }

                this.CanContinue(Orders.NextPartida);
            }
        }
        GameStatus status = EndGameStatus();
        return status;

    }

    protected GameStatus EndGameStatus()
    {
        foreach (var item in this.PlayerStats)
        {
            double score = this.judge.PlayerScore(item.player);
            item.AddPuntuation(score);
        }
        GameStatus status = new GameStatus(this.PlayerStats, hands, board!, true);
        List<Player> winners = this.Winner();
        status.AddWinners(winners);
        this.GameStatus!.Invoke(status);
        return status;

    }

    private void Print(Player player, Board board, GamePlayerHand<IToken> hand)
    {
        PlayerStats PlayerStats = new PlayerStats(player);

        this.PlayerStats.Remove(PlayerStats);
        this.PlayerStats.Add(PlayerStats);

        GameStatus status = new GameStatus(this.PlayerStats, this.hands, board);
        this.GameStatus!.Invoke(status);

    }



    private IToken Turno(Player player, WatchPlayer watch) //el turno solo le pide al jugador su ficha a jugar
    {
        return player.BestPlay(watch);
    }
    public Game Clone() => new Game(this.MaxDouble, this.judge, this.Manager);





}


