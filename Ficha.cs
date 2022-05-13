namespace Domino
{
    public class Ficha
    {
        public int Parte1 { set; get; }
        public int Parte2 { set; get; }

        public Ficha(int parte1, int parte2)
        {
            Parte1 = parte1;
            Parte2 = parte2;
        }

        public override string ToString()
        {
            return " [" + Parte1 + "|" + Parte2 + "] ";
        }

        public bool IsMatch(Ficha other){
            return (this.Parte1 == other.Parte1) ||
                   (this.Parte1 == other.Parte2) || 
                   (this.Parte2 == other.Parte2) ||
                   (this.Parte2 == other.Parte1);
        }

        public bool Contains(int a){
            return (this.Parte1 == a || this.Parte2 ==a);
        }

        public bool IsDouble(){
            return (this.Parte1 == this.Parte2);
        }

        public int Suma(){
            return this.Parte1 + this.Parte2;
        }

        public void SwapFicha(){
           var temp = this.Parte1;
           this.Parte1 = this.Parte2;
           this.Parte2 = temp;
        }

      
    }

    public class Game{
       public List<Ficha> board {get; set;}
       public List<Ficha> ComputerFichas {get; set;}
       public List<Ficha> UserFichas {get; set;}

       public Game(List<Ficha> board,List<Ficha> ComputerFichas, List<Ficha> UserFichas ){
           this.board = board;
           this.ComputerFichas = ComputerFichas;
           this.UserFichas = UserFichas;

       }

       public bool ValidPlay(Ficha ficha){
           if (ficha.Contains(board.First().Parte1)) return true;
           if (ficha.Contains(board.Last().Parte2)) return true;
           return false;

       }

    }
}


