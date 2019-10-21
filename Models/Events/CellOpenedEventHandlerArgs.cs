using System;

namespace Models.Events {
    /// <summary>
    ///     Аргументы события открытия кнопки
    /// </summary>
    public class CellOpenedEventHandlerArgs: EventArgs {
        /// <summary>
        ///     Конструктор
        /// </summary>
        /// <param name="position">Позиция открывшейся ячейки</param>
        /// <param name="isMineHere">Есть ли мина в открытой ячейке?</param>
        /// <param name="minesAroundCount">Количество мин вокруг откытой ячейки</param>
        public CellOpenedEventHandlerArgs(Position position, bool isMineHere, byte minesAroundCount) {
            Position = position;
            IsMineHere = isMineHere;
            MinesAroundCount = minesAroundCount;
        }

        /// <summary>
        ///     Позиция открывшейся ячейки
        /// </summary>
        public Position Position { get; }

        /// <summary>
        ///     Есть ли мина в открытой ячейке?
        /// </summary>
        public bool IsMineHere { get; }

        /// <summary>
        ///     Количество мин вокруг откытой ячейки
        /// </summary>
        public byte MinesAroundCount { get; }

        public override string ToString() {
            return $"{Position.Row}; {Position.Column}; {IsMineHere}; {MinesAroundCount}";
        }
    }
}
