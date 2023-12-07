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
            // Implement logic to randomly place ships on the board
            // This is a simple example; you may want to customize ship placement.
            Random random = new Random();
            for (int i = 0; i < 10; i++)
            {
                int row = random.Next(0, BoardSize);
                int col = random.Next(0, BoardSize);
                board[row, col] = 'S'; // Mark ship
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
