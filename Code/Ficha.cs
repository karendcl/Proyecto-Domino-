namespace Juego
{
    public class Token{
        public int Part1 { set; get; }
        public int Part2 { set; get; }
        

        public Token(int Part1, int Part2)
        {
            this.Part1 = Part1;
            this.Part2 = Part2;   
        }

        public override string ToString()
        {
            return "[" + Part1 + "|" + Part2 + "] ";
        }

        public bool IsMatch(Token other)
        {
            return (this.Part1.Equals(other.Part1)) ||
                   (this.Part1.Equals(other.Part2)) ||
                   (this.Part2.Equals(other.Part2)) ||
                   (this.Part2.Equals(other.Part1)) ;
        }

        public bool Contains(int a)
        {
            return (this.Part1.Equals(a) || this.Part2.Equals(a));
        }

        public bool IsDouble()
        {
            return (this.Part1.Equals(this.Part2));
        }

        public void SwapToken()
        {
            var temp = this.Part1;
            this.Part1 = this.Part2;
            this.Part2 = temp;
        }
       
    }
}
