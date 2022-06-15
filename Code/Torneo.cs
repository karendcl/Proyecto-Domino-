using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace Juego;



//Torneo Se encarga de crear los juegos y procesarlos
public class Championship
{
    public int Champions { get; private set; }
    private Game[] Games;
    Observer observer = new Observer();

    public Championship(int cantTorneos)
    {
        this.Champions = cantTorneos;
        this.Games = new Game[cantTorneos];
        run();
    }


    public void run()
    {
        Games = observer.SelectGameTypes(Games);

        for (int i = 0; i < Games.Length; i++)
        {
            Game game = Games[i];

            PlayAGame(game);
            System.Console.WriteLine("Termino el juego {0}", i + 1);
            Thread.Sleep(500);

            if (!ContinueGames()) { observer.WriteStats(game); break; }
            observer.WriteStats(game);
        }

    }

    public bool ContinueGames()
    {
        return observer.Msg("Desea seguir jugando?, si / no");
    }

    private bool PlayAGame(Game game)
    {

        //game.AssignTokens();
        //Control que todo se pase por referencia
        // Debug.Assert(game.board.board.Count>1,"Problemas con valores de referencia");

        IJudge judge = game.judge;
        List<IPlayer> player = game.player;
        IBoard board = game.board;

        while (!judge.EndGame(game)) //mientras no se acabe el juego
        {
            for (int i = 0; i < game.player.Count; i++) //turno de cada jugador
            {
                if (judge.EndGame(game)) break;


                // Console.WriteLine(player[i].ToString());
                observer.PaintPlayerInConsole(player[i]);
                // Console.WriteLine(game.board.ToString());

                observer.PaintBord(board);

                observer.Clean();

                Token Token1 = Turno(player[i], game);  //la ficha que se va a jugar                     

                if (Token1 is null || !game.judge.ValidPlay(game.board, Token1))
                { //si es nulo, el jugador se ha pasado
                    game.SwapDirection(i);
                }

                if (Token1 != null) //si no es nulo, entonces si lleva
                {
                    if (game.judge.ValidPlay(game.board, Token1))
                    { //si es valido
                        int index = -1;

                        if (game.judge.PlayAmbigua(Token1, game.board))
                        {  //si se puede jugar por ambos lados, se le pide que escoja un lado
                            index = player[i].ChooseSide(game);
                        }

                        game.judge.AddTokenToBoard(Token1, game.board, index);
                        player[i].hand.Remove(Token1); //se elimina la ficha de la mano del jugador
                    }
                }

                Thread.Sleep(100);
            }


        }
        return true;

    }

    private Token Turno(IPlayer player, Game game)
    {
        return player.BestPlay(game);
    }



}



