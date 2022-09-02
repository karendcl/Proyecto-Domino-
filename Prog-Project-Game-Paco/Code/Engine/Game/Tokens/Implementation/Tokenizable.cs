namespace Game;

#region ITokenizable

/* Todos los objetos pueden ser tokenizados solo con implementar ITokenizable */
/// <summary>
///  Enteros siendo Tokenizados
/// </summary>
/// <param name=""></param>
/// <returns></returns>
public class NormalInt : ITokenizable
{
    public static string Description => "Enteros";

    public double ComponentValue { get; protected set; }

    public NormalInt(int value) { this.ComponentValue = value; }
    public int CompareTo(ITokenizable? obj)
    {
        return this.ComponentValue.CompareTo(obj?.ComponentValue);
    }


    public override string ToString()
    {
        return this.ComponentValue.ToString();

    }

    public bool Equals(ITokenizable? other)
    {
        if (other == null) return false;
        return this.ComponentValue.Equals(other.ComponentValue);
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
public class EnergyGenerator : ITokenizable
{
    public string name { get; set; }

    public double minPotenci { get; protected set; }
    public double maxPotenci { get; protected set; }

    public static string Description { get => "Termoeléctrica"; }

    public double ComponentValue => PotencialNow();// Este es el valor a tener en cuenta para un token


    public override string ToString()
    {
        return this.ComponentValue.ToString();
    }


    public double PotencialNow()
    {
        Random random = new Random();
        int x = random.Next(-1, 1);
        return x * random.Next((int)minPotenci, (int)maxPotenci);
    }

    public string Paint()
    {
        return Description;
    }

    public int CompareTo(ITokenizable? other)
    {
        if (other == null) return -1;
        return this.ComponentValue.CompareTo(other.ComponentValue);
    }

    public bool Equals(ITokenizable? other)
    {
        if (other == null) return false;
        return this.ComponentValue.Equals(other.ComponentValue);
    }

    public EnergyGenerator(string name, double minPotenci, double maxPotenci)
    {
        this.name = name;
        this.minPotenci = minPotenci;
        this.maxPotenci = maxPotenci;

    }

}

#endregion