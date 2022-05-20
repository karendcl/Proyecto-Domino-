using System;
using System.Collections.Generic;
using System.Linq;

namespace Domino
{
    public class Program
    {
        public static List<Ficha> PosiblesFichas = new List<Ficha>();

        public static void Main(string[] args)
        {
            PosiblesFichas.Clear();

            int max = 9;
            GenerarFichas(max);
            
            
            foreach (var item in PosiblesFichas)
            {
                Console.WriteLine(item.ToString());
            }

            int cadauno = 10;

            Player[] players = {
                new  Player(RepartirFichas(cadauno)),
                new  Player(RepartirFichas(cadauno)),
            };

            var r = new Random();

            Ficha fichainicial = PosiblesFichas[r.Next(0,PosiblesFichas.Count)];

            Game game = new Normal(new List<Ficha>(), players);

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
                        game.AddFichaToGame(ficha1, -1);
                        player.hand.Remove(ficha1);
                        if (player.hand.Count == 0){
                         break;
                        } 
                    }
                }
                catch{}

                Thread.Sleep(1000);
                
                }
            }

            Console.Clear();
            Console.WriteLine("Game Over");
            Winner(game);
            

        }


       public static void Winner(Game game){

           int suma1 = Suma(game.player[0].hand);
           int suma2 = Suma(game.player[1].hand);

           if (suma1 > suma2) Console.WriteLine("Gano player 2.");
           else if (suma2> suma1) Console.WriteLine("Gano player 1.");
           else Console.WriteLine("Empate");
        
            Console.WriteLine("Score del player 1 {0}, score del player 2 {1}", suma1, suma2);

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
