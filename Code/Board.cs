namespace Game;

public class Board : IBoard
{
    public List<Token> board { get; protected set; }

    public Token First
    {
        get
        {
            if (board.Count == 0) return null!;
            return this.board.First();
        }
    }

    public Token Last
    {
        get
        {
            if (board.Count == 0) throw new NullReferenceException("The token can´t be null");
            return this.board.Last();
        }
    }

    public Board()
    {
        this.board = new List<Token>() { };
    }

    public override string ToString()
    {
        string a = "\n Board:  \n";

        foreach (var item in this.board)
        {
            a += item.ToString();
        }

        return a;
    }


    public void AddTokenToBoard(Token token, int side)
    {
        if (side == 0) { this.board.Insert(0, token); }
        else { this.board.Add(token); }
    }

    public IBoard Clone()
    {
        return new Board();
    }
    public IBoard Clone(List<Token> CopyTokens)
    {
        this.board = CopyTokens;
        return this;
    }

}




