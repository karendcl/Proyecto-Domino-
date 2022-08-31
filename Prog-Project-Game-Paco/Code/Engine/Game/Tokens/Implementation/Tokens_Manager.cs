namespace Game;

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

    public int GetHashCode(IToken obj)
    {

        return obj.GetHashCode();
    }
}
