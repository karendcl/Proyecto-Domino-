namespace Game;



#region  Champion


public class ChampionJudge : IDescriptible, IChampionJudge<GameStatus>
{
    protected virtual IStopGame<List<IGame<GameStatus>>, List<IPlayerScore>> stopcriteria { get; set; }
    protected virtual IWinCondition<IGame<GameStatus>, List<IPlayerScore>> winCondition { get; set; }
    protected virtual IValidPlay<List<IGame<GameStatus>>, IPlayer, bool> valid { get; set; }
    protected virtual IGetScore<List<IPlayerScore>> howtogetscore { get; set; }
    protected virtual CalculateChampionScore getPlayerScore { get; set; } = new CalculateChampionScore(new List<int>());
    protected virtual List<IGame<GameStatus>> finishedGames { get; set; } = new List<IGame<GameStatus>>();

    protected virtual List<int> playersId { get; set; } = new List<int>();

    public static string Description => "Juez Honesto para el torneo";

    public ChampionJudge(IStopGame<List<IGame<GameStatus>>, List<IPlayerScore>> stopcriteria, IWinCondition<IGame<GameStatus>, List<IPlayerScore>> winCondition, IValidPlay<List<IGame<GameStatus>>, IPlayer, bool> valid, IGetScore<List<IPlayerScore>> howtogetscore)
    {
        this.howtogetscore = howtogetscore;
        this.stopcriteria = stopcriteria;
        this.valid = valid;
        this.winCondition = winCondition;

    }

    public virtual void Run(List<IPlayer> players)
    {
        foreach (var item in players)
        {
            playersId.Add(item.Id);

        }
        this.getPlayerScore = new CalculateChampionScore(this.playersId);

    }
    public virtual bool EndGame(List<IGame<GameStatus>> game)
    {
        if (stopcriteria.MeetsCriteria(game, howtogetscore)) return true;
        return false;
    }
    //Verificar antes de comenzar otro juego 

    public virtual void AddFinishGame(IGame<GameStatus> game)
    {
        finishedGames.Add(game);
        foreach (var item in game.PlayerScores())
        {
            this.getPlayerScore.AddPlayerScore(item.PlayerId, item);
        }
    }


    public virtual bool ValidPlay(IPlayer player)//Los que continuan
    {
        return valid.ValidPlay(this.finishedGames, player);
    }

    public virtual List<IPlayer> Winners()
    {
        return this.winCondition.Winner(this.finishedGames, this.howtogetscore);
    }

    public double PlayerScore(int playerId)
    {
        if (this.playersId.Contains(playerId)) return double.MinValue;
        var x = this.getPlayerScore.GetPlayerScore(playerId);
        return this.howtogetscore.Score(x);
    }

}



public class CorruptionChampionJugde : ChampionJudge, IChampionJudge<GameStatus>
{
    public static new string Description => "Juez Corrupto para el Torneo";
    public CorruptionChampionJugde(IStopGame<List<IGame<GameStatus>>, List<IPlayerScore>> stopcriteria, IWinCondition<IGame<GameStatus>, List<IPlayerScore>> winCondition, IValidPlay<List<IGame<GameStatus>>, IPlayer, bool> valid, IGetScore<List<IPlayerScore>> howtogetscore) : base(stopcriteria, winCondition, valid, howtogetscore)
    {
    }

    public bool MakeCorruption()
    {
        Random random = new Random();
        int x = random.Next(0, 100);
        if (x > 50)
        {
            return true;
        }
        return false;

    }

    public override bool EndGame(List<IGame<GameStatus>> game)
    {
        if (MakeCorruption())
        {
            return !base.EndGame(game);
        }
        return base.EndGame(game);
    }

    public override bool ValidPlay(IPlayer player)
    {
        bool x = (player.GetType() == typeof(CorruptionPlayer));
        if (MakeCorruption() && x)
        {
            double ofert = this.getPlayerScore.GetScore(player.Id) / 2;
            if (ofert > 0)
            {
                CorruptionPlayer temp = (CorruptionPlayer)player;
                if (temp.Corrupt(ofert))
                {
                    return true;
                }
            }

        }

        return base.ValidPlay(player);
    }


}

#endregion