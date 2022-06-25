namespace Game;

public class Diagnostics
{
    public bool TestGame(IBoard board)
    {
        if (board.board.Count < 1) { return true; }
        for (int i = 0; i < board.board.Count; i++)
        {
            if (i > 0)
            {
                Token anterior = board.board[i - 1];
                Token c = board.board[i];

                if (anterior.Part2 != c.Part1)
                {
                    System.Console.WriteLine("nO MACHEA BIEN");

                    System.Console.WriteLine();

                    System.Console.WriteLine(board.ToString());

                    Console.ReadKey();
                    return false;

                }
            }

        }
        return true;
    }
}