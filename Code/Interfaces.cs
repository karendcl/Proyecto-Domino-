namespace Juego
{

 public interface IWinCondition
 {
   List<IPlayer> Winner(List<IPlayer> players, IJudge judge);
 }

 public interface IPlayerStrategy {
   int Evaluate(Token token, List<Token> hand,Game game);
   int ChooseSide(Game game);
 }
 
   public interface IGetScore{
    int Score(Token token);
  }
  public interface IBoard{
    List<Token> board {get;set;}
    void AddTokenToBoard(Token Token, int side);
    string ToString();
    Token First();
    Token Last();
  }

  public interface IStopGame{
    bool MeetsCriteria(IPlayer player, IGetScore howtogetscore);
  }

  public interface IPlayer {
    public List<Token> hand {get; set;}
    public int Id{get; set;}
    public Token BestPlay(Game game);

    public int TotalScore{get;set;}
    public int ChooseSide(Game game);
  }

  public interface IRules 
{
    bool SwitchDirection{get; set;}
    bool DrawToken{get;set;}
    int MaxDouble{get; set;}
    int Players{get; set;}
    int TokensForEach{get; set;}
}
  
  public interface IValidPlay{
     bool FirstPlay(IBoard board);
    
    bool ValidPlay(IBoard board, Token token);
    bool ValidPlayFront(IBoard board, Token token);
    bool ValidPlayBack(IBoard board, Token token);
    bool Match (int Part1, int part2);

    bool FirstPlay(IBoard board);
  }

  public interface IJudge
  {
    bool ValidPlay(IBoard board, Token token);
    bool EndGame(Game game);
    bool ValidSettings(int TokensForEach, int MaxDoble, int players);
    IStopGame stopcriteria{get; set;}
    IWinCondition winCondition {get;set;}

    IValidPlay valid{get;set;}
   int PlayerScore(IPlayer player);
   public IGetScore howtogetscore {get;set;}

   bool PlayAmbigua(Token token, IBoard board);

   void AddTokenToBoard(Token token, IBoard board, int side);
    
  }
}