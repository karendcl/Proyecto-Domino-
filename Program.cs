using System;
using System.Collections.Generic;
using System.Linq;

namespace Juego
{
     public class Program
    {
        public static List<Ficha> PosiblesFichas = new List<Ficha>();

        public static void Main(string[] args)
        {
            PosiblesFichas.Clear();

            int cadauno = 10;
            int cantplay = 4;

            while (!ValidSettings(cantplay, cadauno))
            {
            Console.Clear();
            Console.WriteLine("Escriba el doble maximo de las fichas");
            int max = int.Parse(Console.ReadLine()!);
            GenerarFichas(max);
     
            Console.WriteLine("Escriba cuantas fichas se van a repartir a cada jugador");
            cadauno = int.Parse(Console.ReadLine()!);

            Console.WriteLine("Escriba cuantos jugadores van a jugar");
            cantplay = int.Parse(Console.ReadLine()!); 
            }



            Player[] players = new Player[cantplay];

            for (int i = 0; i < cantplay; i++)
            {
                players[i] = new Player(RepartirFichas(cadauno));
            }

            var r = new Random();

            Ficha fichainicial = PosiblesFichas[r.Next(0,PosiblesFichas.Count)];

            Game game = new Game(new List<Ficha>(), players);

            game.board.Add(fichainicial);

            while (!EndGame(game))
            {
               
                int count=0;
                int turno = 1;

                foreach (var player in players)
                {
                    Console.Clear();
                    Console.WriteLine("Player {0}", turno);
                    Console.WriteLine();
                    turno++;

                  foreach (var ficha in player.hand)
                {
                    Console.Write( ficha.ToString());
                    count++;
                }  

                 Console.WriteLine(" ");

                 Console.WriteLine("Board:");

                 foreach (var item in game.board)
                {
                    Console.Write(item.ToString());
                }

                 try
                {
                    var ficha1 = Turno(player, game);

                    if (ficha1 != null)
                    {
                        int index = -1;
                        if (ficha1.Ambigua(game)){
                         index = player.ChooseSide(game);
                        }
                        game.AddFichaToGame(ficha1, index);
                        player.hand.Remove(ficha1);
                        if (player.hand.Count == 0){
                         break;
                        } 
                    }
                }
                catch{}

                Thread.Sleep(500);
                
                }
            }

            Console.Clear();
            Console.WriteLine("Game Over");
            Winner(game);
            

        }

        public static bool ValidSettings(int players, int cadauno){

            if (players <= 0) return false;
            if (cadauno <=0) return false;
            if (PosiblesFichas.Count == 0) return false ;
            return (PosiblesFichas.Count - (players*cadauno) >= 0);
        }


       public static void Winner(Game game){
         int[] scores = new int[game.player.Length];

        int count = 0;

        Console.WriteLine("All Scores:");

        foreach (var player in game.player)
        {
            foreach (var ficha in player.hand)
            {
                scores[count] += ficha.Suma();
            }
            count++;
            Console.WriteLine("Player {0} : {1} points", count, scores[count-1]);
        }

        int score = scores.Min();
        int index = Array.IndexOf(scores, score);

        Console.WriteLine("\n \n The winner is Player {0}, with a score of {1}", index +1, score);

        

        

       }

       public static int Suma(List<Ficha> hand){
           int suma = 0;

           foreach (var item in hand)
           {
               suma += item.Suma();
           }

           return suma;
       }

        public static Ficha Turno(Player player, Game game)
        {
            return player.BestPlay(game);           
        }


        public static bool EndGame(Game game)
        {

            if (game.player[0].hand.Count == 0 || game.player[1].hand.Count == 0) return true;

            foreach (var item in game.player[0].hand)
            {
                if (game.ValidPlay(item)) return false;
            }

            foreach (var item in game.player[1].hand)
            {
                if (game.ValidPlay(item)) return false;
            }

            return true;

        }


        public static List<Ficha> RepartirFichas(int cadauno)
        {
            int cantidadDisponible = PosiblesFichas.Count;

            if (!(cadauno * 2 < cantidadDisponible)) cadauno = cantidadDisponible / 2;

            int contador = 0;
            var lista = new List<Ficha>();

                while (contador != cadauno)
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

        public static void GenerarFichas(int max)
        {
            for (int i = 0; i <= max; i++)
            {
                for (int j = i; j <= max; j++)
                {
                    Ficha ficha = new Ficha(i, j);
                    PosiblesFichas.Add(ficha);
                }
            }
        }
    }
}
