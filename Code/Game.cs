namespace Game;

public class Game : ICloneable<Game>
{

    public IBoard? board { get; protected set; }
    public List<IPlayer>? player { get; protected set; }
    public bool SwitchDirection { get; set; }
    public int MaxDouble { get; set; }
    public int Players { get; set; }
    public int TokensForEach { get; set; }
    public Judge judge { get; set; }

    protected List<GamePlayerHand<Token>> hands { get { return this.PlayersHands.Values.ToList<GamePlayerHand<Token>>(); } }

    protected List<PlayerStrats> playerStrats = new List<PlayerStrats>() { };
    protected List<(IPlayer, GamePlayerHand<Token>)> Hands { get; set; } = new List<(IPlayer, GamePlayerHand<Token>)>() { };

    public event Action<GameStatus>? GameStatus;



    private Dictionary<int, GamePlayerHand<Token>> PlayersHands = new Dictionary<int, GamePlayerHand<Token>>() { };

    public Game(bool direction, int max, int rep, Judge judge)
    {
        this.SwitchDirection = direction;
        this.MaxDouble = max;

        this.TokensForEach = rep;
        this.judge = judge;


        // GenerarTokens();

    }



    public void AssignTokens()
    {

        List<Token> PosiblesTokens = GenerarTokens();

        foreach (var player in player)
        {
            List<Token> temp = RepartirTokens(PosiblesTokens);
            GamePlayerHand<Token> hand = new GamePlayerHand<Token>(player.Id, temp.ToList<Token>());
            PlayersHands.TryAdd(player.Id, hand);
            AddToHands(player, hand);
            player.AddHand(temp);
        }
    }

    private void AddToHands(IPlayer player, GamePlayerHand<Token> hand)
    {
        this.Hands.Add((player, hand));
    }
    public List<Token> RepartirTokens(List<Token> PosiblesTokens)
    {
        int cantidadDisponible = PosiblesTokens.Count;

        int contador = 0;
        var lista = new List<Token>();

        while (contador != TokensForEach)
        {
            var r = new Random();
            var index = r.Next(0, cantidadDisponible);
            cantidadDisponible--;
            contador++;
            lista.Add(PosiblesTokens[index]);
            PosiblesTokens.RemoveAt(index);
        }

        return lista;
    }

    public List<Token> GenerarTokens()
    {

        List<Token> PosiblesTokens = new List<Token>();

        for (int i = 0; i <= this.MaxDouble; i++)
        {
            for (int j = i; j <= this.MaxDouble; j++)
            {
                Token Token = new Token(i, j);
                PosiblesTokens.Add(Token);
            }
        }

        return PosiblesTokens;
    }

    public override string ToString()
    {
        string a = "Resultados del juego:  \n";

        a += board.ToString();

        return a;
    }



    public void SwapDirection(int player)
    {

        if (SwitchDirection)
        {
            IPlayer[] players = new Player[this.Players];

            for (int i = 0; i < players.Length; i++)
            {
                if (player == 0) player = players.Length;

                players[i] = this.player[player - 1];
                player--;
            }

            this.player = players.ToList();

        }
    }

    public virtual List<IPlayer> Winner()
    {
        List<(IPlayer player, List<Token> hand)> temp = MatchHandAndPlayer();
        return this.judge.Winner(temp);
    }

    protected virtual List<(IPlayer player, List<Token> hand)> MatchHandAndPlayer()
    {
        List<(IPlayer player, List<Token> hand)> temp = new List<(IPlayer player, List<Token> hand)>() { };
        foreach (var item in this.player)
        {
            int id = item.Id;
            if (PlayersHands.ContainsKey(id))
            {
                GamePlayerHand<Token> hands = PlayersHands[id];

                (IPlayer player, List<Token> hand) Player = (item, hands.hand);
                temp.Add(Player);
            }


        }
        return temp;
    }

    private GamePlayerHand<Token> PullAPlayerAndHand(IPlayer player)
    {
        if (PlayersHands.ContainsKey(player.Id)) { return PlayersHands[player.Id]; }
        return null!;
    }
    public GameStatus PlayAGame(IBoard board, List<IPlayer> players)
    {
        this.board = board;
        this.player = players;//Añadir al juego los jugadores y el tablero

        Judge judge = this.judge;
        List<IPlayer> player = this.player;
        AssignTokens();


        Func<List<(IPlayer, List<Token>)>, IBoard, bool> EndGame = (player, board) => judge.EndGame(player, board.Clone(this.board.board));

        while (!EndGame(MatchHandAndPlayer(), board)) //mientras no se acabe el juego
        {
            for (int i = 0; i < this.player.Count; i++) //turno de cada jugador
            {

                List<(IPlayer player, List<Token> hand)> Match = MatchHandAndPlayer();

                IPlayer playerNow = player[i];

                GamePlayerHand<Token> playerHand = PullAPlayerAndHand(playerNow);

                if (playerHand == null) { continue; }

                if (EndGame(Match, board)) break;

                WatchPlayer watch = judge.RunWatchPlayer(board.Clone(this.board.board));

                this.Print(playerNow, this.board, playerHand);

                Token Token1 = Turno(playerNow, watch);  //la ficha que se va a jugar                     
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



                        Func<IPlayer, GamePlayerHand<Token>, Token, IBoard, int, bool> AddPTokenToBoard = (player, playerHand, Token1, board, index) => judge.AddTokenToBoard(player, playerHand, Token1, board, index);
                        bool control = this.judge.AddTokenToBoard(playerNow, playerHand, Token1, this.board, index);
                        playerNow.AddHand(playerHand.hand);
                        //Actualizar la mano del jugador
                        this.Print(playerNow, this.board, playerHand);
                    }
                }

                Thread.Sleep(1500);
            }

        }
        GameStatus status = EndGameStatus();
        return status;

    }

    protected GameStatus EndGameStatus()
    {
        foreach (var item in this.playerStrats)
        {
            int score = this.judge.PlayerScore(item.player);
            item.AddPuntuation(score);
        }
        GameStatus status = new GameStatus(this.playerStrats, this.hands, board, true);
        List<IPlayer> winners = this.Winner();
        status.AddWinners(winners);
        this.GameStatus.Invoke(status);
        return status;

    }

    private void Print(IPlayer player, IBoard board, GamePlayerHand<Token> hand)
    {
        PlayerStrats playerStrats = new PlayerStrats(player);

        this.playerStrats.Remove(playerStrats);
        this.playerStrats.Add(playerStrats);

        GameStatus status = new GameStatus(this.playerStrats, this.hands, board);
        this.GameStatus.Invoke(status);
        // Observer.Invoke(player.Clone(), board.Clone(this.board.board), hand.Clone());
    }
    private Token Turno(IPlayer player, WatchPlayer watch)
    {
        return player.BestPlay(watch);
        //Otorgar aca que si el juez lo deja jugar
    }
    public Game Clone() => new Game(this.SwitchDirection, this.MaxDouble, this.TokensForEach, this.judge);





}


