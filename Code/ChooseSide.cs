namespace Game;


public class ChooseStrategyWrapped : IEquatable<ChooseStrategyWrapped>
{
    public bool CanMatch { get; private set; } = false;

    public bool FirstPlay { get; private set; } = false;
    public Board board { get; private set; }
    public IToken itoken { get; private set; }
    public List<ChooseSideWrapped> side { get; private set; }

    public ChooseStrategyWrapped(Board board, IToken itoken, bool FirstPlay = false)
    {
        this.FirstPlay = FirstPlay;
        this.CanMatch = FirstPlay;
        this.board = board;
        this.side = new List<ChooseSideWrapped>() { };
        this.itoken = itoken;

    }

    /*  private void Run()
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
    {  // SOn iguales si tienen el mismo itoken
        if (other == null) { return false; }
        return (this.itoken.Equals(other.itoken));
    }
}

public sealed class ChooseSideWrapped
{
    public int index { get; private set; }
    public bool canChoose { get; private set; } = false;

    public List<int> WhereCanMacht { get; private set; }

    internal ChooseSideWrapped(int index)
    {
        this.index = index;
        this.WhereCanMacht = new List<int>() { };

    }

    public void AddSide(int i)
    {
        WhereCanMacht.Add(i);
    }
    public void Run()//Verifica si se puede o no 
    {
        if (WhereCanMacht.Count > 0) { canChoose = true; }
    }
}
