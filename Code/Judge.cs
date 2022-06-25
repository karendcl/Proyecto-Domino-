namespace Game;

public class Judge
{
    protected virtual IStopGame<IPlayer, Token> stopcriteria { get; set; }
    protected virtual IGetScore<Token> howtogetscore { get; set; }


    protected virtual IWinCondition<(IPlayer player, List<Token> hand), Token> winCondition { get; set; }
    protected virtual IValidPlay<IBoard, Token, ChooseStrategyWrapped> valid { get; set; }

    // public WatchPlayer watchPlayer { get; private set; }
    protected IPlayer playernow { get; set; } = null!;
    protected List<ChooseStrategyWrapped> validTokenFornow;
    //Guarda las fichas que son validas en un momento x
    public Judge(IStopGame<IPlayer, Token> stop, IGetScore<Token> getscore, IWinCondition<(IPlayer player, List<Token> hand), Token> winCondition, IValidPlay<IBoard, Token, ChooseStrategyWrapped> valid)
    {
        this.stopcriteria = stop;
        this.howtogetscore = getscore;
        this.winCondition = winCondition;
        this.valid = valid;
        this.validTokenFornow = new List<ChooseStrategyWrapped>() { };
    }
    public virtual bool ValidSettings(int TokensForEach, int MaxDoble, int players)
    {
        int totalamount = 0;

        if (TokensForEach == 0 || MaxDoble == 0 || players == 0) return false;

        for (int i = 0; i <= MaxDoble + 1; i++)
        {
            totalamount += i;
        }

        return (TokensForEach * players > totalamount) ? false : true;
    }
    public WatchPlayer RunWatchPlayer(IBoard board)
    {
        return new WatchPlayer(this.howtogetscore, this.stopcriteria, this.valid, this.winCondition, board);
    }
    public bool PlayerMeetsStopCriteria(IPlayer player)
    {
        return this.stopcriteria.MeetsCriteria(player, this.howtogetscore);
    }
    public virtual ChooseStrategyWrapped ValidPlay(IPlayer player, IBoard board, Token token)
    {
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

    public virtual bool EndGame(List<(IPlayer, List<Token>)> players, IBoard board)
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


    public int PlayerScore(IPlayer player)
    {
        int result = 0;

        foreach (var token in player.hand)
        {
            result += this.howtogetscore.Score(token);
        }
        player.TotalScore += result;

        return result;
    }

    // -1,1
    private bool CheckHand(IPlayer player, GamePlayerHand<Token> hand, Token token)
    {
        if (player.hand.Count != hand.hand.Count) { return false; }

        if (player.hand.Contains(token) && hand.ContainsToken(token)) { return true; }

        return false;

    }

    public virtual bool AddTokenToBoard(IPlayer player, GamePlayerHand<Token> hand, Token token, IBoard board, int side)
    {
        ChooseStrategyWrapped x = new ChooseStrategyWrapped(board, token);
        int index = validTokenFornow.IndexOf(x);
        if (!CheckHand(player, hand, token)) { return false; }
        if (player.hand.Count != hand.hand.Count) { return false;  /*Añadir a descalificacion*/}

        if (board.board.Count == 0)//Poner aca los no descalificados
        {
            validTokenFornow.Clear();
            hand.AddLastPlay(token);
            board.board.Add(token);

            return true;
        }

        if (index < 0 || !player.Equals(this.playernow)) { validTokenFornow.Clear(); return false; }

        ChooseStrategyWrapped temp = validTokenFornow[index];

        validTokenFornow.Clear();


        (bool Bool, ChooseSideWrapped Elije) d = temp.ControlSide(side);
        if (!d.Bool) { return false; }

        Token first = board.First();
        Token last = board.Last();
        side = d.Elije.index;//Da el indice donde es Valido 
        if (side == 0) { PlayAlante(d.Elije, token, first, board); } else { PlayAtras(d.Elije, token, last, board); }
        hand.AddLastPlay(token);// Se le quita de la mano

        Diagnostics diagnostics = new Diagnostics();

        bool xxx = diagnostics.TestGame(board);
        if (!xxx)
        {
            System.Console.WriteLine(side);
        }

        return true;

    }

    public List<IPlayer> Winner(List<(IPlayer player, List<Token> hand)> players)
    {
        List<(IPlayer player, List<Token> hand)> temp = new List<(IPlayer player, List<Token> hand)>() { };
        foreach (var (player, hand) in players)
        {
            if (ItsThePlayerHand(player, hand)) { temp.Add((player, hand)); }
        }
        if (temp.Count < 1) { return null!; }
        return this.winCondition.Winner(temp, this.howtogetscore);
    }

    private bool ItsThePlayerHand(IPlayer player, List<Token> hand)
    {
        List<Token> phand = player.hand;
        if (hand.Count != player.hand.Count) { return false; }
        for (int i = 0; i < hand.Count; i++)
        {
            if (!phand[i].Equals(hand[i])) { return false; }
        }
        return true;
    }



    public void PlayAlante(ChooseSideWrapped where, Token token, Token first, IBoard board)
    {

        if (where.WhereCanMacht.Contains(1)) { token.SwapToken(); }


        board.board.Insert(0, token);
    }

    public void PlayAtras(ChooseSideWrapped where, Token token, Token last, IBoard board)
    {
        if (where.WhereCanMacht.Contains(1)) { token.SwapToken(); }
        board.board.Add(token);

    }




}

public class ControlPlayer
{
    public IPlayer player { get; private set; }

    public List<HistorialPlayer> TokenToPlayNow { get; private set; }

    public bool CanPlay { get; private set; } = false;

    public ControlPlayer(IPlayer player)
    {
        this.player = player;
        this.TokenToPlayNow = new List<HistorialPlayer>() { };

    }

    public class HistorialPlayer
    {

    }

}

#region  Champion


public class ChampionJudge
{
    public IStopGame<Game, (Game, IPlayer)> stopcriteria { get; set; }
    public IWinCondition<Game, IPlayer> winCondition { get; set; }
    public IValidPlay<Game, IPlayer, List<IPlayer>> valid { get; set; }
    public IGetScore<(Game, IPlayer)> howtogetscore { get; set; }

    public ChampionJudge(IStopGame<Game, (Game, IPlayer)> stopcriteria, IWinCondition<Game, IPlayer> winCondition, IValidPlay<Game, IPlayer, List<IPlayer>> valid, IGetScore<(Game, IPlayer)> howtogetscore)
    {
        this.howtogetscore = howtogetscore;
        this.stopcriteria = stopcriteria;
        this.valid = valid;
        this.winCondition = winCondition;
    }
    public bool EndGame(Game game)
    {
        if (stopcriteria.MeetsCriteria(game, howtogetscore)) return true;
        return false;
    }
    //Verificar antes de comenzar otro juego 
    public List<IPlayer> ValidPlay(Game game, IPlayer player)//Los que continuan
    {
        return valid.ValidPlay(game, player);
    }

    public virtual bool ValidSettings(int TokensForEach, int MaxDoble, int players)
    {
        int totalamount = 0;

        if (TokensForEach == 0 || MaxDoble == 0 || players == 0) return false;

        for (int i = 0; i <= MaxDoble + 1; i++)
        {
            totalamount += i;
        }

        return (TokensForEach * players > totalamount) ? false : true;
    }
}


#endregion