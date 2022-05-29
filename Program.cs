using System;
using System.Collections.Generic;
using System.Linq;


namespace Juego
{
     public class Program
    {
        public static List<Ficha> PosiblesFichas = new List<Ficha>();

    [STAThread]

        public static void Main(string[] args)
        {
            PosiblesFichas.Clear();

            int cadauno = 10;
            int cantplay = 4;
            int max = 9;
            
            Console.WriteLine("Desea que cambie la direccion del juego cada vez que alguien se pase?, si o no");
            char change =  Console.ReadLine()![0];
            bool changedirection = (change == 's' || change== 'S')? true : false;

            while (!ValidSettings(cantplay, cadauno))
            {
            Console.Clear();
            Console.WriteLine("Escriba el doble maximo de las fichas");
            max = int.Parse(Console.ReadLine()!);
     
            Console.WriteLine("Escriba cuantas fichas se van a repartir a cada jugador");
            cadauno = int.Parse(Console.ReadLine()!);

            Console.WriteLine("Escriba cuantos jugadores van a jugar");
            cantplay = int.Parse(Console.ReadLine()!); 
            }

            Player[] players = new Player[cantplay];

            for (int i = 0; i < cantplay; i++)
            {
                Console.Clear();
                Console.WriteLine("Elija el tipo de jugador para el Player {0}. \n \n Escriba 0 para jugador humano. \n Escriba 1 para jugador botagorda. \n Escriba 2 para jugador random. \n Escriba 3 para jugar la primera que vea", i+1);
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

                     case 3:
                     players[i] = new FirstSee(new List<Ficha>(), i+1);
                     break;
                }
            }

           

            Board board = new Board(new List<Ficha>());

            Game game = new Game(board, players, changedirection, max, cantplay,cadauno);

            while (!game.EndGame())
            {
                for (int i = 0; i < players.Length; i++)
                {
                    Console.Clear();
                    Console.WriteLine(players[i].ToString());
                    Console.WriteLine(game.board.ToString());

                 try
                {
                    Ficha ficha1; 

                    ficha1 = (game.board.board.Count ==0)? PrimerTurno(players[i],game) : Turno(players[i], game);

                    if (ficha1 is null){
                        if (changedirection) game.SwapDirection(i);;
                    }

                    if (ficha1 != null)
                    {
                        int index = -1;

                        if (ficha1.Ambigua(game)){
                         index = players[i].ChooseSide(game);
                        }

                        game.board.AddFichaToBoard(ficha1, index);
                        players[i].hand.Remove(ficha1);
                        if (game.EndGame()){
                         break;
                        } 
                    }
                }
                catch{}

                Thread.Sleep(500);  
                }
                    
                
               // }
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
            //if (PosiblesFichas.Count == 0) return false ;
           //return (PosiblesFichas.Count - (players*cadauno) >= 0);
           return true;
        }

        public static Ficha Turno(Player player, Game game)
        {
            return player.BestPlay(game);           
        }

        public static Ficha PrimerTurno(Player player, Game game)
        {
            return player.BestPlay(game);           
        }


        

    }
}
