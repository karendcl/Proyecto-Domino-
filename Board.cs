namespace Juego
{
    public class Board : IBoard {
        public List<Ficha> board {get; set;}

        public Board(List<Ficha> a){
          this.board = a;
        }

        public override string ToString(){
          string a ="\n Board:  \n";

          foreach (var item in this.board)
          {
              a += item.ToString();
          }

          return a;
        }

      public  void AddFichaToBoard(Ficha ficha, int side){

            if (this.board.Count==0) {
                board.Insert(0, ficha);
                return;
            }

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

        public Ficha First(){
            if (board.Count == 0) return null!;
            return this.board.First();
        }

        public Ficha Last(){
            if (board.Count == 0) return null!;
            return this.board.Last();
        }
    }
    
}