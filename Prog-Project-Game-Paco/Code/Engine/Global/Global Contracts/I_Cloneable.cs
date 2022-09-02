namespace Game;

public interface ICloneable<T> : ICloneable
{
    new T Clone();
    Object ICloneable.Clone() => Clone()!;
}

public interface ICloneable<T1, T2> : ICloneable<T1>
{
    T1 Clone(T2 item);

}