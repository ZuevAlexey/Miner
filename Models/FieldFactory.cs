using System.Diagnostics;
using System.Linq;

namespace Models {
    public class FieldFactory {
        private readonly IMiningAlgorithm _miningAlgorithm;

        public FieldFactory(IMiningAlgorithm miningAlgorithm) {
            _miningAlgorithm = miningAlgorithm;
        }

        public Field Create(PlaySettings settings) {
            var result = new Field(settings.Rows, settings.Columns);
            for (byte i = 0; i < result.Rows; i++) {
                for (byte j = 0; j < result.Columns; j++) {
                    result[i, j] = new Cell(i, j);
                }
            }

            BeforeMining(result, settings);
            _miningAlgorithm.DropMines(result, settings);
            AfterMining(result, settings);

            Debug.WriteLine(result.ToString());
            return result;
        }

        protected virtual void AfterMining(Field field, PlaySettings settings) {
            CalculateMineCount(field);
        }

        protected void CalculateMineCount(Field field) {
            foreach (var cell in field.AllCells) {
                if (cell.IsMineHere) {
                    continue;
                }

                var neighbors = field.GetCellsAround(cell).ToList();
                cell.MineAroundCount = (byte) neighbors.Sum(e => e.MineCount);
            }
        }

        protected virtual void BeforeMining(Field field, PlaySettings settings) { }
    }
}