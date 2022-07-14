namespace Game;



/// <summary>
///  Es un envoltorio para seleccionar la posicion de la ficha en el tablero
/// </summary>
/// <param name=""></param>
/// <returns></returns>
public class ChooseStrategyWrapped : IEquatable<ChooseStrategyWrapped>, IChooseStrategyWrapped
{
    public bool CanMatch { get; private set; } = false;// Si se puede poner la ficha en el tablero

    public bool FirstPlay { get; private set; } = false; //Si es la primera partida
    public IBoard board { get; private set; } // el tablero
    public IToken itoken { get; private set; } // TOken a poner
    public List<ChooseSideWrapped> side { get; private set; }// Lista de envoltorio de las posibles posiciones a poner

    public ChooseStrategyWrapped(IBoard board, IToken itoken, bool FirstPlay = false)
    {
        this.FirstPlay = FirstPlay;
        this.CanMatch = FirstPlay;
        this.board = board;
        this.side = new List<ChooseSideWrapped>() { };
        this.itoken = itoken;

    }


    /// <summary>
    ///  Controla que el lugar a poner sea valido y devuelve el envoltorio para dicho indice
    /// </summary>
    /// <param name=""></param>
    /// <returns>retorna true si es posible y el envoltorio correspondiente,  false si no es posible y null el envoltorio</returns>
    public virtual (bool, ChooseSideWrapped) ControlSide(int side)
    {
        if (side >= this.side.Count) { return (false, null)!; }
        ChooseSideWrapped temp = this.side[side];
        if (!temp.canChoose)
        {
            for (int i = 0; i < this.side.Count; i++)
            {
                if (this.side[i].canChoose) { temp = this.side[i]; }
            }
        }
        return (temp.canChoose, temp);
    }

    /// <summary>
    ///  Se añade el envoltorio de un posible lugar a poner el token
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    public void AddSide(ChooseSideWrapped side)
    {
        this.side.Add(side);
        if (side.canChoose)
        {
            CanMatch = true;
        }
    }

    public bool Equals(ChooseStrategyWrapped? other)
    {  // SOn iguales si tienen el mismo itoken
        if (other == null) { return false; }
        return (this.itoken.Equals(other.itoken));
    }
}



/// <summary>
///  Seleccion de lugares donde se puede o no poner 
/// </summary>
/// <param name=""> Contiene el indice en numeros enteros y el bool canChoose si se puede poner</param>
/// <returns> </returns>
public class ChooseSideWrapped : IChooseSideWrapped
{
    public int index { get; private set; }//Indice a poner el token
    public bool canChoose { get; private set; } = false;//Si se puede poner el token

    public List<int> WhereCanMacht { get; private set; }

    internal ChooseSideWrapped(int index)
    {
        this.index = index;
        this.WhereCanMacht = new List<int>() { };

    }

    public void AddSide(int i)
    {
        WhereCanMacht.Add(i);
    }
    public void Run()//Verifica si se puede o no 
    {
        if (WhereCanMacht.Count > 0) { canChoose = true; }
    }
}
