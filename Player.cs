namespace Juego
{
    public abstract class Player : IPlayer
    {
        public List<Ficha> hand{get; set;}
        public int Id{get; set;}

        public Player(List<Ficha> fichas, int id)
        {
            this.hand = fichas;
            this.Id = id;
        }

        public virtual Ficha FirstFicha(Game game){
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

        public List<Ficha> PossiblePlays(Game game)
        {
            var CanPlay = new List<Ficha>();

            foreach (var item in hand)
            {
                if (game.ValidPlay(item)) CanPlay.Add(item);
            }

            return CanPlay;
        }

        public abstract Ficha BestPlay(Game game);

        public virtual int ChooseSide(Game game){
            return 0;
        }

    }

    public class BotaGorda : Player{
        public BotaGorda(List<Ficha> fichas, int id) : base(fichas,id)
        {
        }

        public override Ficha BestPlay(Game game){
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

        public  int Evaluate(Ficha ficha, Game game){
             int valor = 0;

             foreach (var item in this.hand)
             {
                if (item.IsMatch(ficha)) valor++; 
             }

             if (ficha.IsDouble()) valor++;

             valor += (int)(ficha.Suma() / 2);

             return valor;
        }
    }

    public class RandomPlayer : Player{
        public RandomPlayer(List<Ficha> fichas, int id) : base(fichas,id)
        {
        }

        public int Evaluate(Ficha ficha, Game game){
             var r = new Random();
             return r.Next(1,100);
        }

          public override Ficha BestPlay(Game game){
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
        public FirstSee(List<Ficha> fichas, int id) : base(fichas,id)
        {
        }

        public override Ficha BestPlay(Game game){
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
        public HumanPlayer(List<Ficha> fichas, int id) : base(fichas,id)
        {
        }

        public bool OutRange(int index)
        {
            return index < -1 || index >= hand.Count;
        }

        public override Ficha FirstFicha(Game game){
           return BestPlay(game);
        }

        public override Ficha BestPlay(Game game)
        {
            int ToPlay = -2;

            while (OutRange(ToPlay))
            {
                Console.WriteLine("Escriba el indice de la ficha a jugar. Comenzando desde 0. Si no lleva escriba -1");
                ToPlay = int.Parse(Console.ReadLine()!);
                if (ToPlay == -1) return null!;

                 while (!game.ValidPlay(this.hand[ToPlay]))
            {
                Console.WriteLine("No haga trampa! Escriba otra ficha");
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