namespace Game;


public interface IWinCondition<TCriterio, TToken>
{                           //List<IPlayer> players
    List<IPlayer> Winner(List<TCriterio> criterios, IGetScore<TToken> howtogetscore);
}

public interface IPlayerStrategy
{
    int Evaluate(Token token, List<Token> hand, WatchPlayer watch);
    int ChooseSide(ChooseStrategyWrapped choose, WatchPlayer watch);
}

public interface IGetScore<TToken>
{   //Puede ser un token o un juego
    int Score(TToken item);//hacer uno en torneo que sume todo lo de los metodos
}
public interface IBoard : ICloneable<IBoard>, ICloneable<IBoard, List<Token>>
{
    List<Token> board { get; }
    void AddTokenToBoard(Token Token, int side);
    Token First();
    Token Last();
}

public interface IStopGame<TCriterio, TToken>
{                    // Puede ser un jugador 
                     // Puede ser un Juego para parar el torneo
    bool MeetsCriteria(TCriterio criterio, IGetScore<TToken> howtogetscore);
}

public interface IPlayer : IPlayerStrategy, ICloneable<IPlayer>, IEquatable<IPlayer>
{
    public List<Token> hand { get; set; }
    public int Id { get; set; }
    IPlayerStrategy strategy { get; set; }

    //quitar estos y poner un Play para poner un jugador humano 
    public Token BestPlay(WatchPlayer watch);
    public int TotalScore { get; set; }

}

public interface IRules
{
    bool SwitchDirection { get; set; }
    bool DrawToken { get; set; }
    int MaxDouble { get; set; }
    int Players { get; set; }
    int TokensForEach { get; set; }
}


public interface IValidPlay<TGame, TPlayer, TCriterio>
{
    TCriterio ValidPlay(TGame game, TPlayer player);

}


//Los criterios pueden darse en base a jugador o juego 
public interface IJudge<TCriterio, TToken, TWrapped>
{
    IStopGame<TCriterio, TToken> stopcriteria { get; set; }
    IWinCondition<TCriterio, TToken> winCondition { get; set; }

    IValidPlay<TCriterio, TToken, TWrapped> valid { get; set; }
    public IGetScore<Token> howtogetscore { get; set; }

    bool ValidPlay(TCriterio criterio, TToken token);
    bool EndGame(Game game);

    int PlayerScore(IPlayer player);

    bool ValidSettings(int TokensForEach, int MaxDoble, int players);

    void AddTokenTCriterio(Token token, IBoard board, int side);
    //Añadir al board
    //Añadir a un partido un juegador
}


public interface ITokenizable
{
    public string Description { get; set; }


    public double ComponentValue { get; }
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
