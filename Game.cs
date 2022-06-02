namespace Juego
{
        public class Game : IRules
    {
        public Board board { get; set; }
        public List<Player> player{get; set;}
        public bool SwitchDirection{get; set;}
        public int MaxDouble{get; set;}
        public int Players{get; set;}
        public int TokensForEach{get; set;}
        public Judge judge{get;set;}


        public Game(Board board, Player[] players, bool direction, int max, int plays, int rep, Judge judge)
        {
            this.board = board;
            this.player = players.ToList();
            this.SwitchDirection = direction;
            this.MaxDouble = max;
            this.Players = plays;
            this.TokensForEach = rep;
            this.judge = judge;

           // GenerarTokens();
            AssignTokens();
        }

        public void AddPlayer(Player player){
            this.player.Add(player);
        }

        public void AssignTokens(){

            List<Token> PosiblesTokens =  GenerarTokens(); 

            foreach (var play in player)
            {
                play.hand = RepartirTokens(PosiblesTokens);
            }
        }


        public List<Token> RepartirTokens(List<Token> PosiblesTokens){
            int cantidadDisponible = PosiblesTokens.Count;

            int contador = 0;
            var lista = new List<Token>();

                while (contador != TokensForEach)
                {
                    var r = new Random();
                    var index = r.Next(0, cantidadDisponible);
                    cantidadDisponible--;
                    contador++;
                    lista.Add(PosiblesTokens[index]);
                    PosiblesTokens.RemoveAt(index);
                }

            return lista;
        }

        public List<Token> GenerarTokens(){

            List<Token> PosiblesTokens = new List<Token>();

             for (int i = 0; i <= this.MaxDouble; i++)
            {
                for (int j = i; j <= this.MaxDouble; j++)
                {
                    Token Token = new Token(i, j);
                    PosiblesTokens.Add(Token);
                }
            }

            return PosiblesTokens;
        } 

        public override string ToString(){
           string a = "Resultados del juego:  \n";

           a+= board.ToString();

          return a;     
        }
    

       
        public void SwapDirection(int player){

          if (SwitchDirection){
              Player[] players = new Player[this.Players];

               for (int i = 0; i < players.Length; i++)
                 {
                     if (player ==0) player = players.Length;

                     players[i] = this.player[player-1];
                     player --;   
                 }

                 this.player = players.ToList();
          
          }


        }

        
    }
}



