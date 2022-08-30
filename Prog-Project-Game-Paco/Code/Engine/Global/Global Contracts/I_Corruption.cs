namespace Game;

public interface ICorruptible
{  /// <summary>
///  Receives a proposal under a double and returns true or false at its discretion
/// </summary>
/// <param name=""></param>
/// <returns></returns>
    bool Corrupt(double ScoreCost);
}