namespace Game;

public class ChooseStrategyWrapped : IEquatable<ChooseStrategyWrapped>
{
    public bool CanMatch { get; private set; } = false;

    public bool FirstPlay { get; private set; } = false;
    public IBoard board { get; private set; }
    public Token token { get; private set; }
    public List<ChooseSideWrapped> side { get; private set; }

    public ChooseStrategyWrapped(IBoard board, Token token, bool FirstPlay = false)
    {
        this.FirstPlay = FirstPlay;
        this.CanMatch = FirstPlay;
        this.board = board;
        this.side = new List<ChooseSideWrapped>() { };
        this.token = token;

    }

    /*  private void Run()
      {
          if (board.board.Count < 1) { CanMatch = true; FirstPlay = true; }
      }*/

    public (bool, ChooseSideWrapped) ControlSide(int side)
    {
        if (side >= this.side.Count) { return (false, null)!; }
        ChooseSideWrapped temp = this.side[side];
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
    public bool canChoose { get; private set; } = false;

    public List<int> WhereCanMacht { get; private set; }

    internal ChooseSideWrapped()
    {

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
