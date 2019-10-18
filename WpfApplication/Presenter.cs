using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Models;
using Models.Events;
using WpfApplication.Views;
using WpfApplication.Views.Events;

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
        private readonly IMinesCountView _minesCountView;
        private readonly ITimerView _timerView;
        private PlaySettings _currentSettings;

        public Presenter(IMatrixView matrixView, IGameManager gameManager, ITimerView timerView,
                         IMinesCountView minesCountView) {
            _matrixView = matrixView;
            _gameManager = gameManager;
            _timerView = timerView;
            _minesCountView = minesCountView;

            _gameManager.OnCellStateChanged += GameManagerCellStateChangedEventHandler;
            _gameManager.OnGameFinished += GameManagerGameFinishedEventHandler;
            _gameManager.OnGameStarted += GameManagerGameStarted;

            _matrixView.OnCellPressed += MatrixViewCellPressedEventHandler;
        }

        private void GameManagerGameStarted(object sender, EventArgs args) {
            _timerView.Start();
        }

        public async Task StartGame(PlaySettings settings) {
            _currentSettings = settings;
            await Task.Run(() => _gameManager.StartGame(settings));
            _matrixView.CreateField(settings.Rows, settings.Columns);
            _minesCountView.MinesCount = settings.MineCount;
        }

        private void MatrixViewCellPressedEventHandler(object sender, OnCellPressedEventHandlerArgs args) {
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
                _gameManager.TryChangeState(row, column, nextState);
            }
        }

        private void HandleLeftClick(OnCellPressedEventHandlerArgs args, CellState curState) {
            if (curState == CellState.Closed) {
                _gameManager.TryChangeState(args.Row, args.Column, CellState.Opened);
            }
        }

        private void GameManagerGameFinishedEventHandler(object sender, GameFinishedEventHandlerArgs args) {
            _timerView.Stop();
            var playAgain = MessageBox.Show(Application.Current.MainWindow,
                args.IsVictory ? "Game Victory! Play again?" : "You loser! Play again?", "Game Result",
                MessageBoxButton.YesNo);
            if (playAgain == MessageBoxResult.Yes) {
                StartGame(_currentSettings);
            }
        }

        private void GameManagerCellStateChangedEventHandler(object sender, CellStateChangedEventHandlerArgs args) {
            _matrixView.ChangeCellState(args.Row, args.Column, args.NewState, args.DisplayString);

            if (args.NewState == CellState.MineHere) {
                _minesCountView.MinesCount--;
                return;
            }

            if (args.OldState == CellState.MineHere) {
                _minesCountView.MinesCount++;
            }
        }
    }
}