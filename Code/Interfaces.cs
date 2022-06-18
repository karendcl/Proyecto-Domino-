namespace Game;


public interface IWinCondition<TCriterio, TToken>
{                           //List<IPlayer> players
    List<IPlayer> Winner(List<TCriterio> criterios, IJudge<TCriterio, TToken> judge);
}

public interface IPlayerStrategy
{
    int Evaluate(Token token, List<Token> hand, Game game);
    int ChooseSide(Game game);
}

public interface IGetScore<TToken>
{   //Puede ser un token o un juego
    int Score(TToken item);//hacer uno en torneo que sume todo lo de los metodos
}
public interface IBoard : ICloneable<IBoard>
{
    List<Token> board { get; set; }
    void AddTokenToBoard(Token Token, int side);
    Token First();
    Token Last();
}

public interface IStopGame<TCriterio, TToken>
{                    // Puede ser un jugador 
                     // Puede ser un Juego para parar el torneo
    bool MeetsCriteria(TCriterio criterio, IGetScore<TToken> howtogetscore);
}

public interface IPlayer : ICloneable<IPlayer>, IEquatable<IPlayer>
{
    public List<Token> hand { get; set; }
    public int Id { get; set; }
    IPlayerStrategy strategy { get; set; }

    //quitar estos y poner un Play para poner un jugador humano 
    public Token BestPlay(Game game);
    public int TotalScore { get; set; }
    public int ChooseSide(Game game);
}

public interface IRules
{
    bool SwitchDirection { get; set; }
    bool DrawToken { get; set; }
    int MaxDouble { get; set; }
    int Players { get; set; }
    int TokensForEach { get; set; }
}

public interface IValidPlayChampion<TGame, TPlayer>
{
    bool ValidPlay(TGame game, TPlayer player);
}

public interface IValidPlay : IValidPlayChampion<IBoard, Token>
{
    //Devuelve el lugar donde el vector de Rn es compatible por la cara y el int -1 izquierda 0 por cualquir lado 
    bool FirstPlay(IBoard board);
    bool ValidPlayFront(IBoard board, Token token);
    bool ValidPlayBack(IBoard board, Token token);
    bool Match(int Part1, int part2);
}
//Los criterios pueden darse en base a jugador o juego 
public interface IJudgeChampion<TCriterio, TToken>
{
    IStopGame<TCriterio, TToken> stopcriteria { get; set; }
    IWinCondition<TCriterio, TToken> winCondition { get; set; }
    IValidPlay valid { get; set; }
    public IGetScore<Token> howtogetscore { get; set; }

    bool ValidPlay(IBoard board, Token token);
    bool EndGame(Game game);
}
public interface IJudge<TCriterio, TToken> : IJudgeChampion<TCriterio, TToken>//Agregar donde Es un IstopGame....
{

    bool ValidSettings(int TokensForEach, int MaxDoble, int players);
    int PlayerScore(IPlayer player);
    bool PlayAmbigua(Token token, IBoard board);
    void AddTokenToBoard(Token token, IBoard board, int side);

}



public interface ICloneable<T> : ICloneable
{
    new T Clone();
    Object ICloneable.Clone() => Clone()!;
}
