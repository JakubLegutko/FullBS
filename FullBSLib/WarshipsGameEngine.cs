using System;

namespace FullBSLib
{
    public class WarshipsGameEngine
    {
        
        public const int BoardSize = 10;
        public char[,] playerBoard;
        public char[,] computerBoard;


        public WarshipsGameEngine()
        {
            playerBoard = InitializeBoard();
            computerBoard = InitializeBoard();


            PlaceShips(playerBoard);
            PlaceShips(computerBoard);
        }

        private char[,] InitializeBoard()
        {
            return new char[BoardSize, BoardSize];
        }

        private void PlaceShips(char[,] board)
        {
            Random random = new Random();

            int[] shipLengths = { 4, 3, 3, 2, 2, 2, 1, 1, 1, 1 };

            foreach (int shipLength in shipLengths)
            {
                bool shipPlaced = false;

                while (!shipPlaced)
                {
                    int direction = random.Next(2); // 0 for horizontal, 1 for vertical

                    int row = random.Next(BoardSize);
                    int col = random.Next(BoardSize);

                    if (CanPlaceShip(board, row, col, shipLength, direction))
                    {
                        PlaceShip(board, row, col, shipLength, direction);
                        shipPlaced = true;
                    }
                }
            }
        }

        private bool CanPlaceShip(char[,] board, int row, int col, int length, int direction)
        {
            if (direction == 0) // Horizontal
            {
                if (col + length > BoardSize)
                {
                    return false; // Ship goes off the board
                }

                for (int i = col; i < col + length; i++)
                {
                    if (board[row, i] != '\0' || IsAdjacentShip(board, row, i))
                    {
                        return false; // Ship intersects with another ship or is adjacent to another ship
                    }
                }
            }
            else // Vertical
            {
                if (row + length > BoardSize)
                {
                    return false; // Ship goes off the board
                }

                for (int i = row; i < row + length; i++)
                {
                    if (board[i, col] != '\0' || IsAdjacentShip(board, i, col))
                    {
                        return false; // Ship intersects with another ship or is adjacent to another ship
                    }
                }
            }

            return true;
        }

        private bool IsAdjacentShip(char[,] board, int row, int col)
        {
            // Check if there is a ship cell in the adjacent positions (horizontal, vertical, and diagonal)
            for (int i = row - 1; i <= row + 1; i++)
            {
                for (int j = col - 1; j <= col + 1; j++)
                {
                    if (i >= 0 && i < BoardSize && j >= 0 && j < BoardSize && board[i, j] == 'S')
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void PlaceShip(char[,] board, int row, int col, int length, int direction)
        {
            if (direction == 0) // Horizontal
            {
                for (int i = col; i < col + length; i++)
                {
                    board[row, i] = 'S';
                }
            }
            else // Vertical
            {
                for (int i = row; i < row + length; i++)
                {
                    board[i, col] = 'S';
                }
            }
        }


        public int PlayerAttack(int row, int col)
        {
            return ProcessAttack(row, col, computerBoard);
        }

        public int ComputerAttack()
        {
            int row = GetRandomCoordinate();
            int col = GetRandomCoordinate();
            return ProcessAttack(row, col, playerBoard);
        }

        private int ProcessAttack(int row, int col, char[,] targetBoard)
        {
            if (IsValidMove(row, col) && targetBoard[row, col] == 'S')
            {
                targetBoard[row, col] = 'X'; // Mark hit
                return 1; // Hit
            }
            else if (IsValidMove(row, col) && targetBoard[row, col] == '\0')
            {
                targetBoard[row, col] = 'O'; // Mark miss
                return 2; // Miss
            }
            // If cell is filled with hit or miss marker, return 0

            return 0; // Invalid move
        }

        public int IsGameOver()
        {
            if (AllShipsSunk(playerBoard))
            {
                return 1; // Player lost
            }
            else if (AllShipsSunk(computerBoard))
            {
                return 2; // Player won
            }
            return 0; // Game not over
        }

        private bool AllShipsSunk(char[,] board)
        {
            // Implement logic to check if all ships on the board are sunk
            // This is a simple example; you may want to customize this based on your ship representation.
            foreach (var cell in board)
            {
                if (cell == 'S')
                {
                    return false; // There is at least one ship cell remaining
                }
            }
            return true; // All ships are sunk
        }

        private bool IsValidMove(int row, int col)
        {
            return row >= 0 && row < BoardSize && col >= 0 && col < BoardSize;
        }

        private int GetRandomCoordinate()
        {
            Random random = new Random();
            return random.Next(0, BoardSize);
        }
    }
}
