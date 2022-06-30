namespace Game;

public class ChooseStrategyWrapped : IEquatable<ChooseStrategyWrapped>
{
    public bool CanMatch { get; protected set; } = false;

    public bool FirstPlay { get; protected set; } = false;
    public IBoard board { get; protected set; }
    public Token token { get; protected set; }
    public List<ChooseSideWrapped> side { get; protected set; }

    public ChooseStrategyWrapped(IBoard board, Token token, bool FirstPlay = false)
    {
        this.FirstPlay = FirstPlay;
        this.CanMatch = FirstPlay;
        this.board = board;
        this.side = new List<ChooseSideWrapped>() { };
        this.token = token;

    }

    /*  protected void Run()
      {
          if (board.board.Count < 1) { CanMatch = true; FirstPlay = true; }
      }*/

    public (bool, ChooseSideWrapped) ControlSide(int side)
    {
        if (side >= this.side.Count) { return (false, null)!; }
        ChooseSideWrapped temp = this.side[side];
        if (!temp.canChoose)
        {
            for (int i = 0; i < this.side.Count; i++)
            {
                if (this.side[i].canChoose) { temp = this.side[i]; }
            }
        }
        return (temp.canChoose, temp);
    }


    public void AddSide(ChooseSideWrapped side)
    {
        this.side.Add(side);
        if (side.canChoose)
        {
            CanMatch = true;
        }
    }

    public bool Equals(ChooseStrategyWrapped? other)
    {  // SOn iguales si tienen el mismo token
        if (other == null) { return false; }
        return (this.token.Equals(other.token));
    }
}

public sealed class ChooseSideWrapped
{
    public int index { get; protected set; }
    public bool canChoose { get; protected set; } = false;

    public bool DontSwap { get; protected set; } = false;

    public List<(int, int)> WhereCanMacht { get; protected set; }

    internal ChooseSideWrapped(int index)
    {
        this.index = index;
        this.WhereCanMacht = new List<(int, int)>() { };

    }
    public void DontNeedSwap() => this.DontSwap = true;
    public void AddSide(int indexToken, int indexBoardToken)
    {

        this.WhereCanMacht.Add((indexToken, indexBoardToken));
    }

    public void AddSide(List<(int, int)> sides)
    {
        this.WhereCanMacht.AddRange(sides);
    }

    public void Run()//Verifica si se puede o no 
    {
        if (WhereCanMacht.Count > 0 || DontSwap) { canChoose = true; }
    }
}
