
namespace Game;

/// <summary>
///  Contains the tokens already played
/// </summary>
/// <param name=""></param>
/// <returns></returns>
public interface IBoard : ICloneable<IBoard>
{
    List<IToken> board { get; }
    IToken First { get; }
    IToken Last { get; }

    void AddTokenToBoard(IToken itoken, int side);
    new IBoard Clone();
    IBoard Clone(List<IToken> CopyTokens);
    string ToString();
}
