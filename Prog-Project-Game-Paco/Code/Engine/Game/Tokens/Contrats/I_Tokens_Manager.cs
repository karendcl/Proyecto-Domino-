namespace Game;

/// <summary>
///  Manage from the creation to the distribution of the tokens
/// </summary>
/// <param name=""></param>
/// <returns></returns>
public interface ITokensManager : ICloneable<ITokensManager>
{
    List<IToken> Elements { get; }
    IEqualityComparer<IToken> equalityComparer { get; }

    /// <summary>
    ///  Distribute the tokens under a given criteria
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    HashSet<IToken> GetTokens();

}