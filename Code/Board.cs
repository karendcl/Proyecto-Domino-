namespace Game;

public class Board : IBoard
{
    public List<Token> board { get; set; }

    public Board(List<Token> a)
    {
        this.board = a;
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

    public void AddTokenToBoard(Token Token, int side)
    {

        if (this.board.Count == 0)
        {
            board.Insert(0, Token);
            return;
        }

        Token first = board.First();
        Token last = board.Last();

        if (side != -1)
        {
            if (side == 0)
            {
                PlayAlante(Token, first);
                return;
            }

            if (side == 1)
            {
                if (Token.Contains(last.Part2))
                    PlayAtras(Token, last);
                return;
            }
        }


        if (Token.Contains(first.Part1))
        {
            PlayAlante(Token, first);
            return;
        }

        if (Token.Contains(last.Part2))
        {
            PlayAtras(Token, last);
            return;
        }

    }

    public void PlayAlante(Token Token, Token first)
    {
        if (first.Part1 == Token.Part1)
        {
            Token.SwapToken();
            board.Insert(0, Token);

            return;
        }

        board.Insert(0, Token);

        return;
    }

    public void PlayAtras(Token Token, Token last)
    {
        if (Token.Part2 == last.Part2)
        {
            Token.SwapToken();
            board.Add(Token);
            return;
        }

        board.Add(Token);
        return;
    }

    public Token First()
    {
        if (board.Count == 0) return null!;
        return this.board.First();
    }

    public Token Last()
    {
        if (board.Count == 0) throw new NullReferenceException("The token can´t be null");
        return this.board.Last();
    }

    public IBoard Clone()
    {
        return new Board(new List<Token>() { });
    }
}

