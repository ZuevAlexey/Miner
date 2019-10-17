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
                CalculateMineCount(field, cell);
            }
        }

        public static void CalculateMineCount(Field field, Cell cell) {
            var neighbors = field.GetNeighbors(cell).ToList();
            cell.MineAroundCount = (byte) neighbors.Count(e => e.IsMineHere);
        }

        protected virtual void BeforeMining(Field field, PlaySettings settings) { }
    }
}