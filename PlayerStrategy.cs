namespace Juego
{
    public class RandomStrategy : IPlayerStrategy{
        public int Evaluate(Token token, List<Token> hand, Game game){
             var r = new Random();
             return r.Next(1,100);
        }
    }

    public class BGStrategy: IPlayerStrategy{
        public int Evaluate( Token token, List<Token> hand, Game game){
            return game.judge.howtogetscore.Score(token);
        }
    } 

    public class SemiSmart: IPlayerStrategy{
        public int Evaluate(Token token, List<Token> hand, Game game){
            int valor = 0;

             foreach (var item in hand)
             {
                if (item.IsMatch(token)) valor++; 
             }

             if (token.IsDouble()) valor++;

             valor += (int)(game.judge.howtogetscore.Score(token) / 2);

             return valor;
        }
    }
}