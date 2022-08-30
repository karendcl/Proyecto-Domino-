namespace Game;
public class observador
{
    public observador() { }

    public int Speed{get ;set;}

    public int IntResponses(string type, string[] options)
    {
        int current = 0;

        if (options is null){
            return GetIntNumber(type);
        }
        else
        while (true)
        {
         Console.Clear();   
         if (current==-1) current=options.Length-1; 
         if (current==options.Length) current=0;

         Console.ForegroundColor = ConsoleColor.Cyan;   
         System.Console.WriteLine(type); 
         Console.ForegroundColor = ConsoleColor.White;

         for (int i = 0; i < options.Length; i++)
        {
            if(i==current) 
            System.Console.WriteLine($"[{i+1}] ==> {options[i]}");
            else System.Console.WriteLine($"[{i+1}] {options[i]}");
        }   


        System.Console.WriteLine("\n\n Use las flechitas del teclado para elegir su opcion");

        switch (Console.ReadKey().Key)
        {
            case (ConsoleKey.LeftArrow):
            case (ConsoleKey.UpArrow): current--; break;
            case (ConsoleKey.RightArrow):
            case (ConsoleKey.DownArrow): current++; break;
            case (ConsoleKey.Enter) : return current;      
            default: break;
        }
        }

        
    }

    public int GetIntNumber(string message){
        int current = 0;

         while (true)
        {
         if (current <0) current=0;   
         Console.Clear();   
         Console.ForegroundColor = ConsoleColor.Cyan;   
         System.Console.WriteLine(message); 
         Console.ForegroundColor = ConsoleColor.White;

        System.Console.WriteLine(current); 

        System.Console.WriteLine("\n\n Use las flechitas del teclado para elegir su opcion");

        switch (Console.ReadKey().Key)
        {
            case (ConsoleKey.RightArrow):
            case (ConsoleKey.UpArrow): current++; break;

            case (ConsoleKey.LeftArrow):
            case (ConsoleKey.DownArrow): current--; break;
            case (ConsoleKey.Enter) : return current;      
            default: break;
        }
        }

    }

    public bool BoolResponses(string arg)
    {

        string[] options = new string[2];
        options[0] = "Si";
        options[1] = "No";

        int choice = IntResponses(arg, options);

        switch (choice)
        {
            case 0: return true;
            case 1: return false;
            default: return true;
        }
    
    }

    public void SettingSpeed(){
        string word = "Elija la velocidad de su juego";
        string[] options = new string[3];
        options[0] = "Velocidad Rapida";
        options[1] = "Velocidad Media";
        options[2] = "Velocidad Lenta";

        int choice = IntResponses(word,options);
        
        switch (choice)
        {
            case 0 : this.Speed=100; break;
            case 1 : this.Speed=600; break;
            case 2 : this.Speed=1000; break;
            default: this.Speed=600; break;
        }
    }

    public void PrintStart(){
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        string message = "Bienvenido a este torneo/juego de dominó. \n";
        message += "A continuación podrá escoger las características de su juego\n";
        message += "\n\nPresione una tecla para continuar...";
        
        for (int i = 0; i < message.Length; i++)
            {
                Console.Write(message[i]);
                System.Threading.Thread.Sleep(60);
            }

        Console.ReadKey();

        Console.ForegroundColor = ConsoleColor.White;        
    }
    public void PrintChampionStatus(ChampionStatus championStatus)
    {
        if (this.Speed !=100 && this.Speed!=600 && this.Speed!=1000) SettingSpeed();

        GameStatus gameStatus = championStatus.gameStatus;
        if (championStatus.ItsAnGameStatus)
        {
            Console.ResetColor(); 
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            
            PrintGameChange(gameStatus);
        }
         if (championStatus.ItsAFinishGame)
        {    
            Console.ResetColor();
            PrintFinishGame(gameStatus);
        }
        if (championStatus.FinishChampion)
        {
            Console.ResetColor(); 
            PrintFinishChampion(championStatus);
        }

    }

    protected void PrintFinishChampion(ChampionStatus championStatus)
    {
        //cuando acabe el torneo se escriben los resultados. 
        //-los jugadrores y sus puntuaciones 
        //-los ganadores del torneo
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        System.Console.WriteLine("El torneo ha terminado. ");

        Console.ForegroundColor = ConsoleColor.DarkBlue;

        System.Console.WriteLine("\nGanadores del Torneo: \n");

        Console.ForegroundColor = ConsoleColor.White;

        foreach (var item in championStatus.Winners)
        {
            System.Console.WriteLine($" Player {item.Id}");
        }

        System.Console.WriteLine("\nPresiona una tecla para continuar...");

        Console.ReadKey();
     
    }

    protected void PrintGameChange(GameStatus gameStatus)
    {
        //en lo que se efectua el juego se escriben en consola las manos de los jugadores y el tablero
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        System.Console.WriteLine(gameStatus.board);
        Thread.Sleep(Speed);
    
        Console.ForegroundColor = ConsoleColor.DarkCyan;
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
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        System.Console.WriteLine("El juego ha terminado: \n");
        Console.ForegroundColor = ConsoleColor.White;

        System.Console.WriteLine("El tablero ha terminado asi:\n");
        foreach (var item in gameStatus.board.board)
        {
            System.Console.Write(item);
        }

        System.Console.WriteLine("\n \n Puntuaciones: ");
        foreach (var item in gameStatus.PlayerStats)
        {
            System.Console.WriteLine($"Player {item.player.Id} con {item.punctuation} puntos");
        }
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        System.Console.WriteLine("\n Ganadores de la partida: ");
        Console.ForegroundColor = ConsoleColor.White;

        foreach (var item in gameStatus.winners)
        {
            System.Console.WriteLine($"Player {item.Id}");
        }
         System.Console.WriteLine("Presiona una tecla para continuar...");
        Console.ReadKey();

    }


}
