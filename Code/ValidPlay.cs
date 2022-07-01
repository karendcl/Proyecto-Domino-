
namespace Game;
#region  Games

#region  Definiton AbstractClass


public abstract class ValidPlayClass : IValidPlay<Board, Token, ChooseStrategyWrapped>
{         //Devuelve el valor del numero donde se puede machear en RN
    public virtual IEqualityComparer<Token> equalityComparer { get; protected set; }
    public virtual IComparer<Token> Comparer { get; protected set; }

    public virtual string Description => "Game Valid Play";

    public ValidPlayClass(IEqualityComparer<Token> equalityComparer, IComparer<Token> Comparer)
    {
        this.Comparer = Comparer;
        this.equalityComparer = equalityComparer;
    }
    public virtual ChooseStrategyWrapped ValidPlay(Board board, Token token)
    {

        ChooseStrategyWrapped choose = new ChooseStrategyWrapped(board, token);

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
    protected virtual ChooseSideWrapped ValidPlayFront(Board board, Token token)
    {


        ChooseSideWrapped choose = new ChooseSideWrapped(0);
        List<int> macth = new List<int>() { };

        if (Match(token, board.First))
        {
            ChooseWhereToPlay(token, board.First, choose);
        }

        choose.Run();
        return choose;
    }

    protected abstract bool Match(Token x, Token y);

    protected virtual bool FirstPlay(Board board)
    {
        if (board.board == null || board.board.Count == 0) return true;
        return false;
    }

    protected virtual ChooseSideWrapped ValidPlayBack(Board board, Token token)
    {
        ChooseSideWrapped choose = new ChooseSideWrapped(1);

        if (Match(token, board.Last))
        {
            ChooseWhereToPlay(board.Last, token, choose);
        }
        choose.Run();
        return choose;
    }

    // colocar en el orden a poner el token
    protected virtual void ChooseWhereToPlay(Token first, Token last, ChooseSideWrapped choose)
    {
        if (first.Last.Equals(last.First))
        { choose.DontNeedSwap(); return; }

        choose.AddSide(Intercept(first, last));


    }


    protected virtual List<(int, int)> Intercept(Token first, Token last)
    {
        List<(int, int)> temp = new List<(int, int)>() { };
        if (first.First.Equals(last.Last)) { temp.Add((-1, -1)); return temp; }
        temp.Add((0, 1));

        return temp;
    }



}
#endregion

# region Derivates class
public class ClassicValidPlay : ValidPlayClass
{
    public override string Description => "Classic Game Valid Play";
    public ClassicValidPlay(IEqualityComparer<Token> equalityComparer, IComparer<Token> Comparer) : base(equalityComparer, Comparer)
    {
    }

    protected override bool Match(Token x, Token y)
    {

        foreach (var xComponent in x.Component)
        {
            if (y.Component[0].Equals(xComponent))
            { return true; }
        }
        return false;
    }



}

public class BiggerValidPlay : ValidPlayClass
{
    public override string Description => "Bigger Game Valid Play";
    public BiggerValidPlay(IEqualityComparer<Token> equalityComparer, IComparer<Token> Comparer) : base(equalityComparer, Comparer)
    {
    }

    protected override bool Match(Token token, Token board)
    {
        double xNorma = 0;
        foreach (var itemx in token.Component)
        {
            xNorma += Math.Pow(itemx.ComponentValue, 2);
        }
        xNorma = Math.Sqrt(xNorma);
        double yNorma = 0;

        foreach (var itemy in board.Component)
        {
            yNorma += Math.Pow(itemy.ComponentValue, 2);
        }
        yNorma = Math.Sqrt(yNorma);
        return (yNorma < yNorma);
    }
}

public class SmallerValidPlay : ValidPlayClass
{
    public override string Description => "Smaller Game Valid Play";
    public SmallerValidPlay(IEqualityComparer<Token> equalityComparer, IComparer<Token> Comparer) : base(equalityComparer, Comparer)
    {
    }

    protected override bool Match(Token token, Token board)
    {
        double xNorma = 0;
        foreach (var itemx in token.Component)
        {
            xNorma += Math.Pow(itemx.ComponentValue, 2);
        }
        xNorma = Math.Sqrt(xNorma);
        double yNorma = 0;

        foreach (var itemy in board.Component)
        {
            yNorma += Math.Pow(itemy.ComponentValue, 2);
        }
        yNorma = Math.Sqrt(yNorma);
        return (yNorma > yNorma);
    }
}

#endregion

#endregion

#region Champion
// Si ha perdido mas de las mitad 
public class ValidChampion : IValidPlay<List<GameStatus>, Player, bool>
{
    public double Porcent { get; protected set; }

    public string Description => "Champion Valid Play";

    public ValidChampion(double Porcent)
    {
        this.Porcent = Porcent;
    }
    public bool ValidPlay(List<GameStatus> gamesStatus, Player player)
    {
        double cant = gamesStatus.Count * Porcent;
        int count = 0;
        foreach (var game in gamesStatus)
        {
            int x = game.winners.IndexOf(player);
            if (x < game.winners.Count / 2) count++;
            if (count < cant) return false;
        }

        return true;

    }
}



public class ValidChampionPerdidasConsecutivas : IValidPlay<List<GameStatus>, Player, bool>
{
    public int CantdeVecesConsecutivas { get; protected set; }

    public string Description => "Champion Valid Play";

    public ValidChampionPerdidasConsecutivas(int cantVeces)
    {

        this.CantdeVecesConsecutivas = cantVeces;

    }

    public bool ValidPlay(List<GameStatus> game, Player player)
    {
        if (CantdeVecesConsecutivas < 1) { return true; }
        int count = 0; int lastIndex = 0;
        for (int i = 0; i < game.Count; i++)
        {
            var status = game[i];
            status.winners.Contains(player);
            if (lastIndex == 0 || i - lastIndex == 1)
            {
                count++;
            }
            else
            {
                lastIndex = 0;
            }
            if (lastIndex > this.CantdeVecesConsecutivas) return false;

        }

        return true;
    }
}




#endregion