using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Models.Events;
using Models.Extension;

namespace Models {
    /// <summary>
    ///     Менеджер игры сапер с обычными правилами
    /// </summary>
    public class GameManager: IGameManager {
        private readonly IFieldFactory _stageFieldFactory;

        private GameSettings _currentSettings;
        private Field _field;
        private volatile bool _fieldGenerated;
        private volatile bool _gameFinished;
        private volatile bool _gameStarted;

        /// <summary>
        ///     Конструктор
        /// </summary>
        /// <param name="stageFieldFactory">Фабрика полей</param>
        public GameManager(IFieldFactory stageFieldFactory) {
            _stageFieldFactory = stageFieldFactory;
        }

        /// <inheritdoc />
        public event CellOpenedEventHandler OnCellOpened;

        /// <inheritdoc />
        public event GameFinishedEventHandler OnGameFinished;

        /// <inheritdoc />
        public event GameStartedEventHandler OnGameStarted;

        /// <inheritdoc />
        public void StartGame(GameSettings settings) {
            _fieldGenerated = false;
            _gameStarted = false;
            _gameFinished = false;

            _currentSettings = settings;
            _field = _stageFieldFactory.Create(_currentSettings);

            _fieldGenerated = true;
        }

        /// <inheritdoc />
        public void TryOpen(Position position) {
            CanPlay();

            var cell = _field[position];

            if(!_gameStarted) {
                _gameStarted = true;
                OnGameStarted?.Invoke(this, EventArgs.Empty);

                if(cell.IsMineHere && !_currentSettings.CanOpenMineFirstTry) {
                    SwapMine(ref cell);
                }
            }

            if(!TryOpen(cell)) {
                return;
            }

            if(IsBoom(cell)) {
                _gameFinished = true;
                OnGameFinished?.Invoke(this,
                    new GameFinishedEventHandlerArgs(false,
                        _field.AllCells.Where(e => e.IsMineHere).Select(e => e.Position).ToList()));
                return;
            }

            if(IsSafeCell(cell)) {
                OpenAllSafeCells(cell);
            }

            if(IsVictory()) {
                OnGameFinished?.Invoke(this, new GameFinishedEventHandlerArgs(true, null));
            }
        }

        private void SwapMine(ref Cell cell) {
            var targetCell = _field.AllCells.FirstOrDefault(e => !e.IsMineHere);

            if(targetCell == null) {
                return;
            }

            var newOpenedCell = new Cell(cell.Position) {
                MineAroundCount = cell.MineAroundCount
            };

            var newTargetCell = new Cell(targetCell.Position) {
                MineAroundCount = targetCell.MineAroundCount
            };
            newTargetCell.TryDropMine();

            _field.Replace(newOpenedCell);
            _field.Replace(newTargetCell);

            RecalculateMinesCount(new List<Cell> { newOpenedCell, newTargetCell });

            cell = newOpenedCell;

            Debug.WriteLine(
                $"Swap [{newOpenedCell.Position.Row},{newOpenedCell.Position.Column}] and [{newTargetCell.Position.Row},{newTargetCell.Position.Column}]");
            Debug.WriteLine(_field);
        }

        private void RecalculateMinesCount(IEnumerable<Cell> cells) {
            foreach(var recalculatedCell in cells.SelectMany(cell => _field.GetNeighbors(cell))) {
                recalculatedCell.MineAroundCount = _field.GetMinesAroundCount(recalculatedCell);
            }
        }

        private static bool IsBoom(Cell cell) {
            return cell.IsMineHere && cell.IsOpened;
        }

        private bool IsVictory() {
            foreach(var cell in _field.AllCells) {
                if(cell.IsMineHere) {
                    if(cell.IsOpened) {
                        return false;
                    }
                } else {
                    if(!cell.IsOpened) {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool TryOpen(Cell cell) {
            var changeStateResult = cell.TryOpen();
            if(changeStateResult) {
                OnCellOpened?.Invoke(this,
                    new CellOpenedEventHandlerArgs(cell.Position, cell.IsMineHere, cell.MineAroundCount));
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
            var needToProcess = new Stack<Cell>(GetNotOpenedAndNotProcessedNeighbors(cell));
            while(needToProcess.Count > 0) {
                var curCell = needToProcess.Pop();
                if(alreadyProcessed.Contains(curCell)) {
                    continue;
                }

                alreadyProcessed.Add(curCell);

                if(!TryOpen(curCell)) {
                    continue;
                }

                if(!IsSafeCell(curCell)) {
                    continue;
                }

                foreach(var neighbor in GetNotOpenedAndNotProcessedNeighbors(curCell, alreadyProcessed)) {
                    needToProcess.Push(neighbor);
                }
            }
        }

        private IEnumerable<Cell> GetNotOpenedAndNotProcessedNeighbors(Cell curCell,
            HashSet<Cell> alreadyProcessed = null) {
            return _field.GetNeighbors(curCell).Where(e => !e.IsOpened && !(alreadyProcessed?.Contains(e) ?? false));
        }

        private void CanPlay() {
            if(!_fieldGenerated) {
                throw new InvalidOperationException("Field must generate before a start the game.");
            }

            if(_gameFinished) {
                throw new InvalidOperationException("Field must re-generate before a start the new game.");
            }
        }
    }
}
