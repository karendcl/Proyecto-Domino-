namespace Domino
{
     public abstract class Game{
       public List<Ficha> board {get; set;}
       public List<Ficha> ComputerFichas {get; set;}
       public List<Ficha> UserFichas {get; set;}

       public Game(List<Ficha> board,List<Ficha> ComputerFichas, List<Ficha> UserFichas ){
           this.board = board;
           this.ComputerFichas = ComputerFichas;
           this.UserFichas = UserFichas;

       }
       public abstract bool ValidPlay(Ficha ficha);

       public abstract Ficha BestPlay();

       

    }

    public class Normal : Game {
      
       public Normal(List<Ficha> board,List<Ficha> ComputerFichas, List<Ficha> UserFichas) : base(board, ComputerFichas, UserFichas){

       }

      public override bool ValidPlay(Ficha ficha){
         if (ficha.Contains(board.First().Parte1)) return true;
           if (ficha.Contains(board.Last().Parte2)) return true;
           return false;
      }

       public  List<Ficha> PossiblePlays( ){

            var CanPlay = new List<Ficha>();

            foreach (var item in ComputerFichas )
            {
                if (ValidPlay(item))  CanPlay.Add(item);
            }

            return CanPlay;
        }

          public override Ficha BestPlay(){

            var Possibles = PossiblePlays();
            int[] Scores = new int[Possibles.Count];
            int count = 0;

            foreach (var item in Possibles)
            {
                Scores[count] = Evaluate(item);
                count++;
            }

            var best =  Scores.Max();

            if (best==0) return null;

            return Possibles.ElementAt(Array.IndexOf(Scores, best));
        }

        public  int Evaluate(Ficha ficha){
            int Score = 0;

            foreach (var item in ComputerFichas)
            {
             if (item.IsMatch(ficha)) Score++;   
            }

            if (ficha.IsDouble()) Score++;

            Score += ficha.Suma() / 2;

            return Score;
        }


        
    }
}



