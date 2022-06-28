using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace Game;

//Torneo Se encarga de crear los juegos y procesarlos
public class Championship
{
    #region Global
    public event Action<ChampionStatus> status;
    public event Func<string, bool> Choose;
    public int Champions { get; protected set; }
    public ChampionJudge judge { get; private set; }
    public List<Game> Games { get; protected set; }
    public PlayersCoach GlobalPlayers { get; protected set; }
    protected bool HaveAWinner { get; set; }
    protected Stack<Game> FinishGames = new Stack<Game>() { };
    protected Stack<GameStatus> GamesStatus = new Stack<GameStatus>() { };
    protected List<IPlayer> Winners { get { return this.ChampionWinners(); } }
    public bool ItsChampionOver { get; protected set; }
    protected List<PlayerStrats> playerStrats { get; set; } = new List<PlayerStrats>() { };
    protected List<IPlayer> AllPlayers { get { return GlobalPlayers.AllPlayers; } }
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
            game.GameStatus += PrintGames;
            List<IPlayer> players = GlobalPlayers.GetNextTeam();
            GameStatus gameStatus = game.PlayAGame(new Board(), players);

            if (judge.EndGame(game))
            {
                GameOver(game, gameStatus, i);
                ChampionOver();
                break;
            }

            if (!ContinueGames()) { /*observer.WriteStats(game);*/ ChampionPrint(players, this.judge.winCondition.Winner(Games, this.judge.howtogetscore)); break; }

            GameOver(game, gameStatus, i);

        }
        ChampionOver();
    }

    #region Mandar informacion de los partidos
    private void GameOver(Game game, GameStatus gameStatus, int i)
    {
        this.FinishGames.Push(game);
        this.PrintGames(gameStatus);
    }

    private void PrintGames(GameStatus gameStatus)
    {
        this.GamesStatus.Push(gameStatus);
        ChampionStatus championStatus = CreateAChampionStatus();
        championStatus.AddGameStatus(gameStatus);
        this.status.Invoke(championStatus);
    }
    #endregion



    protected ChampionStatus CreateAChampionStatus()
    {
        List<IPlayer> AllPlayer = this.GlobalPlayers.AllPlayers;

        ChampionStatus championStatus = new ChampionStatus(this.GamesStatus, this.playerStrats, HaveAWinner, this.Winners, this.ItsChampionOver);
        return championStatus;
    }


    protected void PlayerStrats()
    {
        List<PlayerStrats> Strats = new List<PlayerStrats>() { };
        foreach (var player in this.AllPlayers)
        {
            PlayerStrats temp = new PlayerStrats(player);
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

        this.playerStrats = Strats;
    }


    protected void ChampionOver()
    {
        this.ItsChampionOver = true;
        PlayerStrats();
        ChampionStatus status = CreateAChampionStatus();
        this.status.Invoke(status);

    }



    protected List<IPlayer> ChampionWinners() => this.judge.winCondition.Winner(this.FinishGames.ToList<Game>(), this.judge.howtogetscore);




    //  observer.WriteStats(game);
    protected bool ContinueGames() => Choose.Invoke(("Desea seguir jugando?, si / no"));

    protected void ChampionPrint(List<IPlayer> Players, List<IPlayer> Winners)
    {
        ChampionStatus championStatus = this.CreateAChampionStatus();
        this.status.Invoke(championStatus);
    }

}





