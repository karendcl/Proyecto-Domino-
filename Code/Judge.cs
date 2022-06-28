namespace Game;

public class Judge
{
    protected virtual IStopGame<IPlayer, Token> stopcriteria { get; set; }
    protected virtual IGetScore<Token> howtogetscore { get; set; }

    protected virtual IWinCondition<(IPlayer player, List<Token> hand), Token> winCondition { get; set; }
    protected virtual IValidPlay<IBoard, Token, ChooseStrategyWrapped> valid { get; set; }
    protected virtual IPlayer playernow { get; set; } = null!;
    protected virtual List<ChooseStrategyWrapped> validTokenFornow { get; set; }
    //Guarda las fichas que son validas en un momento x
    public Judge(IStopGame<IPlayer, Token> stop, IGetScore<Token> getscore, IWinCondition<(IPlayer player, List<Token> hand), Token> winCondition, IValidPlay<IBoard, Token, ChooseStrategyWrapped> valid)
    {
        this.stopcriteria = stop;
        this.howtogetscore = getscore;
        this.winCondition = winCondition;
        this.valid = valid;
        this.validTokenFornow = new List<ChooseStrategyWrapped>() { };
    }

    public virtual WatchPlayer RunWatchPlayer(IBoard board)
    {
        return new WatchPlayer(this.howtogetscore, this.stopcriteria, this.valid, this.winCondition, board);
    }
    protected virtual bool PlayerMeetsStopCriteria(IPlayer player)
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


    public virtual int PlayerScore(IPlayer player)
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
    protected virtual bool CheckHand(IPlayer player, GamePlayerHand<Token> hand, Token token)
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

        Token first = board.First;
        Token last = board.Last;
        side = d.Elije.index;//Da el indice donde es Valido 
        if (side == 0) { PlayAlante(d.Elije, token, first, board); } else { PlayAtras(d.Elije, token, last, board); }
        hand.AddLastPlay(token);// Se le quita de la mano



        return true;

    }

    public virtual List<IPlayer> Winner(List<(IPlayer player, List<Token> hand)> players)
    {
        List<(IPlayer player, List<Token> hand)> temp = new List<(IPlayer player, List<Token> hand)>() { };
        foreach (var (player, hand) in players)
        {
            if (ItsThePlayerHand(player, hand)) { temp.Add((player, hand)); }
        }
        if (temp.Count < 1) { return null!; }
        return this.winCondition.Winner(temp, this.howtogetscore);
    }

    protected virtual bool ItsThePlayerHand(IPlayer player, List<Token> hand)
    {
        List<Token> phand = player.hand;
        if (hand.Count != player.hand.Count) { return false; }
        for (int i = 0; i < hand.Count; i++)
        {
            if (!phand[i].Equals(hand[i])) { return false; }
        }
        return true;
    }



    public virtual void PlayAlante(ChooseSideWrapped where, Token token, Token first, IBoard board)
    {

        if (where.WhereCanMacht.Contains(1)) { token.SwapToken(); }


        board.AddTokenToBoard(token, 0);
    }

    public virtual void PlayAtras(ChooseSideWrapped where, Token token, Token last, IBoard board)
    {
        if (where.WhereCanMacht.Contains(1)) { token.SwapToken(); }
        board.AddTokenToBoard(token, 1);

    }




}


public class CorruptionJugde : Judge, ICorrupcion
{
    protected Random random { get; set; }
    public CorruptionJugde(IStopGame<IPlayer, Token> stop, IGetScore<Token> getscore, IWinCondition<(IPlayer player, List<Token> hand), Token> winCondition, IValidPlay<IBoard, Token, ChooseStrategyWrapped> valid) : base(stop, getscore, winCondition, valid)
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
    public override bool AddTokenToBoard(IPlayer player, GamePlayerHand<Token> hand, Token token, IBoard board, int side)
    {
        if (MakeCorruption())
        {
            if (random.Next(0, 27) > Math.E / 2) { PlayAlante(null!, token, board.First, board); }
            else
            {
                PlayAtras(null!, token, board.Last, board);
            }

        }
        return base.AddTokenToBoard(player, hand, token, board, side);
    }

    public override void PlayAlante(ChooseSideWrapped where, Token token, Token first, IBoard board)
    {
        if (first.Part1 == token.Part1)
        {
            token.SwapToken();

        }
        board.AddTokenToBoard(token, 0);

    }

    public override void PlayAtras(ChooseSideWrapped where, Token token, Token last, IBoard board)
    {
        if (token.Part2 == last.Part2)
        {
            token.SwapToken();
        }
        board.AddTokenToBoard(token, 1);

    }
}



#region  Champion


public class ChampionJudge
{
    protected virtual IStopGame<Game, (Game, IPlayer)> stopcriteria { get; set; }
    protected virtual IWinCondition<Game, (Game, IPlayer)> winCondition { get; set; }
    protected virtual IValidPlay<Game, IPlayer, List<IPlayer>> valid { get; set; }
    protected virtual IGetScore<(Game, IPlayer)> howtogetscore { get; set; }

    public ChampionJudge(IStopGame<Game, (Game, IPlayer)> stopcriteria, IWinCondition<Game, (Game, IPlayer)> winCondition, IValidPlay<Game, IPlayer, List<IPlayer>> valid, IGetScore<(Game, IPlayer)> howtogetscore)
    {
        this.howtogetscore = howtogetscore;
        this.stopcriteria = stopcriteria;
        this.valid = valid;
        this.winCondition = winCondition;
    }
    public virtual bool EndGame(Game game)
    {
        if (stopcriteria.MeetsCriteria(game, howtogetscore)) return true;
        return false;
    }
    //Verificar antes de comenzar otro juego 
    public virtual List<IPlayer> ValidPlay(Game game, IPlayer player)//Los que continuan
    {
        return valid.ValidPlay(game, player);
    }

    public virtual List<IPlayer> Winners(List<Game> criterios)
    {
        return this.winCondition.Winner(criterios, this.howtogetscore);
    }

}



public class CorruptionChampionJugde : ChampionJudge, ICorrupcion
{
    public CorruptionChampionJugde(IStopGame<Game, (Game, IPlayer)> stopcriteria, IWinCondition<Game, (Game, IPlayer)> winCondition, IValidPlay<Game, IPlayer, List<IPlayer>> valid, IGetScore<(Game, IPlayer)> howtogetscore) : base(stopcriteria, winCondition, valid, howtogetscore)
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

    public override bool EndGame(Game game)
    {
        if (MakeCorruption())
        {
            return !base.EndGame(game);
        }
        return base.EndGame(game);
    }

    public override List<IPlayer> ValidPlay(Game game, IPlayer player)
    {
        if (MakeCorruption())
        {
            List<IPlayer> valid = base.ValidPlay(game, player);
            Random random = new Random();
            int c = random.Next(0, 100);
            if (c <= 50 && valid.Contains(player)) { valid.Remove(player); return valid; }

            return new List<IPlayer>() { player };
        }

        return base.ValidPlay(game, player);
    }


}

#endregion