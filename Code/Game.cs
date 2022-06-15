namespace Juego
{
        public class Game : IRules,ICloneable<Game>
    {
        public bool Test{get; set;}=false; 
        //Para testiar que se clonan correctamente
        public bool AssignToken{get; private set;}=false;
          
        public IBoard board { get; set; }
        public bool DrawToken {get;set;}
        public List<IPlayer> player{get; set;}
        public bool SwitchDirection{get; set;}
        public int MaxDouble{get; set;}
        public int Players{get; set;}
        public int TokensForEach{get; set;}
        public IJudge judge{get;set;}


        public Game(IBoard board, IPlayer[] players, bool direction, int max, int plays, int rep, IJudge judge, bool draw)
        {
            this.board = board;
            this.player = players.ToList();
            this.SwitchDirection = direction;
            this.MaxDouble = max;
            this.Players = plays;
            this.TokensForEach = rep;
            this.judge = judge;
            this.DrawToken = draw;

           // GenerarTokens();
            AssignTokens();
        }

        public void AddPlayer(Player player){
            this.player.Add(player);
        }

        public bool DeletePlayer(Player player){
            if (this.player.Count ==1) return false;

            this.player.Remove(player);
            return true; 
        }

        public void AssignTokens(){

            List<Token> PosiblesTokens =  GenerarTokens();

            foreach (var play in player)
            {
                play.hand = RepartirTokens(PosiblesTokens);
            }
            AssignToken=true;
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
              IPlayer[] players = new Player[this.Players];

               for (int i = 0; i < players.Length; i++)
                 {
                     if (player ==0) player = players.Length;

                     players[i] = this.player[player-1];
                     player --;   
                 }

                 this.player = players.ToList();
          
          }
        }

         public virtual List<IPlayer> Winner(){
          return this.judge.winCondition.Winner(this.player, this.judge);
    }

       public Game Clone() 
       {
            return new Game(new Board(new List<Token>()),this.player.ToArray(),this.SwitchDirection,this.MaxDouble,this.Players,this.TokensForEach,this.judge,this.DrawToken);
       }
        
    }
}



