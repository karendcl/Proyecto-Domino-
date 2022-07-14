namespace Game;

public class Board
{
    public List<IToken> board { get; protected set; }

    public IToken First  //devuelve la primera ficha del tablero
    {
        get
        {
            if (board.Count == 0) return null!;
            return this.board.First();
        }
    }

    public IToken Last  //devuelve la ultima ficha del tablero
    {
        get
        {
            if (board.Count == 0) throw new NullReferenceException("The itoken can´t be null");
            return this.board.Last();
        }
    }

    public Board()
    {
        this.board = new List<IToken>() { };
    }

    public override string ToString()  //escribe el tablero y las fichas
    {
        string a = " Board:  \n";

        foreach (var item in this.board)
        {
            a += item.ToString();
        }

        return a;
    }


    public void AddTokenToBoard(IToken itoken, int side) //add una ficha al tablero
    {
        if (side == 0) { this.board.Insert(0, itoken); }
        else { this.board.Add(itoken); }
    }

    public Board Clone()  //clona el tablero
    {
        return new Board();
    }
    public Board Clone(List<IToken> CopyTokens)
    {
        this.board = CopyTokens;
        return this;
    }

}




