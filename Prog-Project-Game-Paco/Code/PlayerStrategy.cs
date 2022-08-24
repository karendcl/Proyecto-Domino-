namespace Game;

public class RandomStrategy : IPlayerStrategy
{
    public static string Description => "Estrategia Random ";

    public int Evaluate(IToken itoken, List<IToken> hand, IWatchPlayer watchPlayer)
    {
        //le da una evaluacion random a las fichas
        var r = new Random();
        return r.Next(1, 100);
    }

    public int ChooseSide(IChooseStrategyWrapped choose, IWatchPlayer watchPlayer)
    {
        //selecciona un lado a jugar random
        var r = new Random();
        return r.Next(2);
    }

    public override string ToString()
    {
        return Description;
    }


}

public class BGStrategy : IPlayerStrategy
{
    public static string Description => "Estrategia BotaGorda";

    public int Evaluate(IToken itoken, List<IToken> hand, IWatchPlayer watchPlayer)
    {
        //le da un score a la ficha que le corresponde al valor de la ficha
        return (int)watchPlayer.howtogetscore.Score(itoken);
    }

    public int ChooseSide(IChooseStrategyWrapped choose, IWatchPlayer watch)
    {
        //va a jugar por el lado del tablero que sea de menor valor
        if (watch.board.board is null || watch.board.board.Count == 0) return 0;
        return ((watch.board.board.First().Part1.ComponentValue) > watch.board.board.Last().Part2.ComponentValue) ? 1 : 0;
    }

    public override string ToString()
    {
        return Description;
    }
}

public class SemiSmart : IPlayerStrategy
{

    public static string Description => "Estrategia de un jugador semi-inteligente";

    public int Evaluate(IToken itoken, List<IToken> hand, IWatchPlayer watch)
    {
        //le da un valor a la ficha en dependencia de cierto criterio
        int valor = 0;

        foreach (var item in hand)
        {
            IChooseStrategyWrapped choose = watch.validPlay.ValidPlay(watch.board, itoken);
            if (choose.CanMatch) valor++;
        }



        valor += (int)(watch.howtogetscore.Score(itoken) / 2);

        return valor;
    }

    public int ChooseSide(IChooseStrategyWrapped choose, IWatchPlayer watch)
    {
        //elige el lado por el cual ponerlo
        IBoard board = watch.board;
        if (board.board is null || board.board.Count == 0) return 0;
        return ((board.board.First().Part1.ComponentValue) > (board.board.Last().Part2.ComponentValue)) ? 1 : 0;
    }

    public override string ToString()
    {
        return Description;
    }
}
