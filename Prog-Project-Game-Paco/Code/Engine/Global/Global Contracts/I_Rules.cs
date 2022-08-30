namespace Game;
#region Rules

public interface IWinCondition<TCriterio, TToken> : IDescriptible
{

    /// <summary>
    ///  Returns the list of winners
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    List<IPlayer> Winner(List<TCriterio> criterios, IGetScore<TToken> howtogetscore);
}

public interface IValidPlay<TGame, TPlayer, TCriterio> : IDescriptible
{      /// <summary>
///     Returns under certain criteria if something is valid or not
/// </summary>
/// <param name=""></param>
/// <returns></returns>
    TCriterio ValidPlay(TGame game, TPlayer player);


}

public interface IStopGame<TCriterio, TToken> : IDescriptible
{    /// <summary>
///  Returns true if the stopping premises are met, false if they are not met
/// </summary>
/// <param name=""></param>
/// <returns></returns>
    bool MeetsCriteria(TCriterio criterio, IGetScore<TToken> howtogetscore);
}


public interface IGetScore<TToken> : IDescriptible
{    /// <summary>
///  Returns the score that has a certain criteria
/// </summary>
/// <param name=""></param>
/// <returns></returns>
    double Score(TToken item);
}

#endregion
