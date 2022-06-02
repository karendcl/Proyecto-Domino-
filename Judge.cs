namespace Juego
{
    public class Judge : IJudge
    {
       public bool ValidPlay(Board board, Token token){
            if (board.board.Count==0) return true;
            if (token.Contains(board.First().Parte1)) return true;
            if (token.Contains(board.Last().Parte2)) return true;
            return false;
        }
   public bool EndGame(Game game){
        
            foreach (var player in game.player)
            {
                if (player.hand.Count ==0) return true;

                foreach (var token in player.hand)
                {
                    if (this.ValidPlay(game.board, token)) return false;
                }
            }
            return true;
        
    }
  public  bool ValidSettings(int TokensForEach, int MaxDoble, int players){
        int totalamount = 0;

        for (int i = 0; i <= MaxDoble +1; i++)
        {
            totalamount += i;
        }

        return (TokensForEach*players > totalamount) ? false : true;
    }
   public List<Player> Winner(Game game){

        int[] scores = new int[game.player.Count];
        var result = new List<Player>();

        int count = 0;

        foreach (var player in game.player)
        {
            if (player.hand.Count==0){
              result.Add(player);
            }

            foreach (var Token in player.hand)
            {
                scores[count] += Token.Value();
            }
            count++;
        }

        if (result.Count != 0) return result;

        int score = scores.Min();
        
        for (int i = 0; i < scores.Length; i++)
        {
            if (scores[i] == score){
                result.Add(game.player[i]);
            }
        }
        
        return result;

    }
    }
}