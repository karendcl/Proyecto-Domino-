namespace Domino
{
    public class Player
    {
        public List<Ficha> hand;

        public Player(List<Ficha> fichas){
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

        public  Ficha BestPlay(Game game)
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

            if (best == 0) return null;

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

    }
}