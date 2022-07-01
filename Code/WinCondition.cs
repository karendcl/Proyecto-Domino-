
namespace Game;

public abstract class WinCondition : IWinCondition<(Player player, List<Token> hand), Token>
{
    public virtual string Description => "Game WinCondition";

    public virtual List<Player> Winner(List<(Player player, List<Token> hand)> players, IGetScore<Token> howtogetscore)
    {

        int count = 0;
        var result = new List<Player>();
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

    public abstract List<Player> FinalWinner(int[] scores, List<(Player player, List<Token> hand)> players);




}
public class MinScore : WinCondition
{

    public override string Description => "MinScore";
    public override List<Player> FinalWinner(int[] scores, List<(Player player, List<Token> hand)> players)
    {
        var result = new List<Player>();

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
    public override string Description => "MaxScore";
    public override List<Player> FinalWinner(int[] scores, List<(Player player, List<Token> hand)> players)
    {
        var result = new List<Player>();

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
    public override string Description => "Specificscore";
    int score { get; set; }
    public Specificscore(int score)
    {
        this.score = score;
    }

    public override List<Player> FinalWinner(int[] scores, List<(Player player, List<Token> hand)> players)
    {
        var result = new List<Player>();

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

public class WinChampion : IWinCondition<Game, (Game, Player)>
{//En total de ganadas 

    public double Porcent { get; protected set; }
    public List<WPlayer<Player>> players { get; protected set; }
    public List<int> cantwins { get; protected set; }

    public string Description => " Champion Win Condition Jugde";

    public WinChampion(double porcentWins)
    {
        this.players = new List<WPlayer<Player>>() { };
        this.cantwins = new List<int>() { };
        this.Porcent = porcentWins;
    }
    public List<Player> Winner(List<Game> games, IGetScore<(Game, Player)> howtogetscore)
    {                                              ///Agregar una que utiize esta 
        List<Player> winners = new List<Player>() { };
        foreach (var game in games)
        {
            winners = game.Winner();

            for (int i = 0; i < winners.Count; i++)
            {
                WPlayer<Player> temp = new WPlayer<Player>(winners[i], 1);
                if (!players.Contains(temp)) { players.Add(temp); }
                else { int x = players.IndexOf(temp); players[x].AddScore(temp.Puntuation); }
            }
        }
        players.Sort(new WPlayer_Comparer());




        List<Player> list = new List<Player>() { };
        for (int i = 0; i < players.Count; i++)
        {
            list.Add(players[i].player);
        }

        return list;
    }





    public class WPlayer<T> : IEquatable<WPlayer<T>> where T : IEquatable<T>
    {
        public virtual T player { get; protected set; }
        public virtual int Puntuation { get; protected set; }

        public WPlayer(T player, int Puntuation)
        {
            this.player = player;
            this.Puntuation = Puntuation;
        }


        public virtual void AddScore(int score)
        {
            this.Puntuation += score;
        }
        public virtual bool Equals(WPlayer<T>? other)
        {
            if (other == null || this == null) { return false; }
            return this.player.Equals(other.player);
        }
    }



    protected internal class WPlayer_Comparer : IComparer<WPlayer<Player>>
    {
        public int Compare(WPlayer<Player>? x, WPlayer<Player>? y)
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