using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Models;
using Models.Events;
using WpfApplication.Views;

namespace WpfApplication {
    public class Presenter {
        private static readonly Dictionary<CellState, CellState> _rightClickStateChain =
            new Dictionary<CellState, CellState> {
                [CellState.Closed] = CellState.MineHere,
                [CellState.MineHere] = CellState.Undefined,
                [CellState.Undefined] = CellState.Closed
            };

        private readonly IGameManager _gameManager;
        private readonly IMatrixView _matrixView;

        public Presenter(IMatrixView matrixView, IGameManager gameManager) {
            _matrixView = matrixView;
            _gameManager = gameManager;

            _gameManager.OnCellStateChanged += GameManagerOnCellStateChangedEventHandler;
            _gameManager.OnGameFinished += GameManagerOnGameFinishedEventHandler;

            _matrixView.OnCellPressed += MatrixViewOnCellPressedEventHandler;
        }

        public void StartGame() {
            var settings = new PlaySettings {
                Columns = 5,
                Rows = 5,
                MineCount = 4
            };
            
            _gameManager.StartGame(settings);
            
            _matrixView.CreateField(settings.Rows, settings.Columns);
            
        }

        private void MatrixViewOnCellPressedEventHandler(object sender, OnCellPressedEventHandlerArgs args) {
            if (args.Button != MouseButton.Left && args.Button != MouseButton.Right) {
                return;
            }

            var curState = _gameManager.GetCellState(args.Row, args.Column);

            if (args.Button == MouseButton.Left) {
                HandleLeftClick(args, curState);
                return;
            }

            HandleRightClick(curState, args.Row, args.Column);
        }

        private void HandleRightClick(CellState curState, byte row, byte column) {
            if (_rightClickStateChain.TryGetValue(curState, out var nextState)) {
                _gameManager.ChangeState(row, column, nextState);
            }
        }

        private void HandleLeftClick(OnCellPressedEventHandlerArgs args, CellState curState) {
            if (curState == CellState.Closed) {
                _gameManager.ChangeState(args.Row, args.Column, CellState.Opened);
            }
        }

        private void GameManagerOnGameFinishedEventHandler(object sender, GameFinishedEventHandlerArgs args) {
            MessageBox.Show(Application.Current.MainWindow, args.IsVictory ? "Game Victory!" : "You loser!");
            StartGame();
        }

        private void GameManagerOnCellStateChangedEventHandler(object sender, CellStateChangedEventHandlerArgs args) {
            _matrixView.ChangeCellState(args.Row, args.Column, args.NewState, args.DisplayString);
        }
    }
}