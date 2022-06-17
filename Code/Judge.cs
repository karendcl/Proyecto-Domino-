namespace Juego
{
    public class Judge : IJudge<IPlayer, Token>
    {
        public IStopGame<IPlayer, Token> stopcriteria { get; set; }
        public IGetScore<Token> howtogetscore { get; set; }
        public IWinCondition<IPlayer, Token> winCondition { get; set; }
        public IValidPlay valid { get; set; }

        public Judge(IStopGame<IPlayer, Token> stop, IGetScore<Token> getscore, IWinCondition<IPlayer, Token> winCondition, IValidPlay valid)
        {
            this.stopcriteria = stop;
            this.howtogetscore = getscore;
            this.winCondition = winCondition;
            this.valid = valid;
        }

        public bool PlayerMeetsStopCriteria(IPlayer player)
        {
            return this.stopcriteria.MeetsCriteria(player, this.howtogetscore);
        }
        public virtual bool ValidPlay(IBoard board, Token token)
        {
            return this.valid.ValidPlay(board, token);
        }

        public virtual bool EndGame(Game game)
        {

            foreach (var player in game.player)
            {
                if (PlayerMeetsStopCriteria(player)) return true;
            }

            foreach (var player in game.player)
            {
                foreach (var token in player.hand)
                {
                    if (this.ValidPlay(game.board, token)) return false;
                }
            }// si no esta trancado

            return true; //esta trancado

        }

        public int PlayerScore(IPlayer player)
        {
            int result = 0;

            foreach (var token in player.hand)
            {
                result += this.howtogetscore.Score(token);
            }
            player.TotalScore += result;

            return result;
        }
        public virtual bool ValidSettings(int TokensForEach, int MaxDoble, int players)
        {
            int totalamount = 0;

            if (TokensForEach == 0 || MaxDoble == 0 || players == 0) return false;

            for (int i = 0; i <= MaxDoble + 1; i++)
            {
                totalamount += i;
            }

            return (TokensForEach * players > totalamount) ? false : true;
        }

        public void AddTokenToBoard(Token token, IBoard board, int side)
        {

            if (board.board.Count == 0)
            {
                board.board.Insert(0, token);
                return;
            }

            Token first = board.First();
            Token last = board.Last();


            if ((side == 0) || (valid.Match(token.Part1, first.Part1) || valid.Match(token.Part2, first.Part1)))
            {
                PlayAlante(token, first, board);
                return;
            }

            if ((side == 1) || (valid.Match(token.Part1, last.Part2) || valid.Match(token.Part2, last.Part2)))
            {
                PlayAtras(token, last, board);
                return;
            }

        }


        public void PlayAlante(Token Token, Token first, IBoard board)
        {
            if (valid.Match(Token.Part1, first.Part1))
                Token.SwapToken();

            board.board.Insert(0, Token);
        }

        public void PlayAtras(Token Token, Token last, IBoard board)
        {
            if (valid.Match(Token.Part2, last.Part2))
                Token.SwapToken();

            board.board.Add(Token);

        }

        public bool PlayAmbigua(Token token, IBoard board)
        {
            return (valid.ValidPlayBack(board, token) && valid.ValidPlayFront(board, token));
        }


    }

}