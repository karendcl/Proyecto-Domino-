using System.Collections;

namespace Game;


public interface IWinCondition<TCriterio, TToken> : IDescriptible
{                           //List<IPlayer> players
    List<Player> Winner(List<TCriterio> criterios, IGetScore<TToken> howtogetscore);
}

public interface IDescriptible
{
    string Description { get; }
}

public interface IPlayerStrategy : IDescriptible
{
    int Evaluate(Token token, List<Token> hand, WatchPlayer watch);
    int ChooseSide(ChooseStrategyWrapped choose, WatchPlayer watch);
}

public interface IGetScore<TToken> : IDescriptible
{   //Puede ser un token o un juego
    int Score(TToken item);//hacer uno en torneo que sume todo lo de los metodos
}


public interface IStopGame<TCriterio, TToken>
{                    // Puede ser un jugador 
                     // Puede ser un Juego para parar el torneo
    bool MeetsCriteria(TCriterio criterio, IGetScore<TToken> howtogetscore);
}

public interface ITokenizable : IComparable<ITokenizable>, IEquatable<ITokenizable>, IDescriptible
{
    string Paint();

    double ComponentValue { get; }

}


public interface ITokenManager
{
    protected int TokensForEach { get; set; }
    public List<Token> Elements { get; protected set; }

    public GamePlayerHand<Token> AssignTokens(Player player);
    public IComparer<Token> Comparer { get; }
}



public interface IValidPlay<TGame, TPlayer, TCriterio> : IDescriptible
{
    TCriterio ValidPlay(TGame game, TPlayer player);

}


//Los criterios pueden darse en base a jugador o juego 
public interface IJudge<TCriterio, TToken, TWrapped> : IDescriptible
{
    IStopGame<TCriterio, TToken> stopcriteria { get; set; }
    IWinCondition<TCriterio, TToken> winCondition { get; set; }

    IValidPlay<TCriterio, TToken, TWrapped> valid { get; set; }
    public IGetScore<Token> howtogetscore { get; set; }

    bool ValidPlay(TCriterio criterio, TToken token);
    bool EndGame(Game game);

    int PlayerScore(Player player);

    bool ValidSettings(int TokensForEach, int MaxDoble, int players);

    void AddTokenTCriterio(Token token, Board board, int side);
    //Añadir al board
    //Añadir a un partido un juegador
}


public interface ITokenizable<T> where T : IEnumerable<T>, IEquatable<T>, IDescriptible
{
    public string Description { get; }

    public List<T> Components { get; set; }


}


public interface ICloneable<T> : ICloneable
{
    new T Clone();
    Object ICloneable.Clone() => Clone()!;
}

public interface ICloneable<T1, T2> : ICloneable<T1>
{
    T1 Clone(T2 item);

}
