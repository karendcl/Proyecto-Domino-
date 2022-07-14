namespace Game;
public class observador
{
    public observador() { }

    public int Speed{get ;set;}

    public int IntResponses(string arg)
    {
        Console.Clear();
        System.Console.WriteLine(arg);
        int x = -1;
        int.TryParse(Console.ReadLine(), out x);
        Console.Clear();
        return x;
    }

    public bool BoolResponses(string arg)
    {
        Console.ResetColor();

        string x = string.Empty;
        bool a = true;
        do
        {
            Console.Clear();
            System.Console.WriteLine(arg + "  ?" + " Sí/No");
            x = Console.ReadLine()!.ToLower();
            if (x != null && x.Length > 0)
            {
                a = (x[0] == 's' || x[0] == 'n');
            }

        } while (!a);
        Console.Clear();
        if (x.Length > 0 && x[0] == 's') return true;
        return false;
    }

    public void SettingSpeed(){
        System.Console.WriteLine(" Presione la tecla [1] para velocidad rapida \n Presione la tecla [2] para velocidad media \n Presione la tecla [3] para velocidad lenta");
        switch (Console.ReadKey().Key)
        {
            case (ConsoleKey)1 : this.Speed=100; break;
            case (ConsoleKey)2 : this.Speed=600; break;
            case (ConsoleKey)3 : this.Speed=1000; break;
            default: this.Speed=500; break;
        }
    }
    public void PrintChampionStatus(ChampionStatus championStatus)
    {
        GameStatus gameStatus = championStatus.gameStatus;
        if (championStatus.ItsAnGameStatus)
        {
            if (this.Speed !=100 && this.Speed!=500 && this.Speed!=1000) SettingSpeed(); 
            Console.ResetColor(); 
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            
            PrintGameChange(gameStatus);
        }
         if (championStatus.ItsAFinishGame)
        {
            if (this.Speed !=100 && this.Speed!=500 && this.Speed!=1000) SettingSpeed(); 
            Console.ResetColor();
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.White;
            PrintFinishGame(gameStatus);
        }
        if (championStatus.FinishChampion)
        {
            Console.ResetColor(); 
            if (this.Speed !=100 && this.Speed!=500 && this.Speed!=1000) SettingSpeed(); 
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            PrintFinishChampion(championStatus);
        }

    }

    protected void PrintFinishChampion(ChampionStatus championStatus)
    {
        //cuando acabe el torneo se escriben los resultados. 
        //-los jugadrores y sus puntuaciones 
        //-los ganadores del torneo
        Console.Clear();
        foreach (var item in championStatus.PlayerStats)
        {
            System.Console.WriteLine($"Player {item.player.Id}. Puntuacion: {item.punctuation} puntos");
        }


        System.Console.WriteLine("Ganadores del Torneo: \n");

        foreach (var item in championStatus.Winners)
        {
            System.Console.WriteLine($" Player {item.Id} \n");
        }

        System.Console.WriteLine("Presiona una tecla para continuar...");

        Console.ReadKey();
    }

    protected void PrintGameChange(GameStatus gameStatus)
    {
        //en lo que se efectua el juego se escriben en consola las manos de los jugadores y el tablero
        Console.Clear();
        System.Console.WriteLine(gameStatus.board);
        Thread.Sleep(Speed);
    
        Console.Clear();
        System.Console.WriteLine(gameStatus.actualPlayer + "\n" + gameStatus.PlayerActualHand);
        Thread.Sleep(Speed);
        
    }

    protected void PrintFinishGame(GameStatus gameStatus)
    {
        //cuando acabe el juego se escriben los resultados. 
        //-los jugadrores y sus puntuaciones 
        //-los ganadores del juego
        Console.Clear();
        System.Console.WriteLine("El juego ha terminado: \n \n Puntuaciones: \n");
        foreach (var item in gameStatus.PlayerStats)
        {
            System.Console.WriteLine($"Player {item.player.Id} con {item.punctuation} puntos");
        }

        System.Console.WriteLine("\n Ganadores de la partida: \n");

        foreach (var item in gameStatus.winners)
        {
            System.Console.WriteLine($"Player {item.Id}");
        }
         System.Console.WriteLine("Presiona una tecla para continuar...");
        Console.ReadKey();

    }


}
