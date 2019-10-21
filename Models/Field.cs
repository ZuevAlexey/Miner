using System;
using System.Collections.Generic;
using System.Text;

namespace Models {
    /// <summary>
    ///     Представляет собой поле игры
    /// </summary>
    public class Field {
        private readonly Cell[,] _cells;

        /// <summary>
        ///     Конструктор
        /// </summary>
        /// <param name="rows">Количество строк поля</param>
        /// <param name="columns">Количество столбцой поля</param>
        public Field(byte rows, byte columns) {
            _cells = new Cell[rows, columns];
            for(byte i = 0;i < rows;i++) {
                for(byte j = 0;j < columns;j++) {
                    _cells[i, j] = new Cell(new Position(i, j));
                }
            }

            Columns = columns;
            Rows = rows;
        }

        /// <summary>
        ///     Размер поля
        /// </summary>
        public int Size => _cells.Length;

        /// <summary>
        ///     Количество строк
        /// </summary>
        private byte Rows { get; }

        /// <summary>
        ///     Количество столбцов
        /// </summary>
        private byte Columns { get; }

        public virtual Cell this[Position position] {
            get => _cells[position.Row, position.Column];
            protected set => _cells[position.Row, position.Column] = value;
        }

        public virtual Cell this[int index] => _cells[index / Columns, index % Columns];

        /// <summary>
        ///     Получить все ячейки поля
        /// </summary>
        public IEnumerable<Cell> AllCells {
            get {
                for(var row = 0;row < Rows;row++) {
                    for(var col = 0;col < Columns;col++) {
                        yield return _cells[row, col];
                    }
                }
            }
        }

        /// <summary>
        ///     Заменить ячейку в поле. Новая ячейка вставляет на ту позицию, которая в ней указана
        /// </summary>
        /// <param name="newCell">Новая ячейка</param>
        public void Replace(Cell newCell) {
            this[newCell.Position] = newCell;
        }

        /// <summary>
        ///     Получить соседей ячейки
        /// </summary>
        /// <param name="target">Ячейка, для которой требуется получить соседей</param>
        /// <returns>Соседи ячейки</returns>
        public IEnumerable<Cell> GetNeighbors(Cell target) {
            var top = Math.Max(0, target.Position.Row - 1);
            var bottom = Math.Min(Rows - 1, target.Position.Row + 1);
            var left = Math.Max(0, target.Position.Column - 1);
            var right = Math.Min(Columns - 1, target.Position.Column + 1);

            for(var row = top;row <= bottom;row++) {
                for(var col = left;col <= right;col++) {
                    if(row != target.Position.Row || col != target.Position.Column) {
                        yield return _cells[row, col];
                    }
                }
            }
        }

        public override string ToString() {
            var sb = new StringBuilder();
            sb.AppendLine(new string('_', Columns * 3));
            for(var row = 0;row < Rows;row++) {
                for(var col = 0;col < Columns;col++) {
                    sb.Append($" {_cells[row, col].DisplayString} |");
                }

                sb.AppendLine();
            }

            sb.AppendLine(new string('_', Columns * 3));
            return sb.ToString();
        }
    }
}
