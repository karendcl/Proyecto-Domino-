namespace Game;


#region Game

public class Judge : IDescriptible, IJudgeGame
{
    #region Global
    protected virtual IStopGame<Player, IToken> stopcriteria { get; set; }
    protected virtual IGetScore<IToken> howtogetscore { get; set; }
    protected virtual IWinCondition<(Player player, List<IToken> hand), IToken> winCondition { get; set; }
    protected virtual IValidPlay<Board, IToken, ChooseStrategyWrapped> valid { get; set; }
    protected virtual Player playernow { get; set; } = null!;
    protected virtual List<ChooseStrategyWrapped> validTokenFornow { get; set; }

    protected virtual Dictionary<int, IPlayerScore> playerScores { get; set; } = new Dictionary<int, IPlayerScore>();

    protected virtual CalculatePlayerScore getPlayerScore { get; set; } = new CalculatePlayerScore();
    public static string Description => "Juez Honesto para el juego";

    #endregion



    //Guarda las fichas que son validas en un momento x
    public Judge(IStopGame<Player, IToken> stop, IGetScore<IToken> getscore, IWinCondition<(Player player, List<IToken> hand), IToken> winCondition, IValidPlay<Board, IToken, ChooseStrategyWrapped> valid)
    {
        this.stopcriteria = stop;
        this.howtogetscore = getscore;
        this.winCondition = winCondition;
        this.valid = valid;
        this.validTokenFornow = new List<ChooseStrategyWrapped>() { };
    }



    public virtual WatchPlayer RunWatchPlayer(Board board)
    {

        return new WatchPlayer(this.howtogetscore, this.stopcriteria, this.valid, this.winCondition, board);
    }
    protected virtual bool PlayerMeetsStopCriteria(Player player)
    {

        return this.stopcriteria.MeetsCriteria(player, this.howtogetscore);
    }
    public virtual ChooseStrategyWrapped ValidPlay(Player player, Board board, IToken token)
    {
        AddPlayerScore(player);
        if (token == null)
        {
            return null!;
        }

        playernow = player;
        if (playernow == null)
        {
            playernow = player;
        }
        else if (!player.Equals(playernow))
        {
            playernow = player;
            validTokenFornow.Clear();
        }
        ChooseStrategyWrapped valid = this.valid.ValidPlay(board, token);

        if (valid.CanMatch) { validTokenFornow.Add(valid); }

        //Implemetar aca las descalificaciones
        return valid;
    }

    protected void AddPlayerScore(Player player)
    {
        if (!playerScores.ContainsKey(player.Id))
        {
            playerScores.Add(player.Id, new PlayerScore(player.Id));
        }
    }
    public virtual bool EndGame(List<(Player, List<IToken>)> players, Board board)
    {

        foreach (var (player, tokens) in players)
        {
            if (PlayerMeetsStopCriteria(player)) return true;
        }

        foreach (var (player, hand) in players)
        {
            foreach (var token in hand)
            {
                ChooseStrategyWrapped temp = this.valid.ValidPlay(board, token);
                if (temp.CanMatch || temp.FirstPlay) return false;
            }
        }// si no esta trancado

        return true; //esta trancado

    }


    public virtual double PlayerScore(Player player)
    {
        if (!this.playerScores.ContainsKey(player.Id)) return -1;
        IPlayerScore score = this.playerScores[player.Id];
        return score.Score;
    }

    public virtual List<IPlayerScore> PlayersScores()
    {
        List<IPlayerScore> result = new List<IPlayerScore>();

        foreach (var item in this.playerScores.Values)
        {
            IPlayerScore temp = item;
            result.Add(temp.Clone());
        }
        return result;
    }

    // -1,1
    protected virtual bool CheckHand(Player player, GamePlayerHand<IToken> hand, IToken token)
    {
        if (player.hand.Count != hand.hand.Count) { return false; }

        if (player.hand.Contains(token) && hand.ContainsToken(token)) { return true; }

        return false;

    }

    public virtual bool AddTokenToBoard(Player player, GamePlayerHand<IToken> hand, IToken token, Board board, int side)
    {
        ChooseStrategyWrapped x = new ChooseStrategyWrapped(board, token);

        int index = validTokenFornow.IndexOf(x);

        if (!CheckHand(player, hand, token))
        {
            this.AddPlayerScoreStatus(player.Id, hand, token, true);

            return false;
        }
        if (player.hand.Count != hand.hand.Count)
        {
            this.AddPlayerScoreStatus(player.Id, hand, token, false);

            return false;  /*Añadir a descalificacion*/
        }

        if (board.board.Count == 0)//Poner aca los no descalificados
        {
            validTokenFornow.Clear();
            hand.AddLastPlay(token);
            board.board.Add(token);
            this.AddPlayerScoreStatus(player.Id, hand, token, true);

            return true;
        }

        if (index < 0 || !player.Equals(this.playernow)) { validTokenFornow.Clear(); return false; }

        ChooseStrategyWrapped temp = validTokenFornow[index];

        validTokenFornow.Clear();


        (bool Bool, ChooseSideWrapped Elije) d = temp.ControlSide(side);
        if (!d.Bool) { return false; }

        IToken first = board.First;
        IToken last = board.Last;
        side = d.Elije.index;//Da el indice donde es Valido 
        if (side == 0) { PlayFront(d.Elije, token, first, board); } else { PlayBack(d.Elije, token, last, board); }
        hand.AddLastPlay(token);// Se le quita de la mano

        this.AddPlayerScoreStatus(player.Id, hand, token, true);

        return true;

    }

    protected virtual void AddPlayerScoreStatus(int id, GamePlayerHand<IToken> hand, IToken token, bool Add)
    {
        double x = this.howtogetscore.Score(token);
        IPlayerScore score = this.playerScores[id];
        this.getPlayerScore.AddPlay(score, hand, x, Add);
    }

    public virtual List<Player> Winner(List<(Player player, List<IToken> hand)> players)
    {
        List<(Player player, List<IToken> hand)> temp = new List<(Player player, List<IToken> hand)>() { };
        foreach (var (player, hand) in players)
        {
            if (ItsThePlayerHand(player, hand)) { temp.Add((player, hand)); }
        }
        if (temp.Count < 1) { return new List<Player>() { }; }
        return this.winCondition.Winner(temp, this.howtogetscore);
    }

    protected virtual bool ItsThePlayerHand(Player player, List<IToken> hand)
    {
        List<IToken> phand = player.hand;
        if (hand.Count != player.hand.Count) { return false; }
        for (int i = 0; i < hand.Count; i++)
        {
            if (!phand[i].Equals(hand[i])) { return false; }
        }
        return true;
    }

    protected virtual void PlayFront(ChooseSideWrapped where, IToken token, IToken first, Board board)
    {

        if (where.WhereCanMacht.Contains(1)) { token.SwapToken(); }


        board.board.Insert(0, token);
    }

    protected virtual void PlayBack(ChooseSideWrapped where, IToken token, IToken last, Board board)
    {
        if (where.WhereCanMacht.Contains(1)) { token.SwapToken(); }
        board.board.Add(token);

    }



}


public class CorruptionJugde : Judge
{
    public static string Description => "Juez Corrupto para el juego";
    protected Random random { get; set; }
    public CorruptionJugde(IStopGame<Player, IToken> stop, IGetScore<IToken> getscore, IWinCondition<(Player player, List<IToken> hand), IToken> winCondition, IValidPlay<Board, IToken, ChooseStrategyWrapped> valid) : base(stop, getscore, winCondition, valid)
    {
        this.random = new Random();

    }

    public bool MakeCorruption()
    {
        int x = random.Next(0, 100);
        if (x > 50)
        {
            return true;
        }
        return false;

    }
    protected virtual void LessPlayerScore(int playerID, double score)
    {
        IPlayerScore score1 = this.playerScores[playerID];
        score1.LessScore(score);
    }
    public override bool AddTokenToBoard(Player player, GamePlayerHand<IToken> hand, IToken token, Board board, int side)
    {
        bool x = (player is ICorruptible);
        bool cant = base.ValidPlay(player, board, token).CanMatch;
        if (MakeCorruption() && !cant && x)
        {
            double score = this.PlayerScore(player) / 3;
            ICorruptible temp = (ICorruptible)player;
            if (temp.Corrupt(score))
            {
                LessPlayerScore(player.Id, score);//Bajar el score del jugador

                if (random.Next(0, 27) > Math.E / 2) { PlayBack(null!, token, board.First, board); }
                else
                {
                    PlayFront(null!, token, board.Last, board);
                }
            }
        }
        return base.AddTokenToBoard(player, hand, token, board, side);
    }




}


#endregion


#region  Champion


public class ChampionJudge : IDescriptible, IChampionJudge
{
    protected virtual IStopGame<List<Game>, List<IPlayerScore>> stopcriteria { get; set; }
    protected virtual IWinCondition<Game, List<IPlayerScore>> winCondition { get; set; }
    protected virtual IValidPlay<List<Game>, Player, bool> valid { get; set; }
    protected virtual IGetScore<List<IPlayerScore>> howtogetscore { get; set; }
    protected virtual CalculateChampionScore getPlayerScore { get; set; }
    protected virtual List<Game> finishedGames { get; set; } = new List<Game>();

    protected virtual List<int> playersId { get; set; } = new List<int>();

    public static string Description => "Juez Honesto para el torneo";

    public ChampionJudge(IStopGame<List<Game>, List<IPlayerScore>> stopcriteria, IWinCondition<Game, List<IPlayerScore>> winCondition, IValidPlay<List<Game>, Player, bool> valid, IGetScore<List<IPlayerScore>> howtogetscore)
    {
        this.howtogetscore = howtogetscore;
        this.stopcriteria = stopcriteria;
        this.valid = valid;
        this.winCondition = winCondition;

    }

    public virtual void Run(List<Player> players)
    {
        foreach (var item in players)
        {
            playersId.Add(item.Id);

        }
        this.getPlayerScore = new CalculateChampionScore(this.playersId);

    }
    public virtual bool EndGame(List<Game> game)
    {
        if (stopcriteria.MeetsCriteria(game, howtogetscore)) return true;
        return false;
    }
    //Verificar antes de comenzar otro juego 

    public virtual void AddFinishGame(Game game)
    {
        finishedGames.Add(game);
        foreach (var item in game.PlayerScores())
        {
            this.getPlayerScore.AddPlayerScore(item.PlayerId, item);
        }
    }


    public virtual bool ValidPlay(Player player)//Los que continuan
    {
        return valid.ValidPlay(this.finishedGames, player);
    }

    public virtual List<Player> Winners()
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



public class CorruptionChampionJugde : ChampionJudge
{
    public static string Description => "Juez Corrupto para el Torneo";
    public CorruptionChampionJugde(IStopGame<List<Game>, List<IPlayerScore>> stopcriteria, IWinCondition<Game, List<IPlayerScore>> winCondition, IValidPlay<List<Game>, Player, bool> valid, IGetScore<List<IPlayerScore>> howtogetscore) : base(stopcriteria, winCondition, valid, howtogetscore)
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

    public override bool EndGame(List<Game> game)
    {
        if (MakeCorruption())
        {
            return !base.EndGame(game);
        }
        return base.EndGame(game);
    }

    public override bool ValidPlay(Player player)
    {
        bool x = (player is ICorruptible);
        if (MakeCorruption() && x)
        {
            double ofert = this.getPlayerScore.GetScore(player.Id) / 2;
            if (ofert > 0)
            {
                ICorruptible temp = (ICorruptible)player;
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