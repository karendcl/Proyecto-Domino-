namespace Game;
public class Events
{

    public event Func<string, int>? Asksmint;

    public event Predicate<Orders>? BooleanAsk;

    public event Action<ChampionStatus>? Status;

    public void Run(ChampionStatus status) => Status(status);

    public int IntermediarioInt(string msg) => Asksmint(msg);

    public bool IntermediarioBool(Orders orders) => BooleanAsk(orders);



}