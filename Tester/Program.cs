using System;
using System.Collections.Generic;
using System.Linq;


namespace Game;

public class Program
{
    public static void Main(string[] args)
    {


        Events events = new Events();

        observador obs = new observador();

        ChampionStart start = new ChampionStart(obs);


        start.Run();







    }

}


