
using System.Collections.ObjectModel;
using TicTacToe_GUI.Models;
namespace TicTacToe
{
    public partial class Home : ContentPage
    {

        public ObservableCollection<CellModel> Board { get; set; } = new ObservableCollection<CellModel>();

        public int Dimension { get; set; }
        int alpha = int.MinValue;
        int beta = int.MaxValue;
        int MaxDepth = int.MaxValue;
        char[,] board;
        public Home(string difficultyLevel, int dimension, char[,] board)
        {
            Dimension = dimension;
            this.board = board;
            SetDifficultyLevel(difficultyLevel);
            InitializeBoard();
            BindingContext = this;
            InitializeComponent();

        }

        void InitializeBoard()
        {
            // Initialize the board with empty spaces

            for (int i = 0; i < Dimension; i++)
            {
                for (int j = 0; j < Dimension; j++)
                {
                    board[i, j] = PlayerMode.Empty;
                    Board.Add(new CellModel { Row = i, Column = j, Value = PlayerMode.Empty });
                }
            }
        }
        private void SetDifficultyLevel(string difficultyLevel)
        {
            if (difficultyLevel == "Hard" && Dimension > 3)
                MaxDepth = Dimension;
            else if (difficultyLevel == "Easy")
                MaxDepth = Dimension / 3;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var cell = (CellModel)((Button)sender).BindingContext;
            PlayerMove(cell);
        }

        async void DisplayFinalResult()
        {
            // display the result of the game

            if (IsWinning(board, PlayerMode.Player))
            {
                await DisplayAlert("Finished", "You Won", "OK");
            }
            else if (IsWinning(board, PlayerMode.Bot))
            {
                await DisplayAlert("Finished", "You Lose", "OK");

            }
            else
            {
                await DisplayAlert("Finished", "It's a tie!", "OK");

            }
            Cells.IsEnabled = false;
        }

        void PlayerMove(CellModel cell)
        {

            if (IsValidMove(cell.Row, cell.Column))
            {
                board[cell.Row, cell.Column] = PlayerMode.Player;
                cell.Value = PlayerMode.Player;

            }
            else
                return;

            // Check if the game is over after the human's move
            if (IsGameOver(board))
            {
                DisplayFinalResult();
                return;
            }


            BotMove();

            if (IsGameOver(board))
                DisplayFinalResult();
        }

        void BotMove()
        {

            // Get the computer's move using the alpha-beta pruning algorithm and update the board
            int[] move = AlphaBeta(board, PlayerMode.Bot, MaxDepth, alpha, beta);
            board[move[0], move[1]] = PlayerMode.Bot;
            foreach (CellModel cell in Board)
            {
                if (cell.Row == move[0] && cell.Column == move[1])
                    cell.Value = PlayerMode.Bot;
            }


        }

        bool IsValidMove(int row, int col)
        {
            return row >= 0 && row < Dimension && col >= 0 && col < Dimension && board[row, col] == PlayerMode.Empty;
        }


        bool IsGameOver(char[,] board)
        {
            return IsWinning(board, PlayerMode.Player) || IsWinning(board, PlayerMode.Bot) || IsFull(board);
        }


        bool IsFull(char[,] board)
        {
            for (int i = 0; i < Dimension; i++)
            {
                for (int j = 0; j < Dimension; j++)
                {
                    if (board[i, j] == PlayerMode.Empty) return false;
                }
            }
            return true;
        }


        bool IsWinning(char[,] board, char player)
        {
            int diagCounter1 = 0;
            int diagCounter2 = 0;

            for (int i = 0; i < Dimension; i++)
            {
                int rowCounter = 0;
                int colCounter = 0;

                // Check rows

                for (int k = 0; k < Dimension; k++)
                {
                    if (board[i, k] == player)
                        rowCounter++;
                }
                if (rowCounter == Dimension) return true;



                // Check columns

                for (int j = 0; j < Dimension; j++)
                {
                    if (board[j, i] == player)
                        colCounter++;
                }
                if (colCounter == Dimension) return true;


                // Check diagonals

                if (board[i, i] == player)
                    diagCounter1++;
                if (board[i, Dimension - i - 1] == player)
                    diagCounter2++;
            }
            // Check diagonals

            if (diagCounter1 == Dimension) return true;
            if (diagCounter2 == Dimension) return true;

          
            return false;
        }


        int[] AlphaBeta(char[,] board, char player, int depth, int alpha, int beta)
        {
            // Initialize the best move and score
            int[] bestMove = new int[2];
            int bestScore;

            // If the board is full or the game is over, return the evaluation and no move
            if (IsGameOver(board) || depth == 0)
            {
                bestScore = Evaluation.Evaluate(board, Dimension);
                bestMove[0] = -1;
                bestMove[1] = -1;

                return new int[] { bestMove[0], bestMove[1], bestScore };
            }

            // If it is the computer's turn, maximize the score
            if (player == PlayerMode.Bot)
            {
                bestScore = int.MinValue;

                // Loop through all the possible moves
                for (int i = 0; i < Dimension; i++)
                {
                    for (int j = 0; j < Dimension; j++)
                    {
                        // If the move is valid
                        if (board[i, j] == PlayerMode.Empty)
                        {
                            // Make the move
                            board[i, j] = player;

                            // Recursively get the score from the opponent's move

                            int[] move = AlphaBeta(board, PlayerMode.Player, depth - 1, alpha, beta);
                            int score = move[2];

                            // Undo the move
                            board[i, j] = PlayerMode.Empty;

                            // Update the best score and move
                            if (score > bestScore)
                            {
                                bestScore = score;
                                bestMove[0] = i;
                                bestMove[1] = j;
                            }

                            // Update the alpha value
                            alpha = Math.Max(alpha, bestScore);

                            // Prune the branch if alpha is greater than or equal to beta
                            if (alpha >= beta)


                                break;

                        }
                    }
                    if (alpha >= beta)

                        break;

                }
            }
            // If it is the human's turn, minimize the score
            else
            {
                bestScore = int.MaxValue;

                // Loop through all the possible moves
                for (int i = 0; i < Dimension; i++)
                {
                    for (int j = 0; j < Dimension; j++)
                    {
                        // If the move is valid
                        if (board[i, j] == PlayerMode.Empty)
                        {
                            // Make the move
                            board[i, j] = player;

                            // Recursively get the score from the opponent's move
                            int[] move = AlphaBeta(board, PlayerMode.Bot, depth - 1, alpha, beta);
                            int score = move[2];

                            // Undo the move
                            board[i, j] = PlayerMode.Empty;

                            // Update the best score and move
                            if (score < bestScore)
                            {
                                bestScore = score;
                                bestMove[0] = i;
                                bestMove[1] = j;
                            }

                            // Update the beta value
                            beta = Math.Min(beta, bestScore);

                            // Prune the branch if alpha is greater than or equal to beta
                            if (alpha >= beta)
                                break;


                        }
                    }
                    if (alpha >= beta)
                        break;
                }
            }

            // Return the best move and score
            return new int[] { bestMove[0], bestMove[1], bestScore };
        }
    }

}
