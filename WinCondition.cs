namespace Juego
{
    public class MinScore : IWinCondition{
       public List<IPlayer> Winner (List<IPlayer> players, IJudge judge){

        int count = 0;
        var result = new List<IPlayer>();
        int[] scores = new int[players.Count];

        foreach (var player in players)
        {
            if (player.hand.Count==0){
              result.Add(player);
            }

            foreach (var Token in player.hand)
            {
                scores[count] += judge.howtogetscore.Score(Token);
            }
            count++;
        }

        if (result.Count != 0) return result;

        return FinalWinner(scores, players);
        
        
        }

        public  List<IPlayer> FinalWinner (int[] scores, List<IPlayer> players){
            var result = new List<IPlayer>();

            int score = scores.Min();

            for (int i = 0; i < scores.Length; i++)
        {
            if (scores[i] == score){
                result.Add(players[i]);
            }
        }
        
        return result;
        }
    }

    public class MaxScore : IWinCondition
    {
       public List<IPlayer> Winner (List<IPlayer> players, IJudge judge){

        int count = 0;
        var result = new List<IPlayer>();
        int[] scores = new int[players.Count];

        foreach (var player in players)
        {
            foreach (var Token in player.hand)
            {
                scores[count] +=  scores[count] += judge.howtogetscore.Score(Token);;
            }
            count++;
        }

        if (result.Count != 0) return result;

        return FinalWinner(scores, players);
        
        
        }

        public  List<IPlayer> FinalWinner (int[] scores, List<IPlayer> players){
            var result = new List<IPlayer>();

            int score = scores.Min();

            for (int i = 0; i < scores.Length; i++)
        {
            if (scores[i] == score){
                result.Add(players[i]);
            }
        }
        
        return result;
        }
    }

    public class Specificscore : IWinCondition{
        int score{get;set;}
        public Specificscore(int score){
            this.score = score;
        }

        
        public List<IPlayer> Winner (List<IPlayer> players, IJudge judge){

        int count = 0;
        var result = new List<IPlayer>();
        int[] scores = new int[players.Count];

        foreach (var player in players)
        {
            if (player.hand.Count==0){
              result.Add(player);
            }

            foreach (var Token in player.hand)
            {
                scores[count] += judge.howtogetscore.Score(Token);
            }
            count++;
        }

        if (result.Count != 0) return result;

        return FinalWinner(scores, players);
        
        
        }

        public  List<IPlayer> FinalWinner (int[] scores, List<IPlayer> players){
            var result = new List<IPlayer>();

            int score = scores.Min();

            for (int i = 0; i < scores.Length; i++)
        {
            if (scores[i] == score){
                result.Add(players[i]);
            }
        }
        
        return result;
        }
    }
}