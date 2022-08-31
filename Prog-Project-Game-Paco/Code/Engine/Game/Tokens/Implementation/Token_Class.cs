namespace Game;

#region  TokenClass

public class Token : IToken //ficha
{
    public virtual ITokenizable Part1 { protected set; get; }
    public virtual ITokenizable Part2 { protected set; get; }



    public Token(ITokenizable Part1, ITokenizable Part2)
    {
        this.Part1 = Part1;
        this.Part2 = Part2;
    }


    public override string ToString()
    {
        return "[" + Part1 + "|" + Part2 + "] ";
    }

    public virtual void SwapToken()
    {
        var temp = this.Part1;
        this.Part1 = this.Part2;
        this.Part2 = temp;
    }

    public virtual IToken Clone()
    {
        return new Token(this.Part1, this.Part2);
    }
}
#endregion










