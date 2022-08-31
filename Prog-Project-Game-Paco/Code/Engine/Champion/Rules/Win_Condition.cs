namespace Game;

#region  Champion

public class WinChampion : IWinCondition<IGame<GameStatus>, List<IPlayerScore>>
{//En total de ganadas 

    public double Porcent { get; protected set; }
    protected List<WPlayer<IPlayer>> players { get; set; }
    protected List<int> cantwins { get; set; }

    protected IGetScore<List<IPlayerScore>> howtogetscore { get; set; } = new ScoreChampionNormal();
    protected virtual Dictionary<int, List<IPlayerScore>> playersScore { get; set; } = new Dictionary<int, List<IPlayerScore>>();
    public static string Description => "Gana el torneo, aquel jugador que haya ganado la mayor cantidad de veces";

    public WinChampion(double porcentWins)
    {
        this.players = new List<WPlayer<IPlayer>>() { };
        this.cantwins = new List<int>() { };
        this.Porcent = porcentWins;
    }

    protected void Run(List<IGame<GameStatus>> games)
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
    public List<IPlayer> Winner(List<IGame<GameStatus>> games, IGetScore<List<IPlayerScore>> howtogetscore)
    {
        this.howtogetscore = howtogetscore;
        Run(games);                                 ///Agregar una que utiize esta 
        var winners = new List<IPlayer>() { };
        foreach (var game in games)
        {
            winners = game.Winner();

            for (int i = 0; i < winners.Count; i++)
            {
                var player = winners[i];

                WPlayer<IPlayer> temp = new WPlayer<IPlayer>(player, 1);
                if (!players.Contains(temp)) { players.Add(temp); }
                else { int x = players.IndexOf(temp); players[x].AddScore(temp.Puntuation); }
            }
        }
        players.Sort(new WPlayer_Comparer());




        var list = new List<IPlayer>() { };
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



    protected internal class WPlayer_Comparer : IComparer<WPlayer<IPlayer>>
    {
        public int Compare(WPlayer<IPlayer>? x, WPlayer<IPlayer>? y)
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