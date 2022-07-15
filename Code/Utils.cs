namespace Game;

public static class Utils
{
    //esto es para el reflection. devuelve un array de tipos que implementan una interfaz o que heredan de una clase
    public static Type[] TypesofFichas()
    {
        return typeof(IGenerator).Assembly.GetTypes().Where(x => typeof(IGenerator).IsAssignableFrom(x) && x.IsClass).ToArray();
    }

    public static Type[] TypesofValidPlayGame()
    {
        return typeof(ValidPlayClass).Assembly.GetTypes().Where(x => typeof(ValidPlayClass).IsAssignableFrom(x) && x.IsClass && !x.IsAbstract).ToArray();
    }

    public static Type[] TypesOfScoreGame()
    {
        return typeof(IGetScore<IToken>).Assembly.GetTypes().Where(x => typeof(IGetScore<IToken>).IsAssignableFrom(x) && x.IsClass).ToArray();
    }

    public static Type[] TypesOfChampionJudge()
    {
        return typeof(IChampionJudge<GameStatus>).Assembly.GetTypes().Where(x => typeof(IChampionJudge<GameStatus>).IsAssignableFrom(x) && x.IsClass).ToArray();
    }

    public static Type[] TypesOfPlayerStrategies()
    {
        return typeof(IPlayerStrategy).Assembly.GetTypes().Where(x => typeof(IPlayerStrategy).IsAssignableFrom(x) && x.IsClass).ToArray();
    }

    public static Type[] TypesofValidChampion()
    {
        return typeof(IValidPlay<List<IGame<GameStatus>>, IPlayer, bool>).Assembly.GetTypes().Where(x => typeof(IValidPlay<List<IGame<GameStatus>>, IPlayer, bool>).IsAssignableFrom(x) && x.IsClass).ToArray();
    }

    public static Type[] TypesOfStopConditionTorneo()
    {
        return typeof(IStopGame<List<IGame<GameStatus>>, List<IPlayerScore>>).Assembly.GetTypes().Where(x => typeof(IStopGame<List<IGame<GameStatus>>, List<IPlayerScore>>).IsAssignableFrom(x) && x.IsClass).ToArray();
    }

    public static Type[] TypesofWinConditionGame()
    {
        return typeof(WinCondition).Assembly.GetTypes().Where(x => typeof(WinCondition).IsAssignableFrom(x) && x.IsClass && !x.IsAbstract).ToArray();
    }

    public static Type[] TypesofStopConditionGame()
    {
        return typeof(IStopGame<Player, IToken>).Assembly.GetTypes().Where(x => typeof(IStopGame<IPlayer, IToken>).IsAssignableFrom(x) && x.IsClass && !x.IsAbstract).ToArray();
    }

    public static Type[] TypesOfJudgeGame()
    {
        return typeof(IJudgeGame).Assembly.GetTypes().Where(x => typeof(IJudgeGame).IsAssignableFrom(x) && x.IsClass).ToArray();
    }

    public static Type[] TypesOfPlayer()
    {
        return typeof(IPlayer).Assembly.GetTypes().Where(x => typeof(IPlayer).IsAssignableFrom(x) && x.IsClass && !x.IsAbstract).ToArray();
    }





}
