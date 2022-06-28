using System;
using System.Collections.Generic;
using System.Linq;


namespace Game;

public class Program
{
    public static void Main(string[] args)
    {
        System.Console.WriteLine("Desea Empezar una nueva partida");
        // string msg= Console.ReadLine()!;
        string msg = "si";
        bool start = msg[0] == 's' ? true : false;
        if (!start) Console.Clear();

        Console.WriteLine("Cuantos plays desea jugar");
        int cantPlays = 1;
        int.TryParse(Console.ReadLine()!, out cantPlays);
        // cantPlays=2;

        Observer observer = new Observer();
        observer.Start();
    }

}
