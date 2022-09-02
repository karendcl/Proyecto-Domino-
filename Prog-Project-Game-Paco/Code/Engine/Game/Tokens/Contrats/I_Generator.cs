namespace Game;

public interface IGenerator : IDescriptible
{
    /// <summary>
    ///  Returns a list of tokens with a double max between the two parts
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    public List<IToken> CreateTokens(int maxDouble);
}
