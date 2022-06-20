
namespace Game;
#region  Games

#region  Definiton AbstractClass


public abstract class ValidPlayClass : IValidPlay<IBoard, Token, ChooseStrategyWrapped>
{         //Devuelve el valor del numero donde se puede machear en RN
    public virtual ChooseStrategyWrapped ValidPlay(IBoard board, Token token)
    {
        // List<(bool, List<int>)> LugarPorMachear = new List<(bool, List<int>)>() { (false, new List<int>() { }), (false, new List<int>() { }) };
        ChooseStrategyWrapped choose = new ChooseStrategyWrapped(board, token);
        bool z = false;
        if (FirstPlay(board))
        {
            return (new ChooseStrategyWrapped(board, token, true));
        }
        ChooseSideWrapped front = ValidPlayFront(board, token);
        choose.AddSide(front);
        ChooseSideWrapped back = ValidPlayBack(board, token);
        choose.AddSide(back);
        return choose;

    }
    protected virtual ChooseSideWrapped ValidPlayFront(IBoard board, Token token)
    {


        ChooseSideWrapped choose = new ChooseSideWrapped();
        List<int> macth = new List<int>() { };
        //if (FirstPlay(board)) return true;
        if (Match(token.Part1, board.First().Part1))
        {
            choose.AddSide(0);
        }
        if (Match(token.Part2, board.First().Part1))
        {
            choose.AddSide(1);
        }
        choose.Run();
        return choose;
    }

    protected abstract bool Match(int Part1, int part2);

    protected virtual bool FirstPlay(IBoard board)
    {
        if (board.board == null || board.board.Count == 0) return true;
        return false;
    }

    protected virtual ChooseSideWrapped ValidPlayBack(IBoard board, Token token)
    {
        ChooseSideWrapped choose = new ChooseSideWrapped();

        // if (FirstPlay(board)) return true;
        if (Match(token.Part1, board.Last().Part2))
        {
            choose.AddSide(0);
        }

        if (Match(token.Part2, board.Last().Part2))
        {
            choose.AddSide(1);
        }
        choose.Run();
        return choose;
    }



}
#endregion

# region Derivates class
public class ClassicValidPlay : ValidPlayClass
{

    protected override bool Match(int Part1, int part2)
    {
        return (part2.Equals(Part1));
    }



}

public class BiggerValidPlay : ValidPlayClass
{
    protected override bool Match(int token, int board)
    {
        return (board < token);
    }
}

public class SmallerValidPlay : ValidPlayClass
{
    protected override bool Match(int Part1, int part2)
    {
        return (part2 > Part1);
    }
}

#endregion

#endregion

#region Champion

public class ValidChampion : IValidPlay<Game, IPlayer, List<IPlayer>>
{
    public List<IPlayer> ValidPlay(Game game, IPlayer players)
    {
        List<IPlayer> playersDescalificados = new List<IPlayer>() { };
        foreach (var player in game.player)
        {
            if (player.hand.Count > 0) { playersDescalificados.Add(player); }
        }
        return playersDescalificados;
    }
}


#endregion