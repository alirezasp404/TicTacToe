

namespace TicTacToe
{
    public static class Evaluation
    {

        public static int Evaluate(char[,] board,int Dimension)
        {

       
            int score = 0;

            char diagDominance1 = PlayerMode.Empty;
            char diagDominance2 = PlayerMode.Empty;

            int diagScore1 = 0;
            int diagScore2 = 0;
            // Loop through the rows, columns, and diagonals of the board
            for (int i = 0; i < Dimension; i++)
            {
                int rowscore = 0;
                int colscore = 0;
                char rowDominance = PlayerMode.Empty;
                char colDominance = PlayerMode.Empty;
                //check the row
                for (int k = 0; k < Dimension; k++)
                {
                    rowDominance = checkState(board[i, k], rowDominance);
                    if (rowDominance == PlayerMode.Both)
                        break;
                    rowscore += calculateScore(board[i, k]);
                }
                //check the column
                for (int j = 0; j < Dimension; j++)
                {
                    colDominance = checkState(board[j, i], colDominance);
                    if (colDominance == PlayerMode.Both)
                        break;
                    colscore += calculateScore(board[j, i]);
                }
                //check diagonal
                diagDominance1 = checkState(board[i, i], diagDominance1);
                diagScore1 += calculateScore(board[i, i]);
                diagDominance2 = checkState(board[i, Dimension - i - 1], diagDominance2);
                diagScore2 += calculateScore(board[i, Dimension - i - 1]);
                
                if (rowDominance != PlayerMode.Both)
                    score += rowscore;
                if (colDominance != PlayerMode.Both)
                    score += colscore;
            }
            if (diagDominance1 != PlayerMode.Both)
                score += diagScore1;
            if (diagDominance2 != PlayerMode.Both)
                score += diagScore2;

            // Return the final score
            return score;
        }

        public static int calculateScore(char state)
        {
            int score = 0;
            if (state == PlayerMode.Bot) score++;
            else if (state == PlayerMode.Player) score--;
            return score;
        }

        public static char checkState(char state, char dominance)
        {
            if (state != PlayerMode.Empty)
            {
                if (dominance == PlayerMode.Empty)
                    dominance = state;
                else if (dominance == PlayerMode.Bot && state == PlayerMode.Player)
                    dominance = PlayerMode.Both;
                else if (dominance == PlayerMode.Player && state == PlayerMode.Bot)
                    dominance = PlayerMode.Both;
            }
            return dominance;
        }

    }
}
