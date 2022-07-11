using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Game;


#region ITokenizable

public class NormalInt : ITokenizable
{
    public static string Description => "Enteros";

    public double ComponentValue { get; protected set; }

    public NormalInt(int value) { this.ComponentValue = value; }
    public int CompareTo(ITokenizable? obj)
    {
        return this.ComponentValue.CompareTo(obj?.ComponentValue);
    }

    public string Paint()
    {
        return this.ComponentValue.ToString();


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
}
public class EnergyGenerator : ITokenizable
{
    public string name { get; set; }

    public int minPotenci { get; protected set; }
    public int maxPotenci { get; protected set; }

    public static string Description { get => "Termoeléctrica"; }

    public double ComponentValue => PotencialKnow();


    public override string ToString()
    {
        return this.ComponentValue.ToString();
    }


    public double PotencialKnow()
    {
        Random random = new Random();
        int x = random.Next(-1, 1);
        return x * random.Next(minPotenci, maxPotenci);

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

    public EnergyGenerator(string name, int minPotenci, int maxPotenci)
    {
        this.name = name;
        this.minPotenci = minPotenci;
        this.maxPotenci = maxPotenci;

    }

}

#endregion

#region  TokenClass

public class Token : IToken
{
    public virtual ITokenizable Part1 { protected set; get; }
    public virtual ITokenizable Part2 { protected set; get; }



    public Token(ITokenizable Part1, ITokenizable Part2)
    {
        this.Part1 = Part1;
        this.Part2 = Part2;
    }

    public virtual bool ItsDouble()
    {
        return (this.Part1.Equals(this.Part2));
    }



    public override string ToString()
    {
        return "[" + Part1 + "|" + Part2 + "] ";
    }

    public virtual bool IsMatch(IToken other)
    {
        return (this.Part1.Equals(other.Part1)) ||
               (this.Part1.Equals(other.Part2)) ||
               (this.Part2.Equals(other.Part2)) ||
               (this.Part2.Equals(other.Part1));
    }

    public virtual bool Contains(int a)
    {
        return (this.Part1.Equals(a) || this.Part2.Equals(a));
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

    public IComparer<IToken> Comparer { get; protected set; }

    public TokensManager(int TokensForEach, IComparer<IToken> Comparer, IEqualityComparer<IToken> equalityComparer, List<IToken> tokens)
    {

        this.Comparer = Comparer;
        this.Elements = new List<IToken>();
        this.Comparer = Comparer;
        this.equalityComparer = equalityComparer;
        this.Elements = tokens;
        this.TokensForEach = TokensForEach;
    }


    public virtual bool ItsDouble(IToken x)
    {
        return (x.Part1.Equals(x.Part2));
    }



    public List<IToken> GetTokens()
    {


        int count = 0;
        var list = new List<IToken>();

        while (count != TokensForEach)
        {
            int cantidadDisponible = this.Elements.Count;
            if (cantidadDisponible < 1) break;
            var r = new Random();
            var index = r.Next(0, cantidadDisponible);
            cantidadDisponible--;
            count++;
            list.Add(this.Elements[index]);
            this.Elements.RemoveAt(index);

        }

        return list;
    }


}


public class ComparerTokens : IComparer<IToken>
{

    public int Compare(IToken? x, IToken? y)
    {
        int temp = 0;
        int xNorma = (int)(Math.Pow(2, x.Part1.ComponentValue) + Math.Pow(2, x.Part2.ComponentValue));

        int yNorma = (int)(Math.Pow(2, y.Part1.ComponentValue) + Math.Pow(2, y.Part2.ComponentValue));

        if (xNorma == yNorma) return 0;
        if (xNorma < yNorma) return -1;
        return 1;

    }
}

public class IEquatablePorCaras : IEqualityComparer<IToken>
{
    public bool Equals(IToken? x, IToken? y)
    {
        if (x == null || y == null) return false;
        bool a = (x.Part1 == y.Part1 || x.Part1 == y.Part2);
        bool b = (x.Part2 == y.Part1 || x.Part2 == y.Part1);
        return (a || b);

    }

    public int GetHashCode([DisallowNull] IToken obj)
    {
        return obj.GetHashCode();
    }
}


public class Fichas_Enteros :IGenerator
{
    public static string Description{get => "Fichas de enteros";}
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



public class Fichas_Termoelectricas : IGenerator
{

    public static string Description{get => "Fichas de termoelectrica";}
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
            int min = random.Next(i, 300);
            int maxPotenci = random.Next(300, 3000);
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















