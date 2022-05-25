namespace Juego
{
    public class Player: IEstrategia
    {
        public List<Ficha> hand;
        public int Id;

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

        public virtual Ficha BestPlay(Game game)
        {

            List<Ficha> Possibles;
            Possibles = PossiblePlays(game);

            int[] Scores = new int[Possibles.Count];
            int count = 0;

            foreach (var item in Possibles)
            {
                Scores[count] = Evaluate(item, game);
                count++;
            }

            var best = Scores.Max();

            if (best == 0) return null!;

            return Possibles.ElementAt(Array.IndexOf(Scores, best));
        }

        public virtual int Evaluate(Ficha ficha, Game game)
        {
            return ficha.Suma();
        }

        public virtual int ChooseSide(Game game){
            return 0;
        }

    }

    public class BotaGorda : Player, IEstrategia{
        public BotaGorda(List<Ficha> fichas, int id) : base(fichas,id)
        {
        }

        public override int Evaluate(Ficha ficha, Game game){
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

    public class RandomPlayer : Player, IEstrategia{
        public RandomPlayer(List<Ficha> fichas, int id) : base(fichas,id)
        {
        }

        public override int Evaluate(Ficha ficha, Game game){
             var r = new Random();
             return r.Next(1,100);
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