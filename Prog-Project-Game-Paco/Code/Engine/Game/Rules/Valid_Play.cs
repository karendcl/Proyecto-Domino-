
namespace Game;
#region  Games

#region  Definiton AbstractClass

public abstract class ValidPlayClass : IValidPlay<IBoard, IToken, IChooseStrategyWrapped>
{

    public static string Description => "Valid Play";

    public virtual IChooseStrategyWrapped ValidPlay(IBoard board, IToken token)
    {

        var choose = new ChooseStrategyWrapped(board, token);

        if (FirstPlay(board)) //Si es l primera partida siempre se puede poner ficha
        {
            return (new ChooseStrategyWrapped(board, token, true));
        }
        ChooseSideWrapped front = ValidPlayFront(board, token);// Si se puede jugar por el indice 0
        choose.AddSide(front);
        ChooseSideWrapped back = ValidPlayBack(board, token);// Si se puede jugar por el ultimo indice
        choose.AddSide(back);
        return choose;

    }
    protected virtual ChooseSideWrapped ValidPlayFront(IBoard board, IToken token)
    {


        ChooseSideWrapped choose = new ChooseSideWrapped(0);

        if (Match((int)token.Part1.ComponentValue, (int)board.First.Part1.ComponentValue))
        {
            choose.AddSide(1);//Girar la ficha
        }
        if (Match((int)token.Part2.ComponentValue, (int)board.First.Part1.ComponentValue))
        {
            choose.AddSide(0);//No GirarLaFicha
        }
        choose.Run();
        return choose;
    }

    protected abstract bool Match(double Part1, double part2);

    protected virtual bool FirstPlay(IBoard board)
    {
        if (board.board == null || board.board.Count == 0) return true;
        return false;
    }

    protected virtual ChooseSideWrapped ValidPlayBack(IBoard board, IToken token)
    {
        ChooseSideWrapped choose = new ChooseSideWrapped(1);


        if (Match(token.Part1.ComponentValue, board.Last.Part2.ComponentValue))
        {
            choose.AddSide(0);//No girar
        }

        if (Match(token.Part2.ComponentValue, board.Last.Part2.ComponentValue))
        {
            choose.AddSide(1);//Girar
        }
        choose.Run();
        return choose;
    }



}
#endregion

# region Derivates class
public class ClassicPlay : ValidPlayClass
{
    public static new string Description => "Clasico (solo si los numeros son iguales)";
    protected override bool Match(double Part1, double part2)
    {

        return (part2.Equals(Part1));
    }

    public override string ToString()
    {
        return Description;
    }



}

public class BiggerValidPlay : ValidPlayClass
{
    public static new string Description => "Solo si el numero a jugar es mayor al numero ya en la mesa";

    protected override bool Match(double token, double board)
    {
        return (board < token);
    }

    public override string ToString()
    {
        return Description;
    }
}

public class SmallerValidPlay : ValidPlayClass
{
    public static new string Description => "Solo si el numero a jugar es menor al numero ya en la mesa";


    protected override bool Match(double Part1, double part2)
    {
        return (part2 > Part1);
    }

    public override string ToString()
    {
        return Description;
    }
}
#endregion

#endregion

