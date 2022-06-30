using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Game;
/*

public class Token : ICloneable<Token>
{
    public virtual int Part1 { set; get; }
    public virtual int Part2 { set; get; }


    public Token(int Part1, int Part2)
    {
        this.Part1 = Part1;
        this.Part2 = Part2;
    }

    public override string ToString()
    {
        return "[" + Part1 + "|" + Part2 + "] ";
    }



    public bool Contains(int a)
    {
        return (this.Part1.Equals(a) || this.Part2.Equals(a));
    }

    public bool IsDouble()
    {
        return (this.Part1.Equals(this.Part2));
    }

    public void SwapToken()
    {
        var temp = this.Part1;
        this.Part1 = this.Part2;
        this.Part2 = temp;
    }

    public Token Clone()
    {
        return new Token(this.Part1, this.Part2);
    }
}


public class RandomToken : Token
{
    protected int Part1Original => base.Part1;
    protected int Part2Original => base.Part2;

    public override int Part1 { get { return Part_1(); } }
    public override int Part2 { get { return Part_2(); } }

    protected readonly int MaxDouble;

    protected readonly int porcent;
    public RandomToken(int Part1, int Part2, int MaxDouble, int porcent) : base(Part1, Part2)
    {
        this.MaxDouble = MaxDouble;
        this.porcent = porcent;
    }

    protected int RandomC()
    {
        Random random = new Random();
        int x = random.Next(0, 10000);
        return x;
    }
    protected int Part_1()
    {
        int x = RandomC();

        if (x > porcent * 100)
        {
            Random random = new Random();
            int ran = random.Next(0, MaxDouble);
            return ran;
        }

        return Part1Original;
    }

    protected int Part_2()
    {
        int x = RandomC();

        if (x > porcent * 100)
        {
            Random random = new Random();
            int ran = random.Next(0, MaxDouble);
            return ran;
        }

        return Part2Original;
    }
}

*/
/*

public class Pinguino : IComparable<Pinguino>, ITokenizable
{
    public string Description { get; set; } = "Pinguino";
    public int altura
    { get; protected set; }
    public int peso { get; protected set; }

    public double ComponentValue { get; protected set; }

    public int age { get; protected set; }
    Stopwatch watch = new Stopwatch();

    public Pinguino(int Altura, int Peso)
    {
        this.peso = peso;
        this.altura = altura;
        watch.Start();
    }

    protected int AgeNow()
    {
        return (int)watch.ElapsedMilliseconds * 20000;
    }

    public double ComponentValueMethod()
    {
        return age * peso * altura / Math.E;
    }

    public int CompareTo(Pinguino? other)
    {
        if (this.age > other.age) return 1;
        if (this.age < other.age) return -1;
        return 0;
    }


}
*/

public class NormalInt : ITokenizable
{
    public string Description => "Enteros";

    public double ComponentValue { get; protected set; }

    public NormalInt(int value) { this.ComponentValue = value; }
    public int CompareTo(ITokenizable? obj)
    {
        return this.ComponentValue.CompareTo(obj.ComponentValue);
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

    public string Description { get => "Termoeléctrica"; }

    public double ComponentValue => PotencialKnow();



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




public class Token : IEquatable<Token>, ICloneable<Token>
{
    //El generador se asegura que todos tengan igual cant de componentes como el de mayor dimension
    public List<ITokenizable> Component { get; set; }
    public int Dimension { get => this.Component.Count; }

    public double Norma { get => CalculateNorma(); }

    public ITokenizable First { get => this.GetFirst(); }

    public ITokenizable Last { get => this.GetLast(); }

    protected virtual double CalculateNorma()
    {
        double xNorma = 0;
        foreach (var itemx in this.Component)
        {
            xNorma += Math.Pow(itemx.ComponentValue, 2);
        }
        return Math.Sqrt(xNorma);
    }

    public Token(List<ITokenizable> Component)
    {
        this.Component = Component;
    }

    public bool IsDouble()
    {
        return (this[0] == this[1]);
    }
    public ITokenizable this[int index]
    {
        get => Component[index];
    }
    protected ITokenizable GetLast()
    {
        int x = this.Component.Count - 1;
        return this.Component[x];
    }

    protected ITokenizable GetFirst()
    {
        return this.Component[0];
    }

    public bool Equals(Token? other)
    {
        if (other == null) return false;
        if (this.Dimension != other.Dimension) { return false; }
        foreach (var (This, Other) in this.Component.Zip(other.Component))
        {
            if (!This.Equals(Other)) return false;
        }
        return true;
    }

    public Token Clone() => new Token(this.Component);

    public override string ToString()
    {
        string x = "[";
        for (int i = 0; i < this.Component.Count; i++)
        {
            x += " , " + this.Component[i];
        }
        x += "]";
        return x;
    }

    public bool SwapToken(int oldIndex, int newIndex)
    {
        if (oldIndex < 0 || newIndex < 0 || oldIndex >= this.Component.Count || newIndex >= this.Component.Count)
        { return false; }
        ITokenizable component = this.Component[oldIndex];
        this.Component.RemoveAt(oldIndex);
        if (this.Component.Count - 1 < newIndex) { this.Component.Add(component); return true; }
        else { this.Component.Insert(newIndex, component); }
        return true;
    }

    public bool SwapToken(List<(int, int)> change)
    {
        bool x = false;
        foreach (var item in change)
        {

            if (this.SwapToken(item.Item1, item.Item2)) x = true;
        }

        return x;
    }


}









public class TokensManager
{
    protected int TokensForEach { get; set; }
    public List<Token> Elements { get; protected set; }

    public IEqualityComparer<Token> equalityComparer { get; protected set; }

    public IComparer<Token> Comparer { get; protected set; }

    public TokensManager(int TokensForEach, IComparer<Token> Comparer, IEqualityComparer<Token> equalityComparer, List<Token> tokens)
    {

        this.Comparer = Comparer;
        this.Elements = new List<Token>();
        this.Comparer = Comparer;
        this.equalityComparer = equalityComparer;
        this.Elements = tokens;
        this.TokensForEach = TokensForEach;
    }


    public bool ItsDouble(Token token)
    {
        ITokenizable x = token[0];
        foreach (var item in token.Component)
        {
            if (item != x) return false;
        }
        return true;
    }



    public List<Token> SetTokensRandom()
    {


        int count = 0;
        var list = new List<Token>();

        while (count != TokensForEach)
        {
            int cantidadDisponible = this.Elements.Count;
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


public class ComparerTokens : IComparer<Token>
{

    public int Compare(Token? x, Token? y)
    {
        double tempx = 0;
        foreach (var item in x!.Component!) { tempx += item.ComponentValue; }

        double tempy = 0;

        foreach (var item in y!.Component!)
        {
            tempy += item.ComponentValue;
        }
        return tempx.CompareTo(tempy);

    }
}

public class IEquatablePorPedazos : IEqualityComparer<Token>
{
    public bool Equals(Token? x, Token? y)
    {
        List<ITokenizable> list = (x.Component.Count > y.Component.Count) ? x.Component : y.Component;
        foreach (var (xComponent, yComponent) in x.Component.Zip(y.Component))
        {
            if (!xComponent.Equals(yComponent)) return false;
        }
        return true;

    }

    public int GetHashCode([DisallowNull] Token obj)
    {
        return obj.GetHashCode();
    }
}


public class IntTokenGenerator
{
    public List<Token> CreateTokens(int MaxValue)
    {

        List<Token> PosiblesTokens = new List<Token>();

        for (int i = 0; i <= MaxValue; i++)
        {
            for (int j = i; j <= MaxValue; j++)
            {
                ITokenizable temp1 = new NormalInt(i);
                ITokenizable temp2 = new NormalInt(j);
                Token token = new Token(new List<ITokenizable>() { temp1, temp2 });
                PosiblesTokens.Add(token);
            }
        }
        return PosiblesTokens;
    }
}















