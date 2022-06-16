namespace Juego
{
    public class Game : IRules, ICloneable<Game>
    {

        public IBoard board { get; set; }
        public bool DrawToken { get; set; }
        public List<IPlayer> player { get; set; }
        public bool SwitchDirection { get; set; }
        public int MaxDouble { get; set; }
        public int Players { get; set; }
        public int TokensForEach { get; set; }
        public IJudge judge { get; set; }

        private Observer observer { get; set; }

        public Game(IBoard board, IPlayer[] players, bool direction, int max, int plays, int rep, IJudge judge, bool draw)
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

        public void AddPlayer(Player player)
        {
            this.player.Add(player);
        }

        public bool DeletePlayer(Player player)
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
            return this.judge.winCondition.Winner(this.player, this.judge);
        }
        /*
                public bool PlayAGame()
                {

                    //Control que todo se pase por referencia
                    // Debug.Assert(game.board.board.Count>1,"Problemas con valores de referencia");

                    IJudge judge = this.judge;
                    List<IPlayer> player = this.player;
                    IBoard board = this.board;

                    while (!judge.EndGame(Game)) //mientras no se acabe el juego
                    {
                        for (int i = 0; i < this.player.Count; i++) //turno de cada jugador
                        {
                            if (judge.EndGame()) break;


                            // Console.WriteLine(player[i].ToString());
                            observer.PaintPlayerInConsole(player[i]);
                            // Console.WriteLine(game.board.ToString());

                            observer.PaintBord(board);

                            observer.Clean();

                            Token Token1 = Turno(player[i], game);  //la ficha que se va a jugar                     

                            if (Token1 is null || !judge.ValidPlay(this.board, Token1))
                            { //si es nulo, el jugador se ha pasado
                                this.SwapDirection(i);
                            }

                            if (Token1 != null) //si no es nulo, entonces si lleva
                            {
                                if (this.judge.ValidPlay(this.board, Token1))
                                { //si es valido
                                    int index = -1;

                                    if (this.judge.PlayAmbigua(Token1, game.board))
                                    {  //si se puede jugar por ambos lados, se le pide que escoja un lado
                                        index = player[i].ChooseSide(game);
                                    }

                                    game.judge.AddTokenToBoard(Token1, game.board, index);
                                    player[i].hand.Remove(Token1); //se elimina la ficha de la mano del jugador
                                }
                            }

                            Thread.Sleep(500);
                        }


                    }
                    return true;

                }

                private Token Turno(IPlayer player)
                {
                    return player.BestPlay(this.game);
                }*/
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
}



