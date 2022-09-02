namespace Game;


/// <summary>
///  It is a token that contains two parts where these two parts must be ITokenizable objects
/// </summary>
/// <param name=""></param>
/// <returns></returns>
public interface IToken
{
    ITokenizable Part1 { get; }
    ITokenizable Part2 { get; }

    IToken Clone();

    void SwapToken();

    string ToString();
}