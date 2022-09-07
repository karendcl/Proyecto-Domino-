namespace Game;

#region Champion
// Si ha perdido mas de las mitad 
public class ValidChampionPorcientoPerdidas : IValidPlay<List<IGame<GameStatus>>, IPlayer, bool>
{
    public double Porcent { get; protected set; }

    public static string Description => "Si ha perdido mas de un x por ciento del total de juegos";

    public ValidChampionPorcientoPerdidas(double Porcent)
    {
        this.Porcent = Porcent / 100;
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
        int count = 0;
        bool Won = false;

        foreach (var game in games)
        {
            Won = false;
            foreach (var play in game.Winner())
            {
                if (play.Id == player.Id){
                    count =0;
                    Won = true;
                }
            }
            if (!Won) count += 1;
            if (count == this.CantdeVecesConsecutivas) return false; 
        }
        return true;
        
    }
}




#endregion