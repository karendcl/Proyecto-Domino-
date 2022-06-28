namespace Game;

public class RandomStrategy : IPlayerStrategy
{
    public int Evaluate(Token token, List<Token> hand, WatchPlayer watchPlayer)
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
    public int Evaluate(Token token, List<Token> hand, WatchPlayer watchPlayer)
    {
        return watchPlayer.howtogetscore.Score(token);
    }

    public int ChooseSide(ChooseStrategyWrapped choose, WatchPlayer watch)
    {
        if (watch.board.board is null || watch.board.board.Count == 0) return 0;
        return (watch.board.board.First().Part1 > watch.board.board.Last().Part2) ? 1 : 0;
    }
}

public class SemiSmart : IPlayerStrategy
{
    public int Evaluate(Token token, List<Token> hand, WatchPlayer watch)
    {
        int valor = 0;

        foreach (var item in hand)
        {
            ChooseStrategyWrapped choose = watch.validPlay.ValidPlay(watch.board, token);
            if (choose.CanMatch) valor++;
        }

        if (token.IsDouble()) valor++;

        valor += (int)(watch.howtogetscore.Score(token) / 2);

        return valor;
    }

    public int ChooseSide(ChooseStrategyWrapped choose, WatchPlayer watch)
    {
        IBoard board = watch.board;
        if (board.board is null || board.board.Count == 0) return 0;
        return (board.board.First().Part1 > board.board.Last().Part2) ? 1 : 0;
    }
}
