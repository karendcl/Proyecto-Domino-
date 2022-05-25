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
            int max = 0;
            
            //Console.WriteLine("Desea que cambie la direccion del juego cada vez que alguien se pase?, si o no");
           // char change =  Console.ReadLine()[0];
            //bool changedirection = (change == 's' || change== 'S')? true : false;

           // while (!ValidSettings(cantplay, cadauno))
            //{
            Console.Clear();
            Console.WriteLine("Escriba el doble maximo de las fichas");
            max = int.Parse(Console.ReadLine()!);
     
            Console.WriteLine("Escriba cuantas fichas se van a repartir a cada jugador");
            cadauno = int.Parse(Console.ReadLine()!);

            Console.WriteLine("Escriba cuantos jugadores van a jugar");
            cantplay = int.Parse(Console.ReadLine()!); 
           // }

            Player[] players = new Player[cantplay];

            for (int i = 0; i < cantplay; i++)
            {
                Console.Clear();
                Console.WriteLine("Elija el tipo de jugador para el Player {0}. \n \n Escriba 0 para jugador humano. \n Escriba 1 para jugador botagorda. \n Escriba 2 para jugador random", i+1);
                //validar que sea entre 0 y no se

                int a = int.Parse(Console.ReadLine()!);

                switch (a)
                {
                    case 0:
                     players[i] = new HumanPlayer(new List<Ficha>(), i+1);
                     break;

                     case 1:
                     players[i] = new BotaGorda(new List<Ficha>(), i+1);
                     break;

                     case 2:
                     players[i] = new RandomPlayer(new List<Ficha>(), i+1);     
                     break;
                }
            }

           

            Board board = new Board(new List<Ficha>());

            Game game = new Game(board, players, true, max, cantplay,cadauno);


            //Console.WriteLine(players[0].ToString());
            //Ficha fichainicial = players[0].FirstFicha(game);

           // game.board.board.Insert(0, fichainicial);

            while (!EndGame(game))
            {
                foreach (var player in players)
                {
                    Console.Clear();
                    Console.WriteLine(player.ToString());
                    Console.WriteLine(game.board.ToString());

                 try
                {
                    Ficha ficha1; 

                    ficha1 = (game.board.board.Count ==0)? PrimerTurno(player,game) : Turno(player, game);

                    if (ficha1 is null){
                        //if (changedirection) game.SwapDirection();;
                    }

                    if (ficha1 != null)
                    {
                        int index = -1;

                        if (ficha1.Ambigua(game)){
                         index = player.ChooseSide(game);
                        }

                        game.board.AddFichaToGame(ficha1, index);
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
            game.Winner();
            

        }

        public static void SwapDirection(Game game){

           for (int i = 0; i < game.player.Length/2; i++)
           {
               Player temp = game.player[i];
               game.player[i] = game.player[game.player.Length -i-1];
               game.player[game.player.Length -i-1] = temp;
           }

        }


        public static bool ValidSettings(int players, int cadauno){

            if (players <= 0) return false;
            if (cadauno <=0) return false;
            if (PosiblesFichas.Count == 0) return false ;
            return (PosiblesFichas.Count - (players*cadauno) >= 0);
        }

        public static Ficha Turno(Player player, Game game)
        {
            return player.BestPlay(game);           
        }

        public static Ficha PrimerTurno(Player player, Game game)
        {
            return player.BestPlay(game);           
        }


        public static bool EndGame(Game game)
        {
            foreach (var player in game.player)
            {
                if (player.hand.Count ==0) return true;

                foreach (var ficha in player.hand)
                {
                    if (game.ValidPlay(ficha)) return false;
                }
            }
            return true;
        }


       /* public static List<Ficha> RepartirFichas(int cadauno)
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
            
        }*/

        /*public static void GenerarFichas(int max)
        {
            for (int i = 0; i <= max; i++)
            {
                for (int j = i; j <= max; j++)
                {
                    Ficha ficha = new Ficha(i, j);
                    PosiblesFichas.Add(ficha);
                }
            }
        }*/
    }
}
