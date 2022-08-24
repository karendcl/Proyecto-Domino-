using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

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

#region  TokenClass

public class Token : IToken //ficha
{
    public virtual ITokenizable Part1 { protected set; get; }
    public virtual ITokenizable Part2 { protected set; get; }



    public Token(ITokenizable Part1, ITokenizable Part2)
    {
        this.Part1 = Part1;
        this.Part2 = Part2;
    }


    public override string ToString()
    {
        return "[" + Part1 + "|" + Part2 + "] ";
    }

    public virtual void SwapToken()
    {
        var temp = this.Part1;
        this.Part1 = this.Part2;
        this.Part2 = temp;
    }

    public virtual IToken Clone()
    {
        return new Token(this.Part1, this.Part2);
    }
}
#endregion



public class TokensManager : ITokensManager
{
    protected int TokensForEach { get; set; }

    public List<IToken> Elements { get; protected set; }

    public IEqualityComparer<IToken> equalityComparer { get; protected set; }


    public TokensManager(int TokensForEach, IEqualityComparer<IToken> equalityComparer, List<IToken> tokens)
    {
        this.equalityComparer = equalityComparer;
        this.Elements = tokens;
        this.TokensForEach = TokensForEach;
    }






    public HashSet<IToken> GetTokens()
    {

        int count = 0;
        var Set = new HashSet<IToken>(this.equalityComparer);

        while (count != TokensForEach)
        {
            int cantidadDisponible = this.Elements.Count;
            if (cantidadDisponible < 1) break;
            var r = new Random();
            var index = r.Next(0, cantidadDisponible);
            cantidadDisponible--;
            count++;
            Set.Add(this.Elements[index]);
            this.Elements.RemoveAt(index);

        }

        return Set;
    }


}


public class IEquatablePorCaras : IEqualityComparer<IToken>
{
    public bool Equals(IToken? x, IToken? y)
    {
        if (x is null || y is null) { throw new Exception("The tokens can´t be null"); }
        return (x.Part1.Equals(y.Part1)) ||
               (x.Part1.Equals(y.Part2)) ||
               (x.Part2.Equals(y.Part2)) ||
               (x.Part2.Equals(y.Part1));
    }

    public int GetHashCode([DisallowNull] IToken obj)
    {
        return obj.GetHashCode();
    }
}


public class Fichas_Enteros : IGenerator  //genera las fichas normales
{
    public static string Description { get => "Fichas de enteros"; }
    public List<IToken> CreateTokens(int MaxValue)
    {

        List<IToken> PosiblesTokens = new List<IToken>();

        for (int i = 0; i <= MaxValue; i++)
        {
            for (int j = i; j <= MaxValue; j++)
            {
                ITokenizable temp1 = new NormalInt(i);
                ITokenizable temp2 = new NormalInt(j);
                Token token = new Token(temp1, temp2);
                PosiblesTokens.Add(token);
            }
        }
        return PosiblesTokens;
    }
}



public class Fichas_Termoelectricas : IGenerator  //genera las fichas random
{

    public static string Description { get => "Fichas de termoelectrica"; }
    List<string> names = new List<string>()
    {
        "Energía Nuclear ",
        "Energía del Gas Natural",
        "Energia del Crudo",
        "Energia Solar",
        "Energía Eólica"
    };
    public string GetAName()
    {
        Random random = new Random();
        int index = random.Next(0, this.names.Count);
        return this.names[index];
    }



    private List<ITokenizable> GenerateITokenizable(int MaxValue)
    {
        List<ITokenizable> temp = new List<ITokenizable>() { };
        for (int i = 0; i < MaxValue; i++)
        {
            Random random = new Random();
            double min = random.Next(0, MaxValue / 2);
            double maxPotenci = random.Next(MaxValue / 2, MaxValue);


            EnergyGenerator generator = new EnergyGenerator(GetAName(), min, maxPotenci);
            temp.Add(generator);
        }
        return temp;
    }

    public List<IToken> CreateTokens(int MaxValue)
    {

        List<IToken> result = new List<IToken>();
        List<ITokenizable> temp = GenerateITokenizable(MaxValue);

        for (int i = 0; i < MaxValue * MaxValue; i++)
        {
            (ITokenizable, ITokenizable) x = GetTokenizables(temp);
            Token token = new Token(x.Item1, x.Item2);
            result.Add(token);
        }

        return result;
    }

    private (ITokenizable, ITokenizable) GetTokenizables(List<ITokenizable> list) => (GetRandom(list), GetRandom(list));

    private ITokenizable GetRandom(List<ITokenizable> list)
    {
        Random random = new Random();
        int x = list.Count;
        int index = random.Next(0, x - 1);
        return list[index];

    }
}















