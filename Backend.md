Jerarquia del backend

La lógica del juego consiste en el desacople de las funcionalidades.

La explicacion jerarquica del mismo se hará de mayor a menor relevancia dentro de este

Como el juego del domino consiste en un torneo
El torneo el cual es una implementacion de la interfaz

```cs
     public interface  IChampionship<TStatus>
```

Como concepto abstracto consiste en un conjunto de partidas independientes entre si con ciertas reglas como la de finalizacion de este, el/los ganadores y la validacion de que jugadores deben continuar a la siguiente partida a los cuales delega esas funcionalidades dada que el comportamiento de este es solo de ejecutar las partidas.

```cs
public void Run()  (Debe ser inicializado externamente)
```

El Run debe implementarse el conjunto de partidas de la implementacion IGame<TStatus> e invocar su método inicializador.

Ademas debe dar paso a otra clase responsable de portar la informacion con la entidad grafica bajo los eventos: Status:El cual bajo reglas internas del torneo de invoca a una clase encargada de portar la información y ser enviada hacia la parte visual esta clase auxiliar puede ser implementada a bajo la necesidad a implementar y al utilizarse eventos puede ser utilizado en varios ámbitos graficos.

La implementación dada es:

```cs
    public class ChampionStatus
```

Dada esta implementación se reconoce que al ser un mero portador las variacion necesarias pudieran ser por medio de la herencia de esta clase dado que recoge la escencia de las acciones del torneo y el modo de interpretarlas por parte de la implementacion grafica es totalmente libre.

Sus funciones principales es de portar el conocimiento de conocer El ultimo estado de la partida en ejecución se deja los meétods de este (serán explicados en otro documento el cual dará a conocer como debe enlazarse con la interfaz gráfica)

```cs
    public Stack<GameStatus> FinishGame
    public List<PlayerStats> PlayerStats
    public bool HaveAWinner
    public List<IPlayer> Winners
    public bool ItsAnGameStatus
    public bool ItsAFinishGame
    public GameStatus gameStatus
    public bool FinishChampion

```

El evento CanContinue: se encarga unicamente de esperar confirmacion de cualquier ente exterior sobre si se puede o no continuar dicho torneo. Para ello se determino que se debe implementar un Enum Orders

```cs
 public virtual event Predicate<Orders> CanContinue;
 public virtual event Action<TStatus> status;

```

Explicación de las clases auxiliares:

En terminos globales se tiene un conjuno de jugadores a los cuales la responsabilidad de contenerlos y decicidir si se configuro para jugar en dicha partida o no, es delegada a la clase:

```cs
      public class PlayersCoach
```

la cual solo determina si un jugador desea o no jugar en dicha clase.

Para Interpretar las reglas del Torneo se auxilia de la implementación de la interfaz

```cs
IChampionJudge<Tstatus>
```

Nota: El tipo Tstatus es determinado por el del tipo que implementa la Interfaz IGame<TStatus>

Su funcion es determinar si es valido continuar y quien debe continuar para ello se auxilia de las reglas del torneo (es el encargado de la interpretación de las mismas) Las cuales son:
Condicion de finalizacion del torneo
Ganador o ganadores del torneo
Si es valido o no continuar jugando por parte de un jugador
Y como se calcula es score de dicho jugador a nivel de torneo

Sus metodos fundamentales son

```cs
 public virtual void Run(List<IPlayer> players)
```

El cual debe de invocarse antes de comenzar el torneo para dar a conocer a este los posibles jugadores

```cs
 public virtual bool EndGame(List<IGame<GameStatus>> game)
```

El cual basandose en los criterios o reglas del juego determina si debe o no finalizar el torneo

Por defecto el torneo finalizará a lo sumo cuando se hayan jugado todas las partidas.

```cs
 public virtual bool ValidPlay(IPlayer player)
```

En ella determina si un jugador puede o no continuar jugando el torneo

```cs
 public virtual List<IPlayer> Winners()
```

Devuelve una lista con el/los ganador/es del torneo

Una Partida:

Como concepto una partida debe contener todos los elementos que aseguren el funcionamiento de una partida de domino tipica:

```cs
public interface IGame<TStatus> : ICloneable<IGame<TStatus>>
{

    event Action<TStatus>? GameStatus;

    event Predicate<Orders> CanContinue;

    IBoard? board { get; }

    List<IPlayer>? GamePlayers { get; }

    double PlayerScore(IPlayer player);

    IGame<TStatus> Clone();

    TStatus PlayAGame(IBoard board, List<IPlayer> players);

    List<IPlayerScore> PlayerScores();

    string ToString();

    List<IPlayer> Winner();
}

```

Esta interfaz contiene la escencia de una partida La implentacíon utilizada promueve el desacople de las funcionalidades de esta.

La clase implementada debe dar a conocer que tipo de implementacion de TStatus es Necesaria
Bajo la implementación dada está la clase game estatus (A explicar en el documento de como enlazar con la interfaz gráfica bajo esta implementación )

```cs
   public interface IGame<TStatus> : ICloneable<IGame<TStatus>>
{

    event Action<TStatus>? GameStatus; //Enviar las actualizaciones de la partida

    event Predicate<Orders> CanContinue;// Espera confirmación de si continuar o no a la siguiente ronda

    IBoard? board { get; }//El tablero

    List<IPlayer>? GamePlayers { get; }//Los jugadores de la partida

    double PlayerScore(IPlayer player);// El Score del jugador en la partida

    IGame<TStatus> Clone();

    TStatus PlayAGame(IBoard board, List<IPlayer> players);//Iniciar la partida

    List<IPlayerScore> PlayerScores();// Score de todos los jugadores de la partida en la interfaz IPlayerScore

    string ToString();

    List<IPlayer> Winner();//Lista de ganadores
}

```

Los anteriores métodos son los métodos los cuales debe recibir la interfaz gráfica.

Este TStatus debe ser devuelto por medio del evento GameStatus y a la finalización del metodo que empieza la partida.

Para comenzar una partida debe invorcarse:

```cs
TStatus PlayAGame(IBoard board, List<IPlayer> players);
```

El cual recibe el tablero a jugar y los jugadores de dicha partida

Se considera que las fichas a repartir debe ser un concepto interno de la partida si es invocada desde un torneo.

Los jugadores deben Implementar la interfaz IPlayer:

```cs

public interface IPlayer : ICloneable<IPlayer>, IEquatable<IPlayer>, IEqualityComparer<IPlayer>, IDescriptible, IEquatable<int>
{
    List<IToken> hand { get; }

    int Id { get; }

    int TotalScore { get; set; }

    IPlayerStrategy strategy { get; }

    List<IPlayerStrategy> strategias { get; }

    void AddHand(List<IToken> Tokens);

    void AddStrategy(IPlayerStrategy strategy);

    IToken BestPlay(IWatchPlayer watchPlayer);

    int ChooseSide(IChooseStrategyWrapped choose, IWatchPlayer watchPlayer);

    IPlayer Clone();

    bool Equals(IPlayer? other);

    bool Equals(IPlayer? x, IPlayer? y);

    bool Equals(int otherId);

    int GetHashCode(IPlayer obj);

    string ToString();
}
```

Un jugador como concepto abstracto debe Tener una o mas strategia/s de uego ademas debe de contener una mano de fichas para evaluar su mejor jugada debe devolver una ficha y dar la posición donde esta debe ponerse para ello se le entrega un IWatchPlayer watchPlayer el cual debe de contener la información de la partida como las reglas y el tablero.

Estrategia de jugadores:

```cs
public interface IPlayerStrategy : IDescriptible
{

    int Evaluate(IToken itoken, List<IToken> hand, IWatchPlayer watch);

    int ChooseSide(IChooseStrategyWrapped choose, IWatchPlayer watch);
}
```

Debe tener interno en su comportamiento analizar una ficha y devolver un resultado en entero(mientras mayor sea mejor) de la evaluacion de dicho token.

Debe seleccionar donde poner el token.

Recibe un token y una lista con la mano del jugador ademas de un contenedor de las reglas del juego y el tablero en su estado actual

El juez de la partida debe ser quien interprete las reglas y evalue si el juego llego a su finalización y el de poner las fichas en el tablero.

```cs
public interface IJudgeGame
{

    bool AddTokenToBoard(IPlayer player, GamePlayerHand<IToken> hand, IToken token, IBoard board, int side);

    bool EndGame(List<(IPlayer, List<IToken>)> players, IBoard board);

    double PlayerScore(IPlayer player);

    List<IPlayerScore> PlayersScores();

    IWatchPlayer RunWatchPlayer(IBoard board);

    IChooseStrategyWrapped ValidPlay(IPlayer player, IBoard board, IToken token);

    List<IPlayer> Winner(List<(IPlayer player, List<IToken> hand)> players);
}
```

Dado que es el conocerdor del conjuto de reglas que se ejecuta en el juego en determinado momento, sus metodos deben de ser invocados por la clase game unicamente y debe ser ella quien solo tenga acceso a estos; sus entidades publicas son el dar a conocer si:
El juego llego a las condiciones de finalización, verdadero en caso que deba terminar.

Si se es valida una acción como poner una ficha en el tablero, para ello entrega una implementación de IChooseStrategyWrapped la cual debe ser entregada al jugador.

Debe entregar un IWatchPlayer para el cual debe de recibir el tablero actual dicha implementación debe ser entregada al jugador para este determinar que jugada debe hacer.

Debe poner las fichas en el tablero en caso de ser posible y devolver true si fue realizado con exito
Necesita conocer el jugador que quiere acomoter dicha accion el tablero en su estado actual , el token que se desea jugar y la dirección del mismo.

Devolverver una lista con una implementacion de IplayerScore donde devuelva el score de estos en la partida

Debe dar a conocer el score en double de un jugador en especifico.

Y auxiliandose de las reglas externas para determinar el/los ganadores

Manager de las fichas:

```cs
public interface ITokensManager
{
    List<IToken> Elements { get; }

    IEqualityComparer<IToken> equalityComparer { get; }

    IComparer<IToken> Comparer { get; }

    List<IToken> GetTokens();

    bool ItsDouble(IToken itoken);
}
```

Contiene la responsabilidad de conocer como se debe repartir las fichas de los jugadores y sobre los criterios de los mismos.

La mano del jugador: dada la posibilidad de una implementación errónea de un jugador o juez puede verse afectado el comportamiento de la mano del jugador por ello es un concepto desacoplado.

```cs
public class GamePlayerHand<TToken>

```

La clase es generica dado que puede ser usada en cualquier tipo de implementación de una ficha incluso de una externa de IToken

Tablero: Un tablero debe de poder brindar la información para conocer la primera y ultima ficha ademas de poder agregar fichas a este y poder ver todas las fichas que contiene.

```cs
public interface IBoard : ICloneable<IBoard>
{
    List<IToken> board { get; }

    IToken First { get; }

    IToken Last { get; }

    void AddTokenToBoard(IToken itoken, int side);

    IBoard Clone();

    IBoard Clone(List<IToken> CopyTokens);

    string ToString();
}

```

Se recomienda que cuando se brinda la propiedad board sea por clonacion de los elementos del mismo.

Las fichas:
Como concepto generico las fichas son cualquier elemento que tenga un comportamiento bajo la interfaz ITokenizable, se conforman por dos Partes.

public interface ITokenizable : IComparable<ITokenizable>, IEquatable<ITokenizable>, IDescriptible
{
string Paint();
double ComponentValue { get; }

}

Para el contenedor Token tener dos elementos con contenido distinto se hace uso de esta interfaz la cual compromete en tener un valor componente del mismo y un modo de impresion de dicho elemento.

El contenedor IToken

```cs
public interface IToken
{
    ITokenizable Part1 { get; }
    ITokenizable Part2 { get; }

    IToken Clone();
    bool ItsDouble();
    void SwapToken();
    string ToString();
}
```

Como contenedor debe ser capaz de:
Brindar dos partes del mismo, conocer si el valor interno del mismo es doble (no significa que sea el valor en las reglas o en el conjunto).

Y poder Intercambiar sus partes .

Generador de fichas

Como el comportamiento de generacion de una ficha es autonomo a esta se necesita de un implementación que brinde esta

```cs
public interface IGenerator : IDescriptible
{

    public List<IToken> CreateTokens(int maxDouble);
}

```

La cual conociendo el maximo doble debe poder crear una lista de tokens bajo sus propios criterios.

Interfaces de Respado

A concepto general desde un torneo hasta una partida debe de usarse una serie de reglas las cuales deben de tener un interpretador (en esta implementador un juez)

Condición de ganada IWinCondition:

```cs
public interface IWinCondition<TCriterio, TToken> : IDescriptible
{

    List<IPlayer> Winner(List<TCriterio> criterios, IGetScore<TToken> howtogetscore);
}
```

Dado un criterio y una forma de tener una puntuacion de otro objeto no necesariamente distinto debe poder devolver una lista de los jugadores que han ganado cierto criterio (una partida o un torneo).

Condicion de validez

Debe de poder brindar el concepto a lo que se le llame juego y un tipo de jugador y poder determinar un criterio dado de la validez del mismo.

````cs
public interface IValidPlay<TGame, TPlayer, TCriterio> : IDescriptible
{
    TCriterio ValidPlay(TGame game, TPlayer player);


}
```
 Condicion de parada
Debe poder dictaminar si la partida o torneo ha llegado a una condicion de finalización
```cs
public interface IStopGame<TCriterio, TToken> : IDescriptible
{
    bool MeetsCriteria(TCriterio criterio, IGetScore<TToken> howtogetscore);
}
```

 Puntuacion
 Dado un criterio sea una ficha o una jugador debe poder devolver un double el cual sea la puntuación de este
```cs
public interface IGetScore<TToken> : IDescriptible
{
    double Score(TToken item);
}

```

Las implementaciones de las reglas deben ser agregadas a un juez el cual sea el intermediario entre el concepto o nivel del juego de dictaminar.






````
