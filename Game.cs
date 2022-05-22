namespace Juego
{
          public class Game
    {
        public List<Ficha> board { get; set; }
        public Player[] player{get; set;}

        public Game(List<Ficha> board, Player[] players)
        {
            this.board = board;
            this.player = players;

        }

        public  bool ValidPlay(Ficha ficha)
        {
            if (ficha.Contains(board.First().Parte1)) return true;
            if (ficha.Contains(board.Last().Parte2)) return true;
            return false;
        }

         public  void AddFichaToGame(Ficha ficha, int side){
            Ficha first = board.First();
            Ficha last = board.Last();

            if (side != -1)
            {
                if (side == 0)
                {
                    PlayAlante(ficha, first);
                    return;
                }

                if (side == 1)
                {
                    if (ficha.Contains(last.Parte2))
                        PlayAtras(ficha, last);
                    return;
                }
            }


            if (ficha.Contains(first.Parte1))
            {
                PlayAlante(ficha, first);
                return;
            }

            if (ficha.Contains(last.Parte2))
            {
                PlayAtras(ficha, last);
                return;
            }
        }

         public void PlayAlante(Ficha ficha, Ficha first)
        {
            if (first.Parte1 == ficha.Parte1)
            {
                ficha.SwapFicha();
                board.Insert(0, ficha);

                return;
            }

            board.Insert(0, ficha);

            return;
        }

        public void PlayAtras(Ficha ficha, Ficha last)
        {
            if (ficha.Parte2 == last.Parte2)
            {
                ficha.SwapFicha();
                board.Add(ficha);
                return;
            }

            board.Add(ficha);
            return;
        }

    }
}



