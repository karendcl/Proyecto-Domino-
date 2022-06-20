namespace Game;

public class Game : IRules, ICloneable<Game>
{

    public IBoard board { get; set; }
    public bool DrawToken { get; set; }
    public List<IPlayer> player { get; set; }
    public bool SwitchDirection { get; set; }
    public int MaxDouble { get; set; }
    public int Players { get; set; }
    public int TokensForEach { get; set; }
    public Judge judge { get; set; }

    //public WinnersList<IPlayer,

    //Meter esto en un Wrapped
    private Observer observer { get; set; }//quitar

    public Game(IBoard board, IPlayer[] players, bool direction, int max, int plays, int rep, Judge judge, bool draw)
    {
        this.board = board;
        this.player = players.ToList();
        this.SwitchDirection = direction;
        this.MaxDouble = max;
        this.Players = plays;
        this.TokensForEach = rep;
        this.judge = judge;
        this.DrawToken = draw;
        this.observer = new Observer();
        // GenerarTokens();
        AssignTokens();
    }

    public void AddPlayer(IPlayer player)
    {
        this.player.Add(player);
    }

    public bool DeletePlayer(IPlayer player)
    {
        if (this.player.Count == 1) return false;

        this.player.Remove(player);
        return true;
    }

    public void AssignTokens()
    {

        List<Token> PosiblesTokens = GenerarTokens();

        foreach (var play in player)
        {
            play.hand = RepartirTokens(PosiblesTokens);
        }

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
        return this.judge.Winner(this.player);
    }

    public bool PlayAGame()
    {
        // IJudge<IPlayer, Token,(bool,List<(bool,List<int>)>)> judge = this.judge;
        Judge judge = this.judge;
        List<IPlayer> player = this.player;
        IBoard board = this.board;

        while (!judge.EndGame(this)) //mientras no se acabe el juego
        {
            for (int i = 0; i < this.player.Count; i++) //turno de cada jugador
            {
                if (judge.EndGame(this)) break;

                WatchPlayer watch = judge.RunWatchPlayer(board.Clone(this.board.board));
                // Console.WriteLine(player[i].ToString());
                observer.PaintPlayerInConsole(player[i]);
                // Console.WriteLine(game.board.ToString());

                observer.PaintBord(board);

                observer.Clean();

                IPlayer playerNow = player[i];

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



                        bool control = this.judge.AddTokenToBoard(playerNow, Token1, this.board, index);
                        if (control) { player[i].hand.Remove(Token1); }
                        //se elimina la ficha de la mano del jugador
                    }
                }

                Thread.Sleep(1500);
            }

        }
        return true;

    }

    private Token Turno(IPlayer player, WatchPlayer watch)
    {
        return player.BestPlay(watch);
        //Otorgar aca que si el juez lo deja jugar
    }
    public Game Clone()
    {
        List<IPlayer> list = this.player;
        IPlayer[] tempPlayers = new IPlayer[list.Count];

        for (int i = 0; i < list.Count; i++)
        {
            tempPlayers[i] = list[i].Clone();
        }

        return new Game(new Board(new List<Token>()), tempPlayers, this.SwitchDirection, this.MaxDouble, this.Players, this.TokensForEach, this.judge, this.DrawToken);
    }

}




