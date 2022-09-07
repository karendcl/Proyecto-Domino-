namespace Game;

#region ITokenizable


public abstract class Tokenizable : ITokenizable
{
    public static string Description { get; } = "Description";
    public abstract double ComponentValue { get; protected set; }

    public abstract ITokenizable Clone();


    public virtual int CompareTo(ITokenizable other)
    {
        return this.ComponentValue.CompareTo(other?.ComponentValue);

    }

    public virtual bool Equals(ITokenizable other)
    {
        if (other == null) return false;
        return this.ComponentValue.Equals(other.ComponentValue);
    }


    public string Paint() => Description;


}

/* Todos los objetos pueden ser tokenizados solo con implementar ITokenizable */
/// <summary>
///  Enteros siendo Tokenizados
/// </summary>
/// <param name=""></param>
/// <returns></returns>
public class NormalInt : Tokenizable
{
    public static string Description => "Enteros";

    public override double ComponentValue { get; protected set; }

    public NormalInt(int value) { this.ComponentValue = value; }

    public override ITokenizable Clone()
    {
        return new NormalInt((int)this.ComponentValue);
    }

    public override string ToString()
    {
        return this.ComponentValue.ToString();

    }

    public string Paint()
    {
        return Description;
    }
}
/// <summary>
///  Energy Generator ITokenizable
/// </summary>
/// <param name=""></param>
/// <returns></returns>
public class EnergyGenerator : Tokenizable
{
    public string name { get; set; }

    public double minPotenci { get; protected set; }
    public double maxPotenci { get; protected set; }

    public static string Description { get => "Termoeléctrica"; }

    public override double ComponentValue { get { return PotencialNow(); } protected set { this.ComponentValue = value; } }// Este es el valor a tener en cuenta para un token

    public override ITokenizable Clone()
    {
        return new EnergyGenerator(this.name, this.minPotenci, this.maxPotenci);
    }

    public double PotencialNow()
    {
        Random random = new Random();
        int x = random.Next(-1, 1);
        return x * random.Next((int)minPotenci, (int)maxPotenci);
    }

    public override string ToString()
    {
        return this.ComponentValue.ToString();

    }


    public EnergyGenerator(string name, double minPotenci, double maxPotenci)
    {
        this.name = name;
        this.minPotenci = minPotenci;
        this.maxPotenci = maxPotenci;

    }

}

#endregion