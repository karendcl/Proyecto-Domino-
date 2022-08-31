
namespace Game;
public interface IChooseSideWrapped : IEquatable<IChooseSideWrapped>
{
    /// <summary>
    ///  board index
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    int index { get; }
    /// <summary>
    ///  True if you can set false otherwise
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    bool canChoose { get; }

    /// <summary>
    ///  Returns by which region part of the token should be matched
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    List<int> WhereCanMacht { get; }
    /// <summary>
    ///  Returns by which region part of the token should be matched
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    void AddSide(int i);
    /// <summary>
    ///  Initialize before viewing properties
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    void Run();
}

/// <summary>
///  Contains the behavior of whether or not a token can be put on the board
/// </summary>
/// <param name=""></param>
/// <returns></returns>
public interface IChooseStrategyWrapped : IEquatable<IChooseStrategyWrapped>
{
    /// <summary>
    ///   True if you can set false otherwise
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    bool CanMatch { get; }
    /// <summary>
    ///  True if it is the first move of the game false otherwise
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    bool FirstPlay { get; }

    IBoard board { get; }
    /// <summary>
    ///  Proposed token to put
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    IToken itoken { get; }
    /// <summary>
    ///  List for all the possible regions to put the token accepted by the rules of the game
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    List<ChooseSideWrapped> side { get; }
    /// <summary>
    ///  Controla que el lugar a poner sea valido y devuelve el envoltorio para dicho indice
    /// </summary>
    /// <param name=""></param>
    /// <returns>retorna true si es posible y el envoltorio correspondiente,  false si no es posible y null el envoltorio</returns>  
    void AddSide(ChooseSideWrapped side);
    (bool, ChooseSideWrapped) ControlSide(int side);


   
}