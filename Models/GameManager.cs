using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Models.Events;

namespace Models {
    public class GameManager : IGameManager {
        private readonly FieldFactory _fieldFactory;

        private PlaySettings _currentSettings;
        private Field _field;
        private volatile bool _fieldGenerated;
        private volatile bool _gameFinished;
        private volatile bool _gameStarted;

        public GameManager(FieldFactory fieldFactory) {
            _fieldFactory = fieldFactory;
        }

        public CellState GetCellState(byte row, byte column) {
            return _field[row, column].State;
        }

        public event CellStateChangedEventHandler OnCellStateChanged;
        public event GameFinishedEventHandler OnGameFinished;
        public event GameStartedEventHandler OnGameStarted;

        public void StartGame(PlaySettings settings) {
            _fieldGenerated = false;
            _gameStarted = false;
            _gameFinished = false;

            _currentSettings = settings;
            _field = _fieldFactory.Create(_currentSettings);
            _fieldGenerated = true;
        }

        public void TryChangeState(byte row, byte column, CellState newState) {
            CanPlay();

            var cell = _field[row, column];

            if (!_gameStarted) {
                _gameStarted = true;
                OnGameStarted?.Invoke(this, EventArgs.Empty);

                if (cell.IsMineHere && newState == CellState.Opened && !_currentSettings.CanOpenMineFirstTry) {
                    SwapMine(ref cell);
                }
            }

            if (!TryChangeState(newState, cell)) {
                return;
            }

            if (IsBoom(cell)) {
                _gameFinished = true;
                OnGameFinished?.Invoke(this, new GameFinishedEventHandlerArgs(false));
                return;
            }

            if (newState == CellState.Opened && IsSafeCell(cell)) {
                OpenAllSafeCells(cell);
            }

            if (IsVictory()) {
                OnGameFinished?.Invoke(this, new GameFinishedEventHandlerArgs(true));
            }
        }

        private void SwapMine(ref Cell cell) {
            var targetCell = _field.AllCells.First(e => !e.IsMineHere);

            var newOpenedCell = new Cell(cell.Row, cell.Column) {
                MineAroundCount = cell.MineAroundCount
            };

            var newTargetCell = new Cell(targetCell.Row, targetCell.Column) {
                MineAroundCount = targetCell.MineAroundCount
            };
            newTargetCell.TryDropMine();

            _field.Replace(newOpenedCell);
            _field.Replace(newTargetCell);

            RecalculateMinesCount(new List<Cell> {newOpenedCell, newTargetCell});

            var newState = cell.State;
            cell = newOpenedCell;
            cell.TryChangeState(newState, out var oldState);

            Debug.WriteLine(
                $"Swap [{newOpenedCell.Row},{newOpenedCell.Column}] and [{newTargetCell.Row},{newTargetCell.Column}]");
            Debug.WriteLine(_field);
        }

        private void RecalculateMinesCount(IEnumerable<Cell> cells) {
            foreach (var recalculatedCell in cells.SelectMany(cell => _field.GetNeighbors(cell))) {
                FieldFactory.CalculateMineCount(_field, recalculatedCell);
            }
        }

        private static bool IsBoom(Cell cell) {
            return cell.IsMineHere && cell.State == CellState.Opened;
        }

        private bool IsVictory() {
            foreach (var cell in _field.AllCells) {
                if (cell.IsMineHere) {
                    if (cell.State == CellState.Opened) {
                        return false;
                    }
                } else {
                    if (cell.State != CellState.Opened) {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool TryChangeState(CellState newState, Cell cell) {
            var changeStateResult = cell.TryChangeState(newState, out var oldState);
            if (changeStateResult) {
                OnCellStateChanged?.Invoke(this,
                    new CellStateChangedEventHandlerArgs(cell.Row, cell.Column, newState, oldState, cell.DisplayString));
            }
            
            return changeStateResult;
        }

        private static bool IsSafeCell(Cell cell) {
            return !cell.IsMineHere && cell.MineAroundCount == 0;
        }

        private void OpenAllSafeCells(Cell cell) {
            var alreadyProcessed = new HashSet<Cell> {
                cell
            };
            var needToProcess = new Stack<Cell>(GetNotOpenedAndNotProcessedNeighbors(cell, null));
            while (needToProcess.Count > 0) {
                var curCell = needToProcess.Pop();
                if (alreadyProcessed.Contains(curCell)) {
                    continue;
                }

                alreadyProcessed.Add(curCell);

                if (!TryChangeState(CellState.Opened, curCell)) {
                    continue;
                }

                if (!IsSafeCell(curCell)) {
                    continue;
                }

                foreach (var neighbor in GetNotOpenedAndNotProcessedNeighbors(curCell, alreadyProcessed)) {
                    needToProcess.Push(neighbor);
                }
            }
        }

        private IEnumerable<Cell> GetNotOpenedAndNotProcessedNeighbors(Cell curCell, HashSet<Cell> alreadyProcessed = null) {
            return _field.GetNeighbors(curCell).Where(e => e.State != CellState.Opened &&
                                                           !(alreadyProcessed?.Contains(e) ?? false));
        }

        private void CanPlay() {
            if (!_fieldGenerated) {
                throw new InvalidOperationException("Field must generate before a start the game.");
            }

            if (_gameFinished) {
                throw new InvalidOperationException("Field must re-generate before a start the new game.");
            }
        }
    }
}