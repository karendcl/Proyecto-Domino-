Si se desea enlazar la implementacion de backend dada a otro entorno visual debe de conocer el siguiente reporte.

Para ello debe de tener un encargado de satisfacer los constructores de dichas clases ensamblandose primeramente las partidas y posteriormente las condiciones del constructor del torneo.

Una vez que se ha creado debe suscribirse a los eventos de la clase torneo ChampionStatus y CanChoose.

El Champion Status es un contenedor de todo los que sucede en el torneo se enviará uno cada vez que occura un movimiento del domino clasico en lo interno del torneo

GameStatus:

``cs
public class ChampionStatus //Esta clase muestra en pantalla todos los sucesos del a nivel de torneo y juego

    public Stack<GameStatus> FinishGame

    public List<PlayerStats> PlayerStats

    public bool HaveAWinner

    public List<IPlayer> Winners

    public bool ItsAnGameStatus

    public bool ItsAFinishGame

    public GameStatus gameStatus

    public bool FinishChampion

`````

Contiene una pila con todos los estados de las partidas (se explica más abajo)

Una pila de contenedor de la puntuación de los jugadores

````cs
    public class PlayerStats : IEquatable<PlayerStats>

    public IPlayer player

    public double puntuation

    ```

    contiene el jugador y su puntuación a nivel de torneo


      bool ItsAnGameStatus:

      True en caso que fue enviado por ser un estado de una partida false en caso contrario

       bool ItsAFinishGame

       True si envia el estatus de una partida acabada de finalizar


        GameStatus gameStatus

        El estado de partida mas actual

          bool FinishChampion

          True si se cumplio la finalizacion del torneo en ese caso será el ultimo enviado



            Inicio de un Torneo

          Para comenzar una partida debe invocarse el metodo Run() el cual es publico y ejecuta todas la partidas.
          Este finaliza cuando haya finalizado la partida.



         evento CanChoose:
          Este evento espera una respuesta booleana para continuar la partida o torneo (true).



          GameStatus:
```cs
    public class GameStatus
    {

    public List<IPlayer> winners

    public List<PlayerStats>

    public bool ItsAFinishGame

    public List<GamePlayerHand<IToken>> Hands

    public IBoard board

    public IPlayer actualPlayer

    public GamePlayerHand<IToken> PlayerActualHand

    }
`````

        public List<IPlayer> winners:
        Contiene en caso de haber los ganadores de la partida

        public List<PlayerStats>:
        Puntuación de los jugadores

        public bool ItsAFinishGame
        True si se es el ultimo estado de esta partida

         public List<GamePlayerHand<IToken>> Hands
         La mano de los jugadores actual

         public IBoard board
         Tablero en estrado actual

         public IPlayer actualPlayer
         El jugador del turno que se esta jugando

         public GamePlayerHand<IToken> PlayerActualHand
         Mano del jugador en el turno actual




         Nota Todos los valores y datos se pasan por Clonacion.

```

```
