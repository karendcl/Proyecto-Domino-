
namespace Game;
#region  Games

#region  Definiton AbstractClass

public abstract class ValidPlayClass : IValidPlay<IBoard, IToken, IChooseStrategyWrapped>
{

    public ValidPlayClass(IEqualityComparer<IToken> equality, IComparer<IToken> Comparer)
    {

    }
    public static string Description => "Valid Play";

    public virtual IChooseStrategyWrapped ValidPlay(IBoard board, IToken token)
    {

        var choose = new ChooseStrategyWrapped(board, token);
        bool z = false;
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
    public static string Description => "Clasico (solo si los numeros son iguales)";
    public ClassicPlay(IEqualityComparer<IToken> equality, IComparer<IToken> Comparer) : base(equality, Comparer)
    {
    }

    protected override bool Match(double Part1, double part2)
    {
        //part2 = (int)part2;
        //Part1 = (int)Part1;
        return (part2.Equals(Part1));
    }

    public override string ToString()
    {
        return Description;
    }



}

public class BiggerValidPlay : ValidPlayClass
{
    public static string Description => "Solo si el numero a jugar es mayor al numero ya en la mesa";
    public BiggerValidPlay(IEqualityComparer<IToken> equality, IComparer<IToken> Comparer) : base(equality, Comparer)
    {
    }

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
    public static string Description => "Solo si el numero a jugar es menor al numero ya en la mesa";
    public SmallerValidPlay(IEqualityComparer<IToken> equality, IComparer<IToken> Comparer) : base(equality, Comparer)
    {
    }

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

#region Champion
// Si ha perdido mas de las mitad 
public class ValidChampionPorcientoPerdidas : IValidPlay<List<IGame<GameStatus>>, IPlayer, bool>
{
    public double Porcent { get; protected set; }

    public static string Description => "Si ha perdido mas de un x por ciento del total de juegos";

    public ValidChampionPorcientoPerdidas(double Porcent)
    {
        this.Porcent = Porcent;
    }
    public bool ValidPlay(List<IGame<GameStatus>> games, IPlayer player)
    {
        double cant = games.Count * Porcent;
        int count = 0;
        foreach (var game in games)
        {
            int x = game.Winner().IndexOf(player);
            if (x < game.Winner().Count / 2) count++;
            if (count < cant) return false;
        }

        return true;

    }
}



public class ValidChampionPerdidasConsecutivas : IValidPlay<List<IGame<GameStatus>>, IPlayer, bool>
{
    public double CantdeVecesConsecutivas { get; protected set; }

    public static string Description => "Si el jugador ha perdido una x cantidad de veces consecutivas";

    public ValidChampionPerdidasConsecutivas(double cantVeces)
    {

        this.CantdeVecesConsecutivas = cantVeces;

    }

    public bool ValidPlay(List<IGame<GameStatus>> games, IPlayer player)
    {
        if (CantdeVecesConsecutivas < 1) { return true; }
        int count = 0; int lastIndex = 0;
        for (int i = 0; i < games.Count; i++)
        {
            var game = games[i];
            game.Winner().Contains(player);
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