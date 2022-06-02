namespace Juego
{
  public class Token  
    {
        public int Parte1 { set; get; }
        public int Parte2 { set; get; }

        public Token(int parte1, int parte2)
        {
            Parte1 = parte1;
            Parte2 = parte2;
        }

        public override string ToString()
        {
            return "[" + Parte1 + "|" + Parte2 + "] ";
        }

        public bool IsMatch(Token other)
        {
            return (this.Parte1 == other.Parte1) ||
                   (this.Parte1 == other.Parte2) ||
                   (this.Parte2 == other.Parte2) ||
                   (this.Parte2 == other.Parte1);
        }

        public bool Contains(int a)
        {
            return (this.Parte1.Equals(a) || this.Parte2.Equals(a));
        }

        public bool IsDouble()
        {
            return (this.Parte1.Equals(this.Parte2));
        }

        public int Value()
        {
            return Parte1 + Parte2;
        }

        public void SwapToken()
        {
            var temp = this.Parte1;
            this.Parte1 = this.Parte2;
            this.Parte2 = temp;
        }

        public bool Ambigua(Game game){
            if (game.board.board.Count==0) return false;
            return (this.Contains(game.board.First().Parte1) && this.Contains(game.board.Last().Parte2));
        }
    }
}
