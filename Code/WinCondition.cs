
namespace Game;

public abstract class WinCondition : IWinCondition<(IPlayer player, List<Token> hand), Token>
{

    public virtual List<IPlayer> Winner(List<(IPlayer player, List<Token> hand)> players, IGetScore<Token> howtogetscore)
    {

        int count = 0;
        var result = new List<IPlayer>();
        int[] scores = new int[players.Count];

        foreach (var (player, hand) in players)
        {
            if (hand.Count == 0)
            {
                result.Add(player);
            }

            foreach (var Token in player.hand)
            {
                scores[count] += howtogetscore.Score(Token);
            }
            count++;
        }

        if (result.Count != 0) return result;

        return FinalWinner(scores, players);



    }

    public abstract List<IPlayer> FinalWinner(int[] scores, List<(IPlayer player, List<Token> hand)> players);




}
public class MinScore : WinCondition
{

    public override List<IPlayer> FinalWinner(int[] scores, List<(IPlayer player, List<Token> hand)> players)
    {
        var result = new List<IPlayer>();

        int score = scores.Min();

        for (int i = 0; i < scores.Length; i++)
        {
            if (scores[i] == score)
            {
                result.Add(players[i].player);
            }
        }

        return result;
    }
}

public class MaxScore : WinCondition
{

    public override List<IPlayer> FinalWinner(int[] scores, List<(IPlayer player, List<Token> hand)> players)
    {
        var result = new List<IPlayer>();

        int score = scores.Min();

        for (int i = 0; i < scores.Length; i++)
        {
            if (scores[i] == score)
            {
                result.Add(players[i].player);
            }
        }

        return result;
    }
}

public class Specificscore : WinCondition
{
    int score { get; set; }
    public Specificscore(int score)
    {
        this.score = score;
    }

    public override List<IPlayer> FinalWinner(int[] scores, List<(IPlayer player, List<Token> hand)> players)
    {
        var result = new List<IPlayer>();

        int score = scores.Min();

        for (int i = 0; i < scores.Length; i++)
        {
            if (scores[i] == score)
            {
                result.Add(players[i].player);
            }
        }

        return result;
    }
}

#region  Champion

public class WinChampion : IWinCondition<Game, IPlayer>
{// Espresar en fraccion el porcentaje de ganadas del total del torneo

    // public double Porcent { get; private set; }
    public List<WIPlayer<IPlayer>> players { get; private set; }
    public List<int> cantwins { get; private set; }
    public WinChampion(double porcentWins)
    {
        this.players = new List<WIPlayer<IPlayer>>() { };
        this.cantwins = new List<int>() { };
        // this.Porcent = porcentWins;
    }
    public List<IPlayer> Winner(List<Game> games, IGetScore<IPlayer> howtogetscore)
    {                                              ///Agregar una que utiize esta 
        List<IPlayer> winners = new List<IPlayer>() { };
        foreach (var game in games)
        {
            winners = game.Winner();

            for (int i = 0; i < winners.Count; i++)
            {
                WIPlayer<IPlayer> temp = new WIPlayer<IPlayer>(winners[i], i);
                if (!players.Contains(temp)) { players.Add(temp); }
                else { int x = players.IndexOf(temp); players[x].Puntuation += temp.Puntuation; }
            }
        }
        players.Sort(new WIPlayer_Comparer());

        players.Reverse();

        List<IPlayer> list = new List<IPlayer>() { };
        for (int i = 0; i < players.Count; i++)
        {
            list.Add(players[i].player);
        }

        return list;
    }

    public class WIPlayer<T> : IEquatable<WIPlayer<T>> where T : IEquatable<T>
    {
        public T player { get; private set; }
        public int Puntuation { get; set; }

        public WIPlayer(T player, int Puntuation)
        {
            this.player = player;
            this.Puntuation = Puntuation;
        }



        public bool Equals(WIPlayer<T>? other)
        {
            if (other == null || this == null) { return false; }
            return this.player.Equals(other.player);
        }
    }

    protected internal class WIPlayer_Comparer : IComparer<WIPlayer<IPlayer>>
    {
        public int Compare(WIPlayer<IPlayer>? x, WIPlayer<IPlayer>? y)
        {
            if (x == null || y == null) { throw new NullReferenceException("null"); }
            if (x?.Puntuation < y?.Puntuation)
            {
                return -1;
            }
            if (x?.Puntuation > y?.Puntuation)
            {
                return 1;
            }

            return 0;
        }
    }
}

#endregion