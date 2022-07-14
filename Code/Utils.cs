namespace Game;

public static class Utils
{
    //esto es para el reflection. devuelve un array de tipos que implementan una interfa o que heredan de una clase
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
        return typeof(ChampionJudge).Assembly.GetTypes().Where(x => typeof(ChampionJudge).IsAssignableFrom(x) && x.IsClass).ToArray();
    }

    public static Type[] TypesOfPlayerStrategies()
    {
        return typeof(IPlayerStrategy).Assembly.GetTypes().Where(x => typeof(IPlayerStrategy).IsAssignableFrom(x) && x.IsClass).ToArray();
    }

    public static Type[] TypesofValidChampion()
    {
        return typeof(IValidPlay<List<Game>, Player, bool>).Assembly.GetTypes().Where(x => typeof(IValidPlay<List<Game>, Player, bool>).IsAssignableFrom(x) && x.IsClass).ToArray();
    }

    public static Type[] TypesOfStopConditionTorneo()
    {
        return typeof(IStopGame<List<Game>, List<IPlayerScore>>).Assembly.GetTypes().Where(x => typeof(IStopGame<List<Game>, List<IPlayerScore>>).IsAssignableFrom(x) && x.IsClass).ToArray();
    }

    public static Type[] TypesofWinConditionGame()
    {
        return typeof(WinCondition).Assembly.GetTypes().Where(x => typeof(WinCondition).IsAssignableFrom(x) && x.IsClass && !x.IsAbstract).ToArray();
    }

    public static Type[] TypesofStopConditionGame()
    {
        return typeof(IStopGame<Player, IToken>).Assembly.GetTypes().Where(x => typeof(IStopGame<Player, IToken>).IsAssignableFrom(x) && x.IsClass && !x.IsAbstract).ToArray();
    }

    public static Type[] TypesOfJudgeGame()
    {
        return typeof(Judge).Assembly.GetTypes().Where(x => typeof(Judge).IsAssignableFrom(x) && x.IsClass && !x.IsAbstract).ToArray();
    }

    public static Type[] TypesOfPlayer()
    {
        return typeof(Player).Assembly.GetTypes().Where(x => typeof(Player).IsAssignableFrom(x) && x.IsClass && !x.IsAbstract).ToArray();
    }





}
