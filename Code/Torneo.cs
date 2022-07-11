using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace Game;

//Torneo Se encarga de crear los juegos y procesarlos
public class Championship
{
    #region Global

    public event Predicate<Orders> CanContinue;
    public event Action<ChampionStatus> status;
    public event Func<string, bool> Choose;
    public int Champions { get; protected set; }
    public ChampionJudge judge { get; protected set; }
    public List<Game> Games { get; protected set; }
    public PlayersCoach GlobalPlayers { get; protected set; }
    protected bool HaveAWinner { get; set; }
    protected List<Game> FinishGames = new List<Game>() { };
    protected Stack<GameStatus> GamesStatus = new Stack<GameStatus>() { };
    protected List<Player> Winners { get { return this.ChampionWinners(); } }
    public bool ItsChampionOver { get; protected set; }
    protected List<PlayerStats> PlayerStats { get; set; } = new List<PlayerStats>() { };
    protected List<Player> AllPlayers { get { return GlobalPlayers.AllPlayers; } }


    #endregion

    public Championship(int cantTorneos, ChampionJudge judge, PlayersCoach players, List<Game> games)
    {
        this.Champions = cantTorneos;
        this.Games = games;
        this.GlobalPlayers = players;
        this.judge = judge;

    }

    public void Run()
    {

        for (int i = 0; i < Games.Count; i++)
        {
            Game game = Games[i];
            game.GameStatus += this.PrintGames;
            game.CanContinue += this.Continue;
            List<Player> players = GlobalPlayers.GetNextPlayers();
            ControlPlayers(players);
            if (players.Count < 1)
            {
                ChampionOver();
                break;
            }
            GameStatus gameStatus = game.PlayAGame(new Board(), players);
            GameOver(game, gameStatus, i);
            if (judge.EndGame(this.FinishGames))
            {

                ChampionOver();
                break;
            }
            Orders c = Orders.NextPlay;
            if (!Continue(c)) { ChampionPrint(); break; }



        }
        ChampionOver();
    }

    #region Mandar informacion de los partidos

    public bool Continue(Orders orders) => this.CanContinue(orders);
    protected void GameOver(Game game, GameStatus gameStatus, int i)
    {
        this.GamesStatus.Push(gameStatus);
        this.FinishGames.Add(game);
        this.PrintGames(gameStatus);
    }

    protected void PrintGames(GameStatus gameStatus)
    {
        
        ChampionStatus championStatus = CreateAChampionStatus();
        championStatus.AddGameStatus(gameStatus);
        this.status.Invoke(championStatus);
    }
    #endregion


    protected ChampionStatus CreateAChampionStatus()
    {
        List<Player> AllPlayer = this.GlobalPlayers.AllPlayers;

        ChampionStatus championStatus = new ChampionStatus(this.GamesStatus, this.PlayerStats, HaveAWinner, this.Winners, this.ItsChampionOver);
        return championStatus;
    }

    protected void ControlPlayers(List<Player> players)
    {
        List<Player> temp = new List<Player>();
        foreach (var player in players)
        {
            if (judge.ValidPlay(this.FinishGames, player)) temp.Add(player);
        }
        players = temp;
    }

    protected void PlayerStatistics()
    {
        List<PlayerStats> Strats = new List<PlayerStats>() { };
        foreach (var player in this.AllPlayers)
        {
            PlayerStats temp = new PlayerStats(player);
            int punctuation = 0;
            foreach (var Game in this.FinishGames)
            {
                if (Game.player.Contains(player))
                {
                    punctuation += Game.judge.PlayerScore(player);
                }
            }
            temp.AddPuntuation(punctuation);
            Strats.Add(temp);
        }

        this.PlayerStats = Strats;
    }


    protected void ChampionOver()
    {
        this.ItsChampionOver = true;
        PlayerStatistics();
        ChampionStatus status = CreateAChampionStatus();
        this.status.Invoke(status);

    }



    protected List<Player> ChampionWinners() => this.judge.Winners(this.FinishGames.ToList<Game>());



    protected void ChampionPrint()
    {
        ChampionStatus championStatus = this.CreateAChampionStatus();
        this.status.Invoke(championStatus);
    }

}





