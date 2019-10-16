using System;
using System.Collections.Generic;
using System.Linq;
using Models.Events;

namespace Models {
    public class GameManager : IGameManager {
        private readonly FieldFactory _fieldFactory;

        private PlaySettings _currentSettings;
        private Field _field;
        private volatile bool _fieldGenerated;

        public GameManager(FieldFactory fieldFactory) {
            _fieldFactory = fieldFactory;
        }

        public event CellStateChangedEventHandler OnCellStateChanged;
        public event GameFinishedEventHandler OnGameFinished;

        public void StartGame(PlaySettings settings) {
            _currentSettings = settings;
            _field = _fieldFactory.Create(_currentSettings);
            _fieldGenerated = true;
        }

        public void ChangeState(byte row, byte column, CellState newState) {
            CheckFieldGenerated();

            var cell = _field[row, column];
            if (!TryChangeState(newState, cell)) {
                return;
            }

            if (cell.IsMineHere) {
                OnGameFinished?.Invoke(this, new GameFinishedEventHandlerArgs(false));
                return;
            }

            if (newState == CellState.Opened && IsSafeCell(cell)) {
                OpenAllSafeCells(cell);
            }

            if (GameFinished()) {
                OnGameFinished?.Invoke(this, new GameFinishedEventHandlerArgs(true));
            }
        }

        private bool GameFinished() {
            return !_field.AllCells.Any(e => !e.IsMineHere && e.State != CellState.Opened);
        }

        private bool TryChangeState(CellState newState, Cell cell) {
            var changeStateResult = cell.TryChangeState(newState, out var oldState);
            OnCellStateChanged?.Invoke(this,
                new CellStateChangedEventHandlerArgs(cell.Row, cell.Column, oldState, newState, changeStateResult));
            return changeStateResult;
        }

        private static bool IsSafeCell(Cell cell) {
            return !cell.IsMineHere && cell.MineAroundCount == 0;
        }

        private void OpenAllSafeCells(Cell cell) {
            var alreadyProcessed = new HashSet<Cell> {
                cell
            };
            var needToProcess = new Stack<Cell>(GetNotOpenedAndNotProcessedNeighbors(cell, alreadyProcessed));
            while (needToProcess.Count > 0) {
                var curCell = needToProcess.Pop();
                if (alreadyProcessed.Contains(curCell)) {
                    continue;
                }

                alreadyProcessed.Add(curCell);

                if (!TryChangeState(CellState.Opened, curCell)) {
                    continue;
                }

                foreach (var neighbor in GetNotOpenedAndNotProcessedNeighbors(curCell, alreadyProcessed)) {
                    needToProcess.Push(neighbor);
                }
            }
        }

        private IEnumerable<Cell> GetNotOpenedAndNotProcessedNeighbors(Cell curCell, HashSet<Cell> alreadyProcessed) {
            return _field.GetNeighbors(curCell).Where(e => e.State != CellState.Opened &&
                                                           !alreadyProcessed.Contains(e));
        }

        private void CheckFieldGenerated() {
            if (_fieldGenerated) {
                return;
            }

            throw new InvalidOperationException("Field must generate before a start the game.");
        }
    }
}