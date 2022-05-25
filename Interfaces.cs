namespace Juego
{
  public interface IEstrategia{
      int Evaluate(Ficha ficha, Game game);
  }

  public interface IBoard{
    List<Ficha> board {get;set;}
    void AddFichaToGame(Ficha ficha, int side);
    string ToString();
  }

  public interface IPlayer{
    
  }
  

  
}