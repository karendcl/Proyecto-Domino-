using System;
using System.Collections.Generic;
using System.Linq;

namespace Domino 
{
    public class Program
    {
        public static List<Ficha> PosiblesFichas = new List<Ficha>();
        public static List<Ficha> ManodelUsuario = new List<Ficha>();
        public static List<Ficha> ManoDeLaCompu = new List<Ficha>();

        public static List<Ficha> Juego = new List<Ficha>();

        

        public static void Main(string[] args)
        {
            PosiblesFichas.Clear();

            int max = 5;
            GenerarFichas( max);

            int cadauno = 5;
            RepartirFichas ( cadauno);

            Ficha fichainicial = PosiblesFichas[0];

            Game game = new Game(Juego, ManoDeLaCompu, ManodelUsuario);

            game.board.Add(fichainicial);

            while (!EndGame(game))
            {
                Console.Clear();
                Console.WriteLine("Fichas Mias");

                foreach (var item in ManodelUsuario)
                {
                    Console.Write(item.ToString());
                }

                Console.WriteLine(" ");

                Console.WriteLine("Board:");

                foreach (var item in Juego)
                {
                    Console.Write( item.ToString());
                }
                
                Console.WriteLine(" ");

                try
                {
                  var ficha = TirnoDelUsuario(game);
                  if (ficha != null) {
                    AddFichaToGame(ficha);  
                    ManodelUsuario.Remove(ficha);
                    if (ManodelUsuario.Count ==0) break;
                }  
                }
                catch (System.Exception)
                { }

              try
                {
                   var ficha1 = TurnoDeCompu(game);

                 if (ficha1 != null) {
                     AddFichaToGame(ficha1);
                    ManoDeLaCompu.Remove(ficha1);
                    if (ManoDeLaCompu.Count ==0) break;
                }
                }  
                
                catch (System.Exception)
                {  }

             
            Thread.Sleep(100);    
            }

            Console.Clear();
            Console.WriteLine("Game Over");  
            Console.WriteLine("Winner: {0}", Winner());        
          
        }

        public static void AddFichaToGame(Ficha ficha){
           Ficha first = Juego.First();

           if (ficha.Contains(first.Parte1)){
               if (first.Parte1 == ficha.Parte1){
                   ficha.SwapFicha();
                   Juego.Insert(0,ficha);
                   return;
               }
               
               Juego.Insert(0, ficha);
               return;
           }

           Ficha last = Juego.Last();

            if (ficha.Contains(last.Parte2)){
               if (ficha.Parte2 == last.Parte2){
                   ficha.SwapFicha();
                   Juego.Add(ficha);
                   return;
               }
               
               Juego.Add(ficha);
               return;
           }



        }

        public static string Winner(){
            var PointsUser = 0;;

            foreach (var item in ManodelUsuario)
            {
                PointsUser += item.Suma();
            }

            var PointsComp = 0;

            foreach (var item in ManoDeLaCompu)
            {
                PointsComp += item.Suma();
            }

            if (PointsComp < PointsUser) return "me";
            if (PointsComp > PointsUser) return "you";
            else return "tie";


        }

        public static Ficha TurnoDeCompu(Game game){

            var Possible = PossiblePlays(game);
            return BestPlay(Possible);

        }

        public static Ficha TirnoDelUsuario(Game game){
            Console.WriteLine("Escriba el indice de la ficha a jugar. Comenzando desde 0. Si no lleva escriba -1");
            int ToPlay = int.Parse(Console.ReadLine());

            if (ToPlay==-1) return null;

            while (! game.ValidPlay(game.UserFichas[ToPlay])){
                Console.WriteLine("No haga trampa! Escriba otra ficha");
                ToPlay = int.Parse(Console.ReadLine());
            }

            return game.UserFichas[ToPlay];

        }

        public static bool EndGame(Game game){

            if (ManoDeLaCompu.Count == 0 || ManodelUsuario.Count == 0) return false;

            foreach (var item in game.ComputerFichas)
            {
               if (game.ValidPlay(item)) return false; 
            }

            foreach (var item in game.UserFichas)
            {
               if (game.ValidPlay(item)) return false; 
            }

            return true;

        } 
        public static Ficha BestPlay(List<Ficha> Possibles){
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

        public static int Evaluate(Ficha ficha){
            int Score = 0;

            foreach (var item in ManoDeLaCompu)
            {
             if (item.IsMatch(ficha)) Score++;   
            }

            if (ficha.IsDouble()) Score++;

            Score += ficha.Suma() / 2;

            return Score;
        }

        public static List<Ficha> PossiblePlays( Game game){

            var CanPlay = new List<Ficha>();

            foreach (var item in game.ComputerFichas )
            {
                if (game.ValidPlay(item))  CanPlay.Add(item);
            }

            return CanPlay;
        }

        

        public static void RepartirFichas( int cadauno){
           int cantidadDisponible = PosiblesFichas.Count;

           if (!(cadauno*2 < cantidadDisponible))  cadauno = cantidadDisponible /2;
           
           int contador;
           var lista = new List<Ficha>();

           for (int i = 0; i < 2; i++)
           {
               contador=0;
               if (i==0) lista = ManoDeLaCompu;
               else lista = ManodelUsuario;

               while (contador != cadauno)
               {
                   var r = new Random();
                   var index = r.Next(0,cantidadDisponible);
                   cantidadDisponible--;
                   contador++;
                   lista.Add(PosiblesFichas.ElementAt(index));
                   PosiblesFichas.RemoveAt(index);                   
               }
           }
        }

        public static void GenerarFichas(int max){
            for (int i = 0; i <= max; i++)
            {
                for (int j = i; j <= max ; j++)
                {
                    Ficha ficha = new Ficha(i,j);
                    PosiblesFichas.Add(ficha); 
                }
            }
        }
    }
}
