using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace Game;

/// <summary>
///  Interface De un torneo
/// </summary>
/// <param name=""></param>

/// <returns></returns>
public interface IChampionship
{

    int Champions { get; }

    bool ItsChampionOver { get; }

    event Predicate<Orders> CanContinue;
    event Action<ChampionStatus> status;


    bool Continue(Orders orders);
    void Run();
}

//Torneo Se encarga de crear los juegos y procesarlos
/// <summary>
///  Un torneo de Dominó clasico 
/// </summary>
/// <param name=""></param>

/// <returns></returns>
public class Championship : IChampionship
{
    #region Global


    #region Events
    public virtual event Predicate<Orders> CanContinue;//Evento que pregunta al final de cada partida si se puede continuar
    public virtual event Action<ChampionStatus> status;// Evento que envia la informacion del torneo en cada accion desde poner un ficha hasta el final del mismo
    #endregion
    public virtual int Champions { get; protected set; }//Cantidad de partidas
    protected virtual ChampionJudge judge { get; set; }// Juez a nivel de torneo
    protected virtual List<Game> Games { get; set; }// Lista d elas partidas
    protected virtual PlayersCoach GlobalPlayers { get; set; }// El organizador de los jugadores a nivel de torneo
    public virtual bool HaveAWinner { get; protected set; } // True si hay un ganador a nivel de torneo  falso si no lo hay
    protected virtual List<Game> FinishGames { get; set; } = new List<Game>() { }; //Juegos Finalizados
    protected virtual Stack<GameStatus> GamesStatus { get; set; } = new Stack<GameStatus>() { }; //Wrapped que envia las partidas
    protected virtual List<Player> Winners { get { return this.ChampionWinners(); } } //Ganadores a nivel de torneo
    public virtual bool ItsChampionOver { get; protected set; }// Si se acabo el torneo
    protected virtual List<PlayerStats> PlayerStats { get; set; } = new List<PlayerStats>() { }; //Puntuacion del jugador 
    protected virtual List<Player> AllPlayers { get { return GlobalPlayers.AllPlayers; } } // Jugadores a nivel de torneo


    #endregion

    public Championship(int cantTorneos, ChampionJudge judge, PlayersCoach players, List<Game> games)
    {
        this.Champions = cantTorneos;
        this.Games = games;
        this.GlobalPlayers = players;
        this.judge = judge;
        this.judge.Run(this.GlobalPlayers.AllPlayers);

    }

    /// <summary>
    ///  Inicializa el Torneo
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    public void Run()
    {

        for (int i = 0; i < Games.Count; i++) //Por cada una de las partidas
        {
            Game game = Games[i]; // La partida a jugar en este momento
            game.GameStatus += this.PrintGames;// Suscribirse al evento
            game.CanContinue += this.Continue;//Suscribirse al evento
            List<Player> players = GlobalPlayers.GetNextPlayers(); //Jugadores en esta partida
            ControlPlayers(players);// Se controla que los jugadores puedan jugar dicha partida
            if (players.Count < 1) //Si no hay jugadores se acabo el torneo
            {
                ChampionOver();
                break;
            }
            GameStatus gameStatus = game.PlayAGame(new Board(), players);// Se envia el ultimo estatus de la partida 
            this.judge.AddFinishGame(game); //Se añade la ultima partida 
            GameOver(game, gameStatus, i); // Se envia el estatus general del torneo con el ultimo estatus de la partida
            if (judge.EndGame(this.FinishGames)) // se pregunta si se puede continuar jugando
            {

                ChampionOver();
                break;
            }
            Orders c = Orders.NextPlay;
            if (!Continue(c)) { ChampionPrint(); break; }// Se espera confirmacion para continuar a la siguiente partida 



        }
        ChampionOver(); //Se envia el ultimo estado del torneo 
    }

    #region Mandar informacion de los partidos

    public bool Continue(Orders orders) => this.CanContinue(orders);
    protected void GameOver(Game game, GameStatus gameStatus, int i) //Significa que se acabo esa partida
    {
        this.GamesStatus.Push(gameStatus);
        this.FinishGames.Add(game);
        this.PrintGames(gameStatus);
    }

    protected void PrintGames(GameStatus gameStatus) //Se crea el estado de la partida dado que ocurrio un evento a nivel de partida que debe imprimirse
    {

        ChampionStatus championStatus = CreateAChampionStatus();
        championStatus.AddGameStatus(gameStatus);
        this.status.Invoke(championStatus);//Se invoca el evento
    }
    #endregion


    protected ChampionStatus CreateAChampionStatus()   //Crea el estatus del torneo con el ultimo estatus de la partida
    {
        List<Player> AllPlayer = this.GlobalPlayers.AllPlayers;

        ChampionStatus championStatus = new ChampionStatus(this.GamesStatus, this.PlayerStats, HaveAWinner, this.Winners, this.ItsChampionOver);
        return championStatus;
    }

    protected void ControlPlayers(List<Player> players) //Se contorla los players
    {
        List<Player> temp = new List<Player>();
        foreach (var player in players)
        {
            if (judge.ValidPlay(player)) temp.Add(player);
        }
        players = temp;
    }
    protected void PlayerStatistics() //Estadisticas de los jugadores
    {
        List<PlayerStats> Strats = new List<PlayerStats>() { };
        foreach (var player in this.AllPlayers)
        {
            PlayerStats temp = new PlayerStats(player);
            double punctuation = 0;
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

    protected void ChampionOver()  //Ultimo estutus del torneo dado que este finalizó
    {
        this.ItsChampionOver = true;//Se inidica que terminó el torneo
        PlayerStatistics();
        ChampionStatus status = CreateAChampionStatus();
        this.status.Invoke(status);// Invocar el evento

    }

    protected List<Player> ChampionWinners() => this.judge.Winners();
    protected void ChampionPrint()
    {
        ChampionStatus championStatus = this.CreateAChampionStatus();
        this.status.Invoke(championStatus); //Invocar el evento
    }

}





