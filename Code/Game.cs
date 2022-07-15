namespace Game;




/// <summary>
///  Una partida de domino completa
/// </summary>
/// <param name=""></param>
/// <returns></returns>
public class Game : IGame<GameStatus>
{
    public virtual event Action<GameStatus>? GameStatus; //Evento sobre acciones del juego
    public virtual event Predicate<Orders> CanContinue;//  Evento de si puede continuar la partida
    public virtual IBoard? board { get; protected set; } //Tablero se recibe en play a game
    public virtual List<IPlayer>? GamePlayers { get { return GetPlayerList(); } }
    protected virtual List<IPlayer>? player { get; set; }// Jugadores de la partida
    internal virtual int MaxDouble { get; set; } //Maximo doble a jugar
    internal IJudgeGame judge { get; set; } //Juez de la partida
    protected List<GamePlayerHand<IToken>> hands { get { return this.PlayersHands.Values.ToList<GamePlayerHand<IToken>>(); } } //Mano de los jugadores
    protected List<PlayerStats> PlayerStats = new List<PlayerStats>() { };  //Estadisticas de los jugadores
    protected TokensManager Manager { get; set; } // Administrador de las fichas

    protected Dictionary<int, GamePlayerHand<IToken>> PlayersHands { get; set; }

    public Game(int max, IJudgeGame judge, TokensManager manager)
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

    protected List<IPlayer>? GetPlayerList()
    {
        var x = new List<IPlayer>();
        foreach (var item in this.player)
        {
            x.Add(item.Clone());
        }

        return x;
    }
    /// <summary>
    ///  Score de todos los player
    /// </summary>
    /// <param name=""></param>
    /// <returns>Lista de IplayerScore</returns>
    public virtual List<IPlayerScore> PlayerScores()
    {
        return judge.PlayersScores();
    }

    /// <summary>
    ///  Lista  De ganadores
    /// </summary>
    /// <param name=""></param>
    /// <returns>Lista de jugadores</returns>
    public virtual List<IPlayer> Winner()
    {
        var temp = MatchHandAndPlayer();
        return this.judge.Winner(temp);
    }

    protected virtual void AssingTokens(List<IPlayer> players)  //se reparten las fichas
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

    protected virtual List<(IPlayer player, List<IToken> hand)> MatchHandAndPlayer()//Se une la mano del jugador con el 
    {
        List<(IPlayer player, List<IToken> hand)> temp = new List<(IPlayer player, List<IToken> hand)>() { };
        foreach (var item in this.player!)
        {
            int id = item.Id;
            if (PlayersHands.ContainsKey(id))
            {
                GamePlayerHand<IToken> hands = PlayersHands[id];

                (IPlayer player, List<IToken> hand) Player = (item, hands.hand);
                temp.Add(Player);
            }


        }

        return temp;
    }

    protected virtual GamePlayerHand<IToken> PullAPlayerAndHand(IPlayer player) //Devuelve la mano de ese jugador
    {
        if (PlayersHands.ContainsKey(player.Id)) { return PlayersHands[player.Id]; }
        return null!;
    }

    /// <summary>
    ///  Jugar la partida 
    /// </summary>
    /// <param name=""></param>
    /// <returns>El estado final de la partida</returns>
    public GameStatus PlayAGame(IBoard board, List<IPlayer> players)
    {

        this.AssingTokens(players); //Se assignan las fichas
        this.board = board;// Se asgina el tablero
        this.player = players;//Añadir al juego los jugadores y el tablero

        var judge = this.judge;

        var player = this.player;//Assignar jugadores



        Func<List<(IPlayer, List<IToken>)>, IBoard, bool> EndGame = (player, board) => judge.EndGame(player, board.Clone(this.board.board));

        while (!EndGame(MatchHandAndPlayer(), board)) //mientras no se acabe el juego
        {
            for (int i = 0; i < this.player.Count; i++) //turno de cada jugador
            {

                var Match = MatchHandAndPlayer();//Se unen las manos de los jugadores

                var playerNow = player[i];

                GamePlayerHand<IToken> playerHand = PullAPlayerAndHand(playerNow);

                if (playerHand == null) { continue; }  //Si no tiene mano continua la partida

                if (EndGame(Match, board)) break;

                IWatchPlayer watch = judge.RunWatchPlayer(board.Clone(this.board.board)); //Envoltorio para el jugador

                this.Print(playerNow, this.board, playerHand);// Enviar a consola el estado actual de la partida

                IToken Token1 = Turno(playerNow, watch);  //la ficha que se va a jugar                     

                IChooseStrategyWrapped valid = this.judge.ValidPlay(playerNow, board, Token1); //Elije los lugares donde puede ser valida 


                if (Token1 is not null) //si no es nulo, entonces si lleva
                {
                    if (valid.CanMatch)
                    { //si es valido
                        int index = 0;

                        index = player[i].ChooseSide(valid, watch);

                        Func<IPlayer, GamePlayerHand<IToken>, IToken, IBoard, int, bool> AddPTokenToBoard = (player, playerHand, Token1, board, index) => judge.AddTokenToBoard(player, playerHand, Token1, board, index);
                        bool control = this.judge.AddTokenToBoard(playerNow, playerHand, Token1, this.board, index); //Intentar añadir la ficha al board
                        playerNow.AddHand(playerHand.hand);
                        //Actualizar la mano del jugador
                        this.Print(playerNow, this.board, playerHand);
                    }
                }

                this.CanContinue(Orders.NextPartida);//Esperar la confirmacion para continuar
            }
        }
        GameStatus status = EndGameStatus();
        return status;

    }

    public double PlayerScore(IPlayer player)
    {
        return this.judge.PlayerScore(player);
    }

    protected GameStatus EndGameStatus()// Si finalizo la partida enviar toda la informacion de finalizacion 
    {
        foreach (var item in this.PlayerStats)
        {
            double score = this.judge.PlayerScore(item.player);
            item.AddPuntuation(score);
        }
        GameStatus status = new GameStatus(this.PlayerStats, hands, board!, true);
        List<IPlayer> winners = this.Winner();
        status.AddWinners(winners);
        this.GameStatus!.Invoke(status);
        return status;

    }

    private void Print(IPlayer player, IBoard board, GamePlayerHand<IToken> hand)
    {
        PlayerStats PlayerStats = new PlayerStats(player);

        this.PlayerStats.Remove(PlayerStats);
        this.PlayerStats.Add(PlayerStats);

        GameStatus status = new GameStatus(this.PlayerStats, this.hands, board);
        this.GameStatus!.Invoke(status);

    }



    private IToken Turno(IPlayer player, IWatchPlayer watch) //el turno solo le pide al jugador su ficha a jugar
    {
        return player.BestPlay(watch);
    }
    public IGame<GameStatus> Clone() => new Game(this.MaxDouble, this.judge, this.Manager);





}


