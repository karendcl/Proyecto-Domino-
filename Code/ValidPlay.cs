
namespace Game;

public abstract class ValidPlayClass : IValidPlay
{         //Devuelve el valor del numero donde se puede machear en RN
    public virtual bool ValidPlay(IBoard board, Token token)
    {
        if (FirstPlay(board)) return true;
        if (ValidPlayFront(board, token)) return true;
        if (ValidPlayBack(board, token)) return true;
        return false;

    }
    public virtual bool ValidPlayFront(IBoard board, Token token)
    {
        if (FirstPlay(board)) return true;
        return (Match(token.Part1, board.First().Part1) ||
                Match(token.Part2, board.First().Part1));
    }

    public abstract bool Match(int Part1, int part2);

    public virtual bool FirstPlay(IBoard board)
    {
        if (board.board == null || board.board.Count == 0) return true;
        return false;
    }

    public virtual bool ValidPlayBack(IBoard board, Token token)
    {
        if (FirstPlay(board)) return true;
        return (Match(token.Part1, board.Last().Part2) ||
                Match(token.Part2, board.Last().Part2));
    }
}

public class ClassicValidPlay : ValidPlayClass
{



    public override bool ValidPlayFront(IBoard board, Token token)
    {
        if (FirstPlay(board)) return true;
        return (token.Contains(board.First().Part1));
    }

    public override bool ValidPlayBack(IBoard board, Token token)
    {
        if (FirstPlay(board)) return true;
        return (token.Contains(board.Last().Part2));
    }

    public override bool Match(int Part1, int part2)
    {
        return (part2.Equals(Part1));
    }
}

public class BiggerValidPlay : ValidPlayClass
{
    public override bool Match(int token, int board)
    {
        return (board < token);
    }
}

public class SmallerValidPlay : ValidPlayClass
{
    public override bool ValidPlay(IBoard board, Token token)
    {
        if (FirstPlay(board)) return true;
        if (board.board.Count == 0) return true;
        if (ValidPlayFront(board, token)) return true;
        if (ValidPlayBack(board, token)) return true;
        return false;

    }

    public override bool Match(int Part1, int part2)
    {
        return (part2 > Part1);
    }
}


#region Champion

public class ValidChampion : IValidPlayChampion<Game, IPlayer>
{
    public bool ValidPlay(Game game, IPlayer players)
    {
        foreach (var player in game.player)
        {
            if (player.hand.Count > 0) return true;
        }
        return false;
    }
}


#endregion