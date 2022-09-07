namespace Game;

/// <summary>
///  All objects that implement it must have a component value.
/// </summary>
/// <param name=""></param>
/// <returns></returns>
public interface ITokenizable : IComparable<ITokenizable>, IEquatable<ITokenizable>, IDescriptible, ICloneable<ITokenizable>
{
    string Paint();
    /// <summary>
    ///  Eigenvalue of object that is due to internal characteristics
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    double ComponentValue { get; }


}
/// <summary>
///  Generates tokens under internal criteria
/// </summary>
/// <param name=""></param>
/// <returns></returns>
