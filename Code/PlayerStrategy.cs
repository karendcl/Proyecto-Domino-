namespace Game;

public class RandomStrategy : IPlayerStrategy
{
    public string Description => " RandomStrategy ";

    public int Evaluate(IToken itoken, List<IToken> hand, WatchPlayer watchPlayer)
    {
        var r = new Random();
        return r.Next(1, 100);
    }

    public int ChooseSide(ChooseStrategyWrapped choose, WatchPlayer watchPlayer)
    {//Agregar aca la parte de seleccionar uno de ellos random implementar Ienumerable
        var r = new Random();
        return r.Next(2);
    }


}

public class BGStrategy : IPlayerStrategy
{
    public string Description => "BGStrategy";

    public int Evaluate(IToken itoken, List<IToken> hand, WatchPlayer watchPlayer)
    {
        return watchPlayer.howtogetscore.Score(itoken);
    }

    public int ChooseSide(ChooseStrategyWrapped choose, WatchPlayer watch)
    {
        if (watch.board.board is null || watch.board.board.Count == 0) return 0;
        return ((watch.board.board.First().Part1.ComponentValue) > watch.board.board.Last().Part2.ComponentValue) ? 1 : 0;
    }
}

public class SemiSmart : IPlayerStrategy
{

    public string Description => "SemiSmart";

    public int Evaluate(IToken itoken, List<IToken> hand, WatchPlayer watch)
    {
        int valor = 0;

        foreach (var item in hand)
        {
            ChooseStrategyWrapped choose = watch.validPlay.ValidPlay(watch.board, itoken);
            if (choose.CanMatch) valor++;
        }

        if (itoken.ItsDouble()) valor++;

        valor += (int)(watch.howtogetscore.Score(itoken) / 2);

        return valor;
    }

    public int ChooseSide(ChooseStrategyWrapped choose, WatchPlayer watch)
    {
        Board board = watch.board;
        if (board.board is null || board.board.Count == 0) return 0;
        return ((board.board.First().Part1.ComponentValue) > (board.board.Last().Part2.ComponentValue)) ? 1 : 0;
    }
}
