namespace Game;


#region Game

public class Judge : IDescriptible, IJudgeGame
{
    #region Global
    protected virtual IStopGame<IPlayer, IToken> stopcriteria { get; set; }
    protected virtual IGetScore<IToken> howtogetscore { get; set; }
    protected virtual IWinCondition<(IPlayer player, List<IToken> hand), IToken> winCondition { get; set; }
    protected virtual IValidPlay<IBoard, IToken, IChooseStrategyWrapped> valid { get; set; }
    protected virtual IPlayer playernow { get; set; } = null!;
    protected virtual List<IChooseStrategyWrapped> validTokenFornow { get; set; }

    protected virtual Dictionary<int, IPlayerScore> playerScores { get; set; } = new Dictionary<int, IPlayerScore>();

    protected virtual CalculatePlayerScore getPlayerScore { get; set; } = new CalculatePlayerScore();
    public static string Description => "Juez Honesto para el juego";

    #endregion



    //Guarda las fichas que son validas en un momento x
    public Judge(IStopGame<IPlayer, IToken> stop, IGetScore<IToken> getscore, IWinCondition<(IPlayer player, List<IToken> hand), IToken> winCondition, IValidPlay<IBoard, IToken, IChooseStrategyWrapped> valid)
    {
        this.stopcriteria = stop;
        this.howtogetscore = getscore;
        this.winCondition = winCondition;
        this.valid = valid;
        this.validTokenFornow = new List<IChooseStrategyWrapped>() { };
    }



    public virtual IWatchPlayer RunWatchPlayer(IBoard board)
    {

        return new WatchPlayer(this.howtogetscore, this.stopcriteria, this.valid, this.winCondition, board);
    }
    protected virtual bool PlayerMeetsStopCriteria(IPlayer player)  // True si el jugador cumple con las condiciones de parada 
    {

        return this.stopcriteria.MeetsCriteria(player, this.howtogetscore);
    }
    public virtual IChooseStrategyWrapped ValidPlay(IPlayer player, IBoard board, IToken token)  //Aca se devuelve un Wrapped 
    // que dicta la validez o no de poner la ficha en el tablero
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
        IChooseStrategyWrapped valid = this.valid.ValidPlay(board, token);

        if (valid.CanMatch) { validTokenFornow.Add(valid); }

        //Implemetar aca las descalificaciones
        return valid;
    }

    protected void AddPlayerScore(IPlayer player)  //Se añade el jugador al dicc que contiene su score
    {
        if (!playerScores.ContainsKey(player.Id))
        {
            playerScores.Add(player.Id, new PlayerScore(player.Id));
        }
    }
    public virtual bool EndGame(List<(IPlayer, List<IToken>)> players, IBoard board)// True si se cumple la condicion de parada
    {

        foreach (var (player, tokens) in players)
        {
            if (PlayerMeetsStopCriteria(player)) return true;
        }

        foreach (var (player, hand) in players)
        {
            foreach (var token in hand)
            {
                IChooseStrategyWrapped temp = this.valid.ValidPlay(board, token);
                if (temp.CanMatch || temp.FirstPlay) return false;
            }
        }// si no esta trancado

        return true; //esta trancado

    }


    public virtual double PlayerScore(IPlayer player)  //Devuelve el score de un player
    {
        if (!this.playerScores.ContainsKey(player.Id)) return -1;
        IPlayerScore score = this.playerScores[player.Id];
        return score.Score;
    }

    public virtual List<IPlayerScore> PlayersScores()  //Devuelve el score de todos los players
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
    protected virtual bool CheckHand(IPlayer player, GamePlayerHand<IToken> hand, IToken token)  //Comprueba si la mano /
    // contiene el token que se desea jugar
    {
        if (player.hand.Count != hand.hand.Count) { return false; }

        if (player.hand.Contains(token) && hand.ContainsToken(token)) { return true; }

        return false;

    }

    public virtual bool AddTokenToBoard(IPlayer player, GamePlayerHand<IToken> hand, IToken token, IBoard board, int side)
    {
        var x = new ChooseStrategyWrapped(board, token); //Se crea un wrapped para buscar en la lista

        int index = validTokenFornow.IndexOf(x);//Se comprueba que ya fue hecho el pedido de poner dicha ficha

        if (!CheckHand(player, hand, token)) //Se comprueba que la mano contenga la ficha
        {

            this.AddPlayerScoreStatus(player.Id, hand, token, false);// Como no se ´puede poner la ficha se le quita el score e esta
            validTokenFornow.Clear();
            return false;
        }
        if (player.hand.Count != hand.hand.Count)
        {
            this.AddPlayerScoreStatus(player.Id, hand, token, false);

            return false;  /*Añadir a descalificacion*/
        }

        if (board.board.Count == 0)//Poner aca los no descalificados
        {
            validTokenFornow.Clear();// Se limpia las ultimas propuestas
            hand.AddLastPlay(token); //Añade la ultima ficha puesta
            board.AddTokenToBoard(token, 0);  //Añade al board la ficha
            this.AddPlayerScoreStatus(player.Id, hand, token, true);// Se le añade el score al jugador

            return true;
        }

        if (index < 0 || !player.Equals(this.playernow)) { validTokenFornow.Clear(); return false; }

        IChooseStrategyWrapped temp = validTokenFornow[index];

        validTokenFornow.Clear();


        (bool Bool, ChooseSideWrapped Elije) d = temp.ControlSide(side); //Se controla que este entre los lugares posibles a poner
        if (!d.Bool) { return false; }

        IToken first = board.First;
        IToken last = board.Last;
        side = d.Elije.index;//Da el indice donde es Valido 
        if (side == 0) { PlayFront(d.Elije, token, first, board); } else { PlayBack(d.Elije, token, last, board); }
        hand.AddLastPlay(token);// Se le quita de la mano

        this.AddPlayerScoreStatus(player.Id, hand, token, true);

        return true;

    }

    protected virtual void PlayFront(IChooseSideWrapped where, IToken token, IToken first, IBoard board)
    {

        if (where.WhereCanMacht.Contains(1)) { token.SwapToken(); }


        board.AddTokenToBoard(token, 0);
    }

    protected virtual void PlayBack(IChooseSideWrapped where, IToken token, IToken last, IBoard board)
    {
        if (where.WhereCanMacht.Contains(1)) { token.SwapToken(); }
        board.AddTokenToBoard(token, 1);

    }


    protected virtual void AddPlayerScoreStatus(int id, GamePlayerHand<IToken> hand, IToken token, bool Add)
    {
        double x = this.howtogetscore.Score(token);
        IPlayerScore score = this.playerScores[id];
        this.getPlayerScore.AddPlay(score, hand, x, Add);
    }

    public virtual List<IPlayer> Winner(List<(IPlayer player, List<IToken> hand)> players)
    {
        List<(IPlayer player, List<IToken> hand)> temp = new List<(IPlayer player, List<IToken> hand)>() { };
        foreach (var (player, hand) in players)
        {
            if (ItsThePlayerHand(player, hand)) { temp.Add((player, hand)); }
        }
        if (temp.Count < 1) { return new List<IPlayer>() { }; }
        return this.winCondition.Winner(temp, this.howtogetscore);
    }

    protected virtual bool ItsThePlayerHand(IPlayer player, List<IToken> hand)//Cheque que esa sea la mano del jugador
    {
        List<IToken> phand = player.hand;
        if (hand.Count != player.hand.Count) { return false; }
        for (int i = 0; i < hand.Count; i++)
        {
            if (!phand[i].Equals(hand[i])) { return false; }
        }
        return true;
    }




}


public class CorruptionJugde : Judge, IJudgeGame
{
    public static string Description => "Juez Corrupto para el juego";
    protected Random random { get; set; }
    public CorruptionJugde(IStopGame<IPlayer, IToken> stop, IGetScore<IToken> getscore, IWinCondition<(IPlayer player, List<IToken> hand), IToken> winCondition, IValidPlay<IBoard, IToken, IChooseStrategyWrapped> valid) : base(stop, getscore, winCondition, valid)
    {
        this.random = new Random();

    }

    public bool MakeCorruption() //Decide si se debe o no realizar la corrupcion
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
    public override bool AddTokenToBoard(IPlayer player, GamePlayerHand<IToken> hand, IToken token, IBoard board, int side)
    {
        bool x = (player.GetType() == typeof(CorruptionPlayer)); //Comprueba si es corruptible

        bool cant = base.ValidPlay(player, board, token).CanMatch; //Comprueba si no se puede jugar ese token
        if (MakeCorruption() && !cant && x)
        {
            double score = this.PlayerScore(player) / 3; //Le hace una oferta de quitarle 1/3 del score
            CorruptionPlayer temp = (CorruptionPlayer)player;
            if (temp.Corrupt(score)) // si acepta
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


public class ChampionJudge : IDescriptible, IChampionJudge<GameStatus>
{
    protected virtual IStopGame<List<IGame<GameStatus>>, List<IPlayerScore>> stopcriteria { get; set; }
    protected virtual IWinCondition<IGame<GameStatus>, List<IPlayerScore>> winCondition { get; set; }
    protected virtual IValidPlay<List<IGame<GameStatus>>, IPlayer, bool> valid { get; set; }
    protected virtual IGetScore<List<IPlayerScore>> howtogetscore { get; set; }
    protected virtual CalculateChampionScore getPlayerScore { get; set; }
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
    public static string Description => "Juez Corrupto para el Torneo";
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