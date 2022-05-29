namespace Juego
{
          public class Game : IRules
    {
        public Board board { get; set; }
        public Player[] player{get; set;}
        public bool SwitchDirection{get; set;}
        public int MaxDouble{get; set;}
        public int Players{get; set;}
        public int FichasForEach{get; set;}


        public Game(Board board, Player[] players, bool direction, int max, int plays, int rep)
        {
            this.board = board;
            this.player = players;
            this.SwitchDirection = direction;
            this.MaxDouble = max;
            this.Players = plays;
            this.FichasForEach = rep;

           // GenerarFichas();
            AssignFichas();
        }

        public void AssignFichas(){

            List<Ficha> PosiblesFichas =  GenerarFichas(); 

            foreach (var play in player)
            {
                play.hand = RepartirFichas(PosiblesFichas);
            }
        }


        public List<Ficha> RepartirFichas(List<Ficha> PosiblesFichas){
            int cantidadDisponible = PosiblesFichas.Count;

            int contador = 0;
            var lista = new List<Ficha>();

                while (contador != FichasForEach)
                {
                    var r = new Random();
                    var index = r.Next(0, cantidadDisponible);
                    cantidadDisponible--;
                    contador++;
                    lista.Add(PosiblesFichas.ElementAt(index));
                    PosiblesFichas.RemoveAt(index);
                }

            return lista;
        }

        public List<Ficha> GenerarFichas(){

            List<Ficha> PosiblesFichas = new List<Ficha>();

             for (int i = 0; i <= this.MaxDouble; i++)
            {
                for (int j = i; j <= this.MaxDouble; j++)
                {
                    Ficha ficha = new Ficha(i, j);
                    PosiblesFichas.Add(ficha);
                }
            }

            return PosiblesFichas;
        } 

        public override string ToString(){
           string a = "Resultados del juego:  \n";

           a+= board.ToString();

          return a;     
        }

        public bool ValidPlay(Ficha ficha)
        {
            if (board.board.Count==0) return true;
            if (ficha.Contains(board.First().Parte1)) return true;
            if (ficha.Contains(board.Last().Parte2)) return true;
            return false;
        }

        

       public virtual void Winner(){
        int[] scores = new int[player.Length];

        int count = 0;

        Console.WriteLine(this.ToString());

        Console.WriteLine("\n\nAll Scores:");

        foreach (var player in player)
        {
            foreach (var ficha in player.hand)
            {
                scores[count] += ficha.Suma();
            }
            count++;
            Console.WriteLine("Player {0} : {1} points", count, scores[count-1]);
        }
 
         var pl =0;

        foreach (var player in player)
        {
            if (player.hand.Count==0){
               Console.WriteLine("\n \n The winner is Player {0}, because they ran out of fichas", pl +1);
               return;
            }
            pl++;
        }

        int score = scores.Min();
        int index = Array.IndexOf(scores, score);

        Console.WriteLine("\n \n The winner is Player {0}, with a score of {1}", index +1, score);
       }

        public void SwapDirection(int player){

          if (SwitchDirection){
              Player[] players = new Player[this.Players];

               for (int i = 0; i < players.Length; i++)
                 {
                     if (player ==0) player = players.Length;

                     players[i] = this.player[player-1];
                     player --;   
                 }

                 this.player = players;
          
          }


        }

        public virtual bool EndGame()
        {
            foreach (var player in this.player)
            {
                if (player.hand.Count ==0) return true;

                foreach (var ficha in player.hand)
                {
                    if (this.ValidPlay(ficha)) return false;
                }
            }
            return true;
        }
    }
}



