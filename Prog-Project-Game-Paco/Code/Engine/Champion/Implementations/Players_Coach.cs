namespace Game;

public class PlayersCoach
{
    public List<IPlayer> AllPlayers { get; protected set; }
    public List<List<IPlayer>> players { get; protected set; }
    public List<List<IPlayer>> LastPlayersPlays { get; protected set; }

    public PlayersCoach(List<IPlayer> AllPLayers)
    {
        this.AllPlayers = AllPLayers;
        this.players = new List<List<IPlayer>>() { };
        this.LastPlayersPlays = new List<List<IPlayer>>() { };
    }

    public void AddPlayers(int idGame, List<IPlayer> players)
    {
        this.players.Add(players);
    }

    public bool CloneLastGame(int id)
    {
        List<IPlayer> last = ClonePlayers(this.players[id]);
        if (last == null) return false;
        this.players.Add(last);
        return true;
    }

    protected List<IPlayer> ClonePlayers(List<IPlayer> list)
    {
        List<IPlayer> temp = new List<IPlayer>() { };
        foreach (var item in list)
        {
            temp.Add(item.Clone());
        }
        return temp;
    }

    public List<IPlayer> GetNextPlayers()
    {
        if (this.players.Count < 1) return null!;
        List<IPlayer> temp = (ClonePlayers(this.players.First()));
        this.LastPlayersPlays.Add(this.players.First());
        this.players.RemoveAt(0);
        return temp;
    }

}

