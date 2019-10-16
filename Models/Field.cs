using System;
using System.Collections.Generic;
using System.Text;

namespace Models {
    public class Field {
        private readonly Cell[,] _cells;

        public Field(byte rows, byte columns) {
            _cells = new Cell[rows, columns];
            Columns = columns;
            Rows = rows;
        }

        public int Size => _cells.Length;
        public byte Rows { get; }
        public byte Columns { get; }

        public virtual Cell this[int row, int column] {
            get => _cells[row, column];
            protected internal set => _cells[row, column] = value;
        }

        public virtual Cell this[int index] {
            get => _cells[index / Columns, index % Columns];
            protected internal set => _cells[index / Columns, index % Columns] = value;
        }

        public IEnumerable<Cell> AllCells {
            get {
                for (var row = 0; row < Rows; row++) {
                    for (var col = 0; col < Columns; col++) {
                        yield return _cells[row, col];
                    }
                }
            }
        }

        public IEnumerable<Cell> GetNeighbors(Cell target) {
            var top = Math.Max(0, target.Row - 1);
            var bottom = Math.Min(Rows - 1, target.Row + 1);
            var left = Math.Max(0, target.Column - 1);
            var right = Math.Min(Columns - 1, target.Column + 1);

            for (var row = top; row <= bottom; row++) {
                for (var col = left; col <= right; col++) {
                    if (row != target.Row || col != target.Column) {
                        yield return _cells[row, col];
                    }
                }
            }
        }

        public override string ToString() {
            var sb = new StringBuilder();
            sb.AppendLine(new string('_', Columns * 3));
            for (var row = 0; row < Rows; row++) {
                for (var col = 0; col < Columns; col++) {
                    sb.Append($" {_cells[row, col].DisplayString} |");
                }

                sb.AppendLine();
            }

            sb.AppendLine(new string('_', Columns * 3));
            return sb.ToString();
        }
    }
}