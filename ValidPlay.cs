
namespace Juego;
public class ClassicValidPlay : IValidPlay
{
    public ClassicValidPlay()
    {
    }
    public bool ValidPlay(IBoard board, Token token)
    {
        if (board.board.Count == 0) return true;
        if (ValidPlayFront(board, token)) return true;
        if (ValidPlayBack(board, token)) return true;
        return false;
    }
    public bool FirstPlay(IBoard board)
    {
        if (board.board == null||board.board.Count==0) return true;
        return false;
    }
    public bool ValidPlayFront(IBoard board, Token token)
    {
        if (FirstPlay(board)) return true;
        return (token.Contains(board.First().Part1));
    }

    public bool ValidPlayBack(IBoard board, Token token)
    {
        if (FirstPlay(board)) return true;
        return (token.Contains(board.Last().Part2));
    }

    public bool Match(int Part1, int part2)
    {
        return (part2.Equals(Part1));
    }
}

public class SmallerValidPlay : IValidPlay
{
    public bool ValidPlay(IBoard board, Token token)
    {
        if (board.board.Count == 0) return true;
        if (ValidPlayFront(board, token)) return true;
        if (ValidPlayBack(board, token)) return true;
        return false;

    }

    public bool ValidPlayFront(IBoard board, Token token)
    {    if (FirstPlay(board)) return true;
        return (token.Part1 < board.First().Part1 ||
                token.Part2 < board.First().Part1);
    }
    public bool FirstPlay(IBoard board)
    {
        if (board.board == null||board.board.Count==0) return true;
        return false;
    }
    public bool ValidPlayBack(IBoard board, Token token)
    {     
        if (FirstPlay(board)) return true;
        return (token.Part1 < board.Last().Part2 ||
                token.Part2 < board.Last().Part2);
    }

    public bool Match(int Part1, int part2)
    {
        return (part2 < Part1);
    }
}

public class BiggerValidPlay : IValidPlay
{
    public bool ValidPlay(IBoard board, Token token)
    {       if (FirstPlay(board)) return true;
        if (board.board.Count == 0) return true;
        if (ValidPlayFront(board, token)) return true;
        if (ValidPlayBack(board, token)) return true;
        return false;

    }

    public bool ValidPlayFront(IBoard board, Token token)
    {      if (FirstPlay(board)) return true;
        return (token.Part1 > board.First().Part1 ||
                token.Part2 > board.First().Part1);
    }
   public bool FirstPlay(IBoard board)
    {    
        if (board.board == null||board.board.Count==0) return true;
        return false;
    }
    public bool ValidPlayBack(IBoard board, Token token)
    {    if (FirstPlay(board)) return true;
        return (token.Part1 > board.Last().Part2 ||
                 token.Part2 > board.Last().Part2);
    }

    public bool Match(int Part1, int part2)
    {
        return (part2 > Part1);
    }
}
