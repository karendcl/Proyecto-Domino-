namespace Juego
{
    public class Token{
        public int Part1 { set; get; }
        public int Part2 { set; get; }

        //public IGetScore howtogetscore {get;set;}
        

        public Token(int Part1, int Part2)
        {
            this.Part1 = Part1;
            this.Part2 = Part2;
           // this.howtogetscore = howto;
            
        }

        public override string ToString()
        {
            /*int a = Part1;
            int b = Part2;

            switch (a,b)
            {
                case (0,0): return "🀱";
                case (0,1): return "🀲";
                case (0,2): return "🀳";
                case (0,3): return "🀱";
                case (0,4): return "🀱";
                case (0,5): return "🀱";
                case (0,6): return "🀱";
                case (1,0): return "🀱";
                case (2,0): return "🀱";
                case (3,0): return "🀱";
                case (4,0): return "🀱";
                case (5,0): return "🀱";
                case (6,0): return "🀱";

            }*/
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
