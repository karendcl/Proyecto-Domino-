namespace Game;

#region Champion
// Si ha perdido mas de las mitad 
public class ValidChampionPorcientoPerdidas : IValidPlay<List<IGame<GameStatus>>, IPlayer, bool>
{
    public double Porcent { get; protected set; }

    public static string Description => "Si ha perdido mas de un x por ciento del total de juegos";

    public ValidChampionPorcientoPerdidas(double Porcent)
    {
        this.Porcent = Porcent;
    }
    public bool ValidPlay(List<IGame<GameStatus>> games, IPlayer player)
    {
        double cant = games.Count * Porcent;
        int count = 0;
        foreach (var game in games)
        {
            int x = game.Winner().IndexOf(player);
            if (x < game.Winner().Count / 2) count++;
            if (count < cant) return false;
        }

        return true;

    }
}



public class ValidChampionPerdidasConsecutivas : IValidPlay<List<IGame<GameStatus>>, IPlayer, bool>
{
    public double CantdeVecesConsecutivas { get; protected set; }

    public static string Description => "Si el jugador ha perdido una x cantidad de veces consecutivas";

    public ValidChampionPerdidasConsecutivas(double cantVeces)
    {

        this.CantdeVecesConsecutivas = cantVeces;

    }

    public bool ValidPlay(List<IGame<GameStatus>> games, IPlayer player)
    {
        if (CantdeVecesConsecutivas < 1) { return true; }
        int count = 0; int lastIndex = 0;
        for (int i = 0; i < games.Count; i++)
        {
            var game = games[i];
            game.Winner().Contains(player);
            if (lastIndex == 0 || i - lastIndex == 1)
            {
                count++;
            }
            else
            {
                lastIndex = 0;
            }
            if (lastIndex > this.CantdeVecesConsecutivas) return false;

        }

        return true;
    }
}




#endregion