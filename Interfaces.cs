namespace Juego
{
  public interface IBoard{
    List<Ficha> board {get;set;}
    void AddFichaToBoard(Ficha ficha, int side);
    string ToString();
  }

  public interface IPlayer {
    public List<Ficha> hand {get; set;}
    public int Id{get; set;}

    public Ficha BestPlay(Game game);
  }

  public interface IRules 
{
    bool SwitchDirection{get; set;}
    int MaxDouble{get; set;}
    int Players{get; set;}
    int FichasForEach{get; set;}
    public bool EndGame();

}
  

  
}