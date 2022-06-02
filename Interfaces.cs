namespace Juego
{
  public interface IBoard{
    List<Token> board {get;set;}
    void AddTokenToBoard(Token Token, int side);
    string ToString();
  }

  public interface IPlayer {
    public List<Token> hand {get; set;}
    public int Id{get; set;}

    public Token BestPlay(Game game);
  }

  public interface IRules 
{
    bool SwitchDirection{get; set;}
    int MaxDouble{get; set;}
    int Players{get; set;}
    int TokensForEach{get; set;}
}
  

  public interface IJudge
  {
    bool ValidPlay(Board board, Token token);
    bool EndGame(Game game);
    bool ValidSettings(int TokensForEach, int MaxDoble, int players);
    List<Player> Winner(Game game);
  }

  
}