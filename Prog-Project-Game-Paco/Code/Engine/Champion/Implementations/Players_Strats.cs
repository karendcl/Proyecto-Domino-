namespace Game;

public class PlayerStats : IEquatable<PlayerStats> //Da la informacion de cada jugador a pantalla
{
    public IPlayer player { get; protected set; }
    public double punctuation { get; protected set; } = -1;

    public PlayerStats(IPlayer player)
    {
        this.player = player;
    }

    public void AddPuntuation(double punctuation)
    {
        if (this.punctuation < 0) this.punctuation = punctuation;
    }

    public bool Equals(PlayerStats other)
    {
        if (other == null) return false;
        return this.player.Equals(other.player);
    }

    public override string ToString()
    {
        return string.Format($"{this.player} : {this.punctuation}");
    }
}
