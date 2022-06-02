namespace Juego
{
    public abstract class Player : IPlayer
    {
        public List<Token> hand{get; set;}
        public int Id{get; set;}

        public Player(List<Token> Tokens, int id)
        {
            this.hand = Tokens;
            this.Id = id;
        }

        public virtual Token FirstToken(Game game){
             var r = new Random();
            return hand.ElementAt(r.Next(0,hand.Count));
        }

        public override string ToString(){
            string a = "Player " + this.Id + "\n";

            foreach (var item in this.hand)
            {
                a += item.ToString();
            }

            return a;
        }

        public virtual int Score(){
            int a = 0;

            foreach (var token in hand)
            {
                a+= token.Value();
            }

            return a;
        }

        public List<Token> PossiblePlays(Game game)
        {
            var CanPlay = new List<Token>();

            foreach (var item in hand)
            {
                if (game.judge.ValidPlay(game.board, item)) CanPlay.Add(item);
            }

            return CanPlay;
        }

        public abstract Token BestPlay(Game game);

        public virtual int ChooseSide(Game game){
            return 0;
        }

    }

    public class BotaGorda : Player{
        public BotaGorda(List<Token> Tokens, int id) : base(Tokens,id)
        {
        }

        public override Token BestPlay(Game game){
            var posibles = PossiblePlays(game);
            if (posibles.Count ==0) return null!;

            int[] scores = new int[posibles.Count];

            for (int i = 0; i < scores.Length; i++)
            {
                scores[i] += Evaluate(posibles[i], game);
            }

            int index = Array.IndexOf(scores, scores.Max());
            return posibles[index];
        }

        public  int Evaluate(Token Token, Game game){
             int valor = 0;

             foreach (var item in this.hand)
             {
                if (item.IsMatch(Token)) valor++; 
             }

             if (Token.IsDouble()) valor++;

             valor += (int)(Token.Value() / 2);

             return valor;
        }
    }

    public class RandomPlayer : Player{
        public RandomPlayer(List<Token> Tokens, int id) : base(Tokens,id)
        {
        }

        public int Evaluate(Token Token, Game game){
             var r = new Random();
             return r.Next(1,100);
        }

          public override Token BestPlay(Game game){
            var posibles = PossiblePlays(game);
            if (posibles.Count ==0) return null!;
            
            int[] scores = new int[posibles.Count];

            for (int i = 0; i < scores.Length; i++)
            {
                scores[i] += Evaluate(posibles[i], game);
            }

            int index = Array.IndexOf(scores, scores.Max());
            return posibles[index];
        }

        public override int ChooseSide(Game game){
            var r = new Random();
            return r.Next(0,2);
        }
    }

     public class FirstSee : Player{
        public FirstSee(List<Token> Tokens, int id) : base(Tokens,id)
        {
        }

        public override Token BestPlay(Game game){
            var posibles = PossiblePlays(game);
            if (posibles.Count ==0) return null!;

            return posibles[0];  
        }

        public override int ChooseSide(Game game){
            return 1;
        }
       
    }

    public class HumanPlayer : Player
    {
        public HumanPlayer(List<Token> Tokens, int id) : base(Tokens,id)
        {
        }

        public bool OutRange(int index)
        {
            return index < -1 || index >= hand.Count;
        }

        public override Token FirstToken(Game game){
           return BestPlay(game);
        }

        public override Token BestPlay(Game game)
        {
            int ToPlay = -2;

            while (OutRange(ToPlay))
            {
                Console.WriteLine("Escriba el indice de la Token a jugar. Comenzando desde 0. Si no lleva escriba -1");
                ToPlay = int.Parse(Console.ReadLine()!);
                if (ToPlay == -1) return null!;

                 while (!game.judge.ValidPlay(game.board,this.hand[ToPlay]))
            {
                Console.WriteLine("No haga trampa! Escriba otra Token");
                ToPlay = int.Parse(Console.ReadLine()!);
            }
            }
            return this.hand[ToPlay];
        }

         public override int ChooseSide(Game game){
           Console.WriteLine("Escriba 0 para jugar alante y 1 para jugar atras");

           return int.Parse(Console.ReadLine()!);
        }        
    }
}