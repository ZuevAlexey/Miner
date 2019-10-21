using System;
using System.Collections.Generic;

namespace Models.Events {
    /// <summary>
    ///     Аргументы события завершения игры
    /// </summary>
    public class GameFinishedEventHandlerArgs: EventArgs {
        /// <summary>
        ///     Конструктор
        /// </summary>
        /// <param name="isVictory">Выйграна ли игра?</param>
        /// <param name="minePositions">Позиции всех мин</param>
        public GameFinishedEventHandlerArgs(bool isVictory, List<Position> minePositions) {
            IsVictory = isVictory;
            MinePositions = minePositions;
        }

        /// <summary>
        ///     Выйграна ли игра?
        /// </summary>
        public bool IsVictory { get; }

        /// <summary>
        ///     Позиции всех мин
        /// </summary>
        public List<Position> MinePositions { get; }
    }
}
