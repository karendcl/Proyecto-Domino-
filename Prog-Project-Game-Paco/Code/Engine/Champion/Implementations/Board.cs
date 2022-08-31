namespace Game;

public class Board : IBoard
{
    public List<IToken> board { get { return GetTokens(); } }

    protected List<IToken> temp { get; set; }

    protected List<IToken> GetTokens()
    {
        List<IToken> list = new List<IToken>();
        foreach (var item in temp)
        {
            list.Add(item.Clone());
        }
        return list;
    }
    public IToken First  //devuelve la primera ficha del tablero
    {
        get
        {
            if (temp.Count == 0) return null!;
            return this.temp.First().Clone();
        }
    }

    public IToken Last  //devuelve la ultima ficha del tablero
    {
        get
        {
            if (temp.Count == 0) throw new NullReferenceException("The itoken can´t be null");
            return this.temp.Last().Clone();
        }
    }

    public Board()
    {
        this.temp = new List<IToken>() { };
    }

    public override string ToString()  //escribe el tablero y las fichas
    {
        string a = " Board:  \n";

        foreach (var item in this.temp)
        {
            a += item.ToString();
        }

        return a;
    }

    /// <summary>
    ///  0 para poner en el frente del tablero 1 para el final
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    public void AddTokenToBoard(IToken itoken, int side) //add una ficha al tablero
    {
        if (temp.Count < 1)
        {
            temp.Add(itoken);
        }
        else
        {
            if (side == 0) { this.temp.Insert(0, itoken); }
            else { this.temp.Add(itoken); }
        }

    }

    public IBoard Clone()  //clona el tablero
    {
        return new Board();
    }
    public IBoard Clone(List<IToken> CopyTokens)
    {
        this.temp = CopyTokens;
        return this;
    }

}




