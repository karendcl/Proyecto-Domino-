namespace Juego
{
    public class Player
    {
        public List<Ficha> hand;

        public Player(List<Ficha> fichas)
        {
            this.hand = fichas;
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
            var Possibles = PossiblePlays(game);
            int[] Scores = new int[Possibles.Count];
            int count = 0;

            foreach (var item in Possibles)
            {
                Scores[count] = Evaluate(item);
                count++;
            }

            var best = Scores.Max();

            if (best == 0) return null!;

            return Possibles.ElementAt(Array.IndexOf(Scores, best));
        }

        public int Evaluate(Ficha ficha)
        {
            int Score = 0;

            foreach (var item in hand)
            {
                if (item.IsMatch(ficha)) Score++;
            }

            if (ficha.IsDouble()) Score++;

            Score += ficha.Suma() / 2;

            return Score;
        }

        public virtual int ChooseSide(Game game){
            return 0;
        }

    }

    public class HumanPlayer : Player
    {
        public HumanPlayer(List<Ficha> fichas) : base(fichas)
        {
        }

        public bool OutRange(int index)
        {
            return index < -1 || index >= hand.Count;
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