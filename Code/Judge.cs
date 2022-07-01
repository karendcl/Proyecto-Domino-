namespace Game;

#region Game
public class Judge : IDescriptible
{
    protected virtual IStopGame<Player, Token> stopcriteria { get; set; }
    protected virtual IGetScore<Token> howtogetscore { get; set; }
    protected virtual IWinCondition<(Player player, List<Token> hand), Token> winCondition { get; set; }
    protected virtual IValidPlay<Board, Token, ChooseStrategyWrapped> valid { get; set; }
    protected virtual Player playernow { get; set; } = null!;
    protected virtual List<ChooseStrategyWrapped> validTokenFornow { get; set; }

    public virtual string Description => "Game";

    //Guarda las fichas que son validas en un momento x
    public Judge(IStopGame<Player, Token> stop, IGetScore<Token> getscore, IWinCondition<(Player player, List<Token> hand), Token> winCondition, IValidPlay<Board, Token, ChooseStrategyWrapped> valid)
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
    public virtual ChooseStrategyWrapped ValidPlay(Player player, Board board, Token token)
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

    public virtual bool EndGame(List<(Player, List<Token>)> players, Board board)
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


    public virtual int PlayerScore(Player player)
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
    protected virtual bool CheckHand(Player player, GamePlayerHand<Token> hand, Token token)
    {
        if (player.hand.Count != hand.hand.Count) { return false; }

        if (player.hand.Contains(token) && hand.ContainsToken(token)) { return true; }

        return false;

    }

    public virtual bool AddTokenToBoard(Player player, GamePlayerHand<Token> hand, Token token, Board board, int side)
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

    public virtual List<Player> Winner(List<(Player player, List<Token> hand)> players)
    {
        List<(Player player, List<Token> hand)> temp = new List<(Player player, List<Token> hand)>() { };
        foreach (var (player, hand) in players)
        {
            if (ItsThePlayerHand(player, hand)) { temp.Add((player, hand)); }
        }
        if (temp.Count < 1) { return new List<Player>() { }; }
        return this.winCondition.Winner(temp, this.howtogetscore);
    }

    protected virtual bool ItsThePlayerHand(Player player, List<Token> hand)
    {
        List<Token> phand = player.hand;
        if (hand.Count != player.hand.Count) { return false; }
        for (int i = 0; i < hand.Count; i++)
        {
            if (!phand[i].Equals(hand[i])) { return false; }
        }
        return true;
    }



    public virtual void PlayAlante(ChooseSideWrapped where, Token token, Token first, Board board)
    {

        if (where.DontSwap)
        {/* token.SwapToken();*/
            board.AddTokenToBoard(token, 0);

            return;

        }

        token.SwapToken(where.WhereCanMacht.ToHashSet<(int, int)>());

        board.AddTokenToBoard(token, 0);
    }

    public virtual void PlayAtras(ChooseSideWrapped where, Token token, Token last, Board board)
    {
        if (where.DontSwap)
        {
            board.AddTokenToBoard(token, 1); return;

        }

        token.SwapToken(where.WhereCanMacht.ToHashSet<(int, int)>());

        board.AddTokenToBoard(token, 1); return;


    }




}


public class CorruptionJugde : Judge
{
    public override string Description => "Corruption Jugde";
    protected Random random { get; set; }
    public CorruptionJugde(IStopGame<Player, Token> stop, IGetScore<Token> getscore, IWinCondition<(Player player, List<Token> hand), Token> winCondition, IValidPlay<Board, Token, ChooseStrategyWrapped> valid) : base(stop, getscore, winCondition, valid)
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
    public override bool AddTokenToBoard(Player player, GamePlayerHand<Token> hand, Token token, Board board, int side)
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

    public override void PlayAlante(ChooseSideWrapped where, Token token, Token first, Board board)
    {
        if (first[0] == token[0])  /*first.Part1 == token.Part1*/
        {
            token.SwapToken(0, 1);

        }
        board.AddTokenToBoard(token, 0);

    }

    public override void PlayAtras(ChooseSideWrapped where, Token token, Token last, Board board)
    {
        if (token[1] == token[1])  //token.Part2 == last.Part2
        {
            token.SwapToken(0, 1);

        }
        board.AddTokenToBoard(token, 1);

    }
}

#endregion


#region  Champion


public class ChampionJudge : IDescriptible
{
    protected virtual IStopGame<Game, (Game, Player)> stopcriteria { get; set; }
    protected virtual IWinCondition<Game, (Game, Player)> winCondition { get; set; }
    protected virtual IValidPlay<List<GameStatus>, Player, bool> valid { get; set; }
    protected virtual IGetScore<(Game, Player)> howtogetscore { get; set; }

    public virtual string Description => "Champion Jugde";

    public ChampionJudge(IStopGame<Game, (Game, Player)> stopcriteria, IWinCondition<Game, (Game, Player)> winCondition, IValidPlay<List<GameStatus>, Player, bool> valid, IGetScore<(Game, Player)> howtogetscore)
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
    public virtual bool ValidPlay(List<GameStatus> game, Player player)//Los que continuan
    {
        return valid.ValidPlay(game, player);
    }

    public virtual List<Player> Winners(List<Game> criterios)
    {
        return this.winCondition.Winner(criterios, this.howtogetscore);
    }

}



public class CorruptionChampionJugde : ChampionJudge
{
    public override string Description => "Corruption Champion Judge";
    public CorruptionChampionJugde(IStopGame<Game, (Game, Player)> stopcriteria, IWinCondition<Game, (Game, Player)> winCondition, IValidPlay<List<GameStatus>, Player, bool> valid, IGetScore<(Game, Player)> howtogetscore) : base(stopcriteria, winCondition, valid, howtogetscore)
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

    public override bool ValidPlay(List<GameStatus> game, Player player)
    {
        if (MakeCorruption())
        {
            bool x = base.ValidPlay(game, player);
            Random random = new Random();
            int c = random.Next(0, 100);
            if (c <= 50 && x) { return false; ; }

            return true;
        }

        return base.ValidPlay(game, player);
    }


}

#endregion