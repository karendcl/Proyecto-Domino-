
namespace Game;

public abstract class WinCondition : IWinCondition<(Player player, List<IToken> hand), IToken>
{
    public static string Description => "Game WinCondition";

    public virtual List<Player> Winner(List<(Player player, List<IToken> hand)> players, IGetScore<IToken> howtogetscore)
    {
        //devuelve una lista de ganadores de la partida
        int count = 0;
        var result = new List<Player>();
        double[] scores = new double[players.Count];

        foreach (var (player, hand) in players)
        {
            if (hand.Count == 0)
            {
                result.Add(player);
            }

            foreach (var IToken in player.hand)
            {
                scores[count] += howtogetscore.Score(IToken);
            }
            count++;
        }

        if (result.Count != 0) return result;

        return FinalWinner(scores, players);
    }

    public abstract List<Player> FinalWinner(double[] scores, List<(Player player, List<IToken> hand)> players);
    //este es el metodo que va a cambiar



}
public class MinScore : WinCondition
{

    public static string Description => "Gana el jugador que tenga menos puntos";
    public override List<Player> FinalWinner(double[] scores, List<(Player player, List<IToken> hand)> players)
    {
        var result = new List<Player>();

        double score = scores.Min();

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
    public static string Description => "Gana el jugador que tenga mas puntos";
    public override List<Player> FinalWinner(double[] scores, List<(Player player, List<IToken> hand)> players)
    {
        var result = new List<Player>();

        double score = scores.Max();

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

public class MiddleScore : WinCondition
{
    public static string Description { get { return "Gana el jugador que tenga la media de puntos"; } }

    public override List<Player> FinalWinner(double[] scores, List<(Player player, List<IToken> hand)> players)
    {
        var result = new List<Player>();

        double[] temp = new double[scores.Length];
        temp = scores.ToArray();

        Array.Sort(temp);
        int mid = temp.Length / 2;
        double score = temp[mid];


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

public class WinChampion : IWinCondition<Game, List<IPlayerScore>>
{//En total de ganadas 

    public double Porcent { get; protected set; }
    protected List<WPlayer<Player>> players { get; set; }
    protected List<int> cantwins { get; set; }

    protected IGetScore<List<IPlayerScore>> howtogetscore { get; set; }
    protected virtual Dictionary<int, List<IPlayerScore>> playersScore { get; set; } = new Dictionary<int, List<IPlayerScore>>();
    public static string Description => "Gana el torneo, aquel jugador que haya ganado la mayor cantidad de veces";

    public WinChampion(double porcentWins)
    {
        this.players = new List<WPlayer<Player>>() { };
        this.cantwins = new List<int>() { };
        this.Porcent = porcentWins;
    }

    protected void Run(List<Game> games)
    {
        foreach (var game in games)
        {
            foreach (var playersScore in game.PlayerScores())
            {
                int id = playersScore.PlayerId;
                if (!this.playersScore.ContainsKey(id))
                {
                    this.playersScore.TryAdd(id, new List<IPlayerScore>() { playersScore });
                }
                else
                {
                    this.playersScore[id].Add(playersScore);
                }
            }
        }
    }
    public List<Player> Winner(List<Game> games, IGetScore<List<IPlayerScore>> howtogetscore)
    {
        this.howtogetscore = howtogetscore;
        Run(games);                                 ///Agregar una que utiize esta 
        List<Player> winners = new List<Player>() { };
        foreach (var game in games)
        {
            winners = game.Winner();

            for (int i = 0; i < winners.Count; i++)
            {
                Player player = winners[i];

                WPlayer<Player> temp = new WPlayer<Player>(player, 1);
                if (!players.Contains(temp)) { players.Add(temp); }
                else { int x = players.IndexOf(temp); players[x].AddScore(temp.Puntuation); }
            }
        }
        players.Sort(new WPlayer_Comparer());




        List<Player> list = new List<Player>() { };
        foreach (var player in players)
        {
            if (Check(player.player.Id))
            {
                list.Add(player.player);
            }
        }

        return list;
    }


    protected virtual bool Check(int playerId)
    {
        var x = this.playersScore[playerId];
        double score = this.howtogetscore.Score(x);
        if (score < 0) return false;
        return true;
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