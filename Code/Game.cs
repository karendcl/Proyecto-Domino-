namespace Game;

public class Game : ICloneable<Game>
{
    public virtual event Action<GameStatus>? GameStatus;
    public virtual event Predicate<Orders> CanContinue;
    public Board? board { get; protected set; }
    public List<Player>? player { get; protected set; }
    public bool SwitchDirection { get; set; }
    public int MaxDouble { get; set; }
    public int Players { get; set; }

    public Judge judge { get; set; }

    protected List<GamePlayerHand<IToken>> hands { get { return this.PlayersHands.Values.ToList<GamePlayerHand<IToken>>(); } }
    protected List<PlayerStrats> playerStrats = new List<PlayerStrats>() { };

    protected TokensManager Manager { get; set; }

    protected Dictionary<int, GamePlayerHand<IToken>> PlayersHands { get; set; }

    public Game(bool direction, int max, Judge judge, TokensManager manager)
    {
        this.SwitchDirection = direction;
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



    public void SwapDirection(int player)
    {

        if (SwitchDirection)
        {
            Player[] players = new Player[this.Players];

            for (int i = 0; i < players.Length; i++)
            {
                if (player == 0) player = players.Length;

                players[i] = this.player![player - 1];
                player--;
            }

            this.player = players.ToList();

        }
    }

    public virtual List<Player> Winner()
    {
        List<(Player player, List<IToken> hand)> temp = MatchHandAndPlayer();
        return this.judge.Winner(temp);
    }

    protected void AssingTokens(List<Player> players)
    {
        Dictionary<int, GamePlayerHand<IToken>> PlayersHands = new Dictionary<int, GamePlayerHand<IToken>>() { };

        foreach (var item in players)
        {
            List<IToken> temp = this.Manager.GetTokens();
            item.AddHand(temp);
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

    private GamePlayerHand<IToken> PullAPlayerAndHand(Player player)
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
                if (Token1 == null)
                {
                    this.SwapDirection(i);
                    continue;
                }
                ChooseStrategyWrapped valid = this.judge.ValidPlay(playerNow, board, Token1);

                if (Token1 is null || !valid.CanMatch)
                { //si es nulo, el jugador se ha pasado
                    this.SwapDirection(i);
                }


                if (Token1 != null) //si no es nulo, entonces si lleva
                {
                    if (valid.CanMatch)
                    { //si es valido
                        int index = 0;


                        index = player[i].ChooseSide(valid, watch);



                        Func<Player, GamePlayerHand<IToken>, IToken, Board, int, bool> AddPTokenToBoard = (player, playerHand, Token1, board, index) => judge.AddTokenToBoard(player, playerHand, Token1, board, index);
                        bool control = this.judge.AddTokenToBoard(playerNow, playerHand, Token1, this.board, index);
                        playerNow.AddHand(playerHand.hand);
                        //Actualizar la mano del jugador
                        this.Print(playerNow, this.board, playerHand);//
                                                                      // Diagnostics diagnostics = new Diagnostics();
                                                                      //  diagnostics.TestGame(this.board);
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
        foreach (var item in this.playerStrats)
        {
            double score = this.judge.PlayerScore(item.player);
            item.AddPuntuation(score);
        }
        GameStatus status = new GameStatus(this.playerStrats, this.hands, board!, true);
        List<Player> winners = this.Winner();
        status.AddWinners(winners);
        this.GameStatus!.Invoke(status);
        return status;

    }

    private void Print(Player player, Board board, GamePlayerHand<IToken> hand)
    {
        PlayerStrats playerStrats = new PlayerStrats(player);

        this.playerStrats.Remove(playerStrats);
        this.playerStrats.Add(playerStrats);

        GameStatus status = new GameStatus(this.playerStrats, this.hands, board);
        this.GameStatus!.Invoke(status);

    }
    private IToken Turno(Player player, WatchPlayer watch)
    {
        player.AddScore(this.judge.PlayerScore(player));
        return player.BestPlay(watch);
        //Otorgar aca que si el juez lo deja jugar
    }
    public Game Clone() => new Game(this.SwitchDirection, this.MaxDouble, this.judge, this.Manager);





}


