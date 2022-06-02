using System;
using System.Collections.Generic;
using System.Linq;


namespace Juego
{
     public class Program
    {
        public static List<Token> PosiblesTokens = new List<Token>();

    [STAThread]

        public static void Main(string[] args)
        {
            PosiblesTokens.Clear();

            int cadauno = 10;
            int cantplay = 4;
            int max = 9;
            
            Console.WriteLine("Desea que cambie la direccion del juego cada vez que alguien se pase?, si o no");
            char change =  Console.ReadLine()![0];
            bool changedirection = (change == 's' || change== 'S')? true : false;

            while (!ValidSettings(cantplay, cadauno))
            {
            Console.Clear();
            Console.WriteLine("Escriba el doble maximo de las Tokens");
            max = int.Parse(Console.ReadLine()!);
     
            Console.WriteLine("Escriba cuantas Tokens se van a repartir a cada jugador");
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
                     players[i] = new HumanPlayer(new List<Token>(), i+1);
                     break;

                     case 1:
                     players[i] = new BotaGorda(new List<Token>(), i+1);
                     break;

                     case 2:
                     players[i] = new RandomPlayer(new List<Token>(), i+1);     
                     break;

                     case 3:
                     players[i] = new FirstSee(new List<Token>(), i+1);
                     break;
                }
            }

           

            Board board = new Board(new List<Token>());
            Judge judge = new Judge();

            Game game = new Game(board, players, changedirection, max, cantplay,cadauno, judge);

            while (!judge.EndGame(game))
            {
                for (int i = 0; i < players.Length; i++)
                {
                    Console.Clear();
                    Console.WriteLine(players[i].ToString());
                    Console.WriteLine(game.board.ToString());

                 try
                {
                    Token Token1; 

                    Token1 = (game.board.board.Count ==0)? PrimerTurno(players[i],game) : Turno(players[i], game);

                    if (Token1 is null){
                        if (changedirection) game.SwapDirection(i);;
                    }

                    if (Token1 != null)
                    {
                        int index = -1;

                        if (Token1.Ambigua(game)){
                         index = players[i].ChooseSide(game);
                        }

                        game.board.AddTokenToBoard(Token1, index);
                        players[i].hand.Remove(Token1);
                        if (judge.EndGame(game)){
                         break;
                        } 
                    }
                }
                catch{}

                Thread.Sleep(500);  
                }
            }

            WriteStats(game);

        }

        public static void WriteStats(Game game){
            Console.Clear();
            Console.WriteLine("Game Over");
            
            Console.WriteLine("\n {0} \n  Scores of this game:", game.board.ToString() );

            foreach (var play in game.player)
            {
                Console.WriteLine("Player {0}: {1}", play.Id, play.Score());
            }
            
            Console.WriteLine("\n Winner(s): ");

            foreach (var winner in game.judge.Winner(game))
            {
                Console.WriteLine("Player {0}", winner.Id);
            }
        }


        public static bool ValidSettings(int players, int cadauno){

            if (players <= 0) return false;
            if (cadauno <=0) return false;
            //if (PosiblesTokens.Count == 0) return false ;
           //return (PosiblesTokens.Count - (players*cadauno) >= 0);
           return true;
        }

        public static Token Turno(Player player, Game game)
        {
            return player.BestPlay(game);           
        }

        public static Token PrimerTurno(Player player, Game game)
        {
            return player.BestPlay(game);           
        }


        

    }
}
