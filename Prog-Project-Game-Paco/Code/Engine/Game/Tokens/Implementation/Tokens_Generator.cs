namespace Game;

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










