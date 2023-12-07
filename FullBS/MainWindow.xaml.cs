using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using FullBSLib;

namespace WarshipsGame
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private WarshipsGameEngine gameEngine;
        private ObservableCollection<ObservableCollection<char>> playerBoard;
        private ObservableCollection<ObservableCollection<char>> computerBoard;
        // Add a new property for the computer's visible board
        private ObservableCollection<ObservableCollection<char>> computerVisibleBoard;

        public ObservableCollection<ObservableCollection<char>> ComputerVisibleBoard
        {
            get { return computerVisibleBoard; }
            set
            {
                computerVisibleBoard = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<ObservableCollection<char>> PlayerBoard
        {
            get { return playerBoard; }
            set
            {
                playerBoard = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ObservableCollection<char>> ComputerBoard
        {
            get { return computerBoard; }
            set
            {
                computerBoard = value;
                OnPropertyChanged();
            }
        }

            public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            gameEngine = new WarshipsGameEngine();
            InitializePlayerBoard();
            InitializeComputerBoard();

            AttackCommand = new FullBS.RelayCommand(param => AttackCommandExecute(param), param => CanAttackCommandExecute());
        }


        public ICommand AttackCommand { get; }

        public string GameStatus
        {
            get { return (gameEngine.IsGameOver() == 1 || gameEngine.IsGameOver() == 2) ? "Game Over!" : "Left < Player Board, Right > Computer Board, click there"; }
        }



        private void InitializePlayerBoard()
        {
            PlayerBoard = new ObservableCollection<ObservableCollection<char>>();
            for (int i = 0; i < WarshipsGameEngine.BoardSize; i++)
            {
                var row = new ObservableCollection<char>();
                for (int j = 0; j < WarshipsGameEngine.BoardSize; j++)
                {
                    row.Add(gameEngine.playerBoard[i, j]);
                }
                PlayerBoard.Add(row);
            }
        }
        // Modify the ComputerBoard initialization to hide ship positions
        private void InitializeComputerBoard()
        {
            ComputerBoard = new ObservableCollection<ObservableCollection<char>>();
            ComputerVisibleBoard = new ObservableCollection<ObservableCollection<char>>();

            for (int i = 0; i < WarshipsGameEngine.BoardSize; i++)
            {
                var row = new ObservableCollection<char>();
                var visibleRow = new ObservableCollection<char>();
                for (int j = 0; j < WarshipsGameEngine.BoardSize; j++)
                {
                    row.Add(gameEngine.computerBoard[i, j]);
                    // Initially hide ship positions
                    visibleRow.Add(gameEngine.computerBoard[i, j] == 'S' ? '\0' : gameEngine.computerBoard[i, j]);
                }
                ComputerBoard.Add(row);
                ComputerVisibleBoard.Add(visibleRow);
            }
        }

        private void AttackCommandExecute(object param)
        {
            if (param is string coordinates)
            {
                var splitCoordinates = coordinates.Split(',');
                if (splitCoordinates.Length == 2 && int.TryParse(splitCoordinates[0], out int row) && int.TryParse(splitCoordinates[1], out int col))
                {
                    int isHit = gameEngine.PlayerAttack(row, col);
                    bool validMove = false ;
                    UpdateComputerBoard();
                    if (isHit == 1)
                    {
                        MessageBox.Show("You hit a ship!");
                        //validMove = true; This way a turn won't end after a hit
                    }
                    else if (isHit == 2)
                    {
                        MessageBox.Show("You missed!");
                        validMove = true;
                    }
                    else
                    {
                        MessageBox.Show("Can't hit same field twice!");
                        validMove = false;
                    }

                    if (gameEngine.IsGameOver() == 0 && validMove)
                    {
                        ComputerTurn();

                    }
                }
            }
        }

        private bool CanAttackCommandExecute()
        {
            return !(gameEngine.IsGameOver() == 1 || gameEngine.IsGameOver() == 2);
        }

        private void ComputerTurn()
        {
            int isHit = gameEngine.ComputerAttack();
            bool validMove = false;
            UpdatePlayerBoard();
            if (isHit == 1)
            {
                MessageBox.Show("Computer hit a ship!");
                //validMove = true; Analogous to the player's turn
                ComputerTurn();
            }
            else if (isHit == 2)
            {
                MessageBox.Show("Computer missed!");
                validMove = true;
            }
            else
            {
                validMove = false;
            }

            if (gameEngine.IsGameOver() != 0 && validMove)
            {
                if (gameEngine.IsGameOver() == 1)
                {
                    MessageBox.Show("Game Over! You lost!");
                }
                else if (gameEngine.IsGameOver() == 2)
                {
                    MessageBox.Show("Game Over! You won!");
                }

            }
        }

        private void UpdatePlayerBoard()
        {
            for (int i = 0; i < WarshipsGameEngine.BoardSize; i++)
            {
                for (int j = 0; j < WarshipsGameEngine.BoardSize; j++)
                {
                    // Update the individual characters inside the ObservableCollection
                    PlayerBoard[i][j] = gameEngine.playerBoard[i, j];
                }
            }

            // Notify that the whole collection has changed
            OnPropertyChanged(nameof(PlayerBoard));
            OnPropertyChanged(nameof(GameStatus));
        }
        // Modify the UpdateComputerBoard method to use the visible board
        private void UpdateComputerBoard()
        {
            for (int i = 0; i < WarshipsGameEngine.BoardSize; i++)
            {
                for (int j = 0; j < WarshipsGameEngine.BoardSize; j++)
                {
                    // Update the individual characters inside the ObservableCollection
                    ComputerBoard[i][j] = gameEngine.computerBoard[i, j];
                    // Update the visible board
                    ComputerVisibleBoard[i][j] = gameEngine.computerBoard[i, j] == 'S' ? '\0' : gameEngine.computerBoard[i, j];
                }
            }

            // Notify that the whole collection has changed
            OnPropertyChanged(nameof(ComputerBoard));
            OnPropertyChanged(nameof(ComputerVisibleBoard));
        }

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}