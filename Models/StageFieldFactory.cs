using System.Diagnostics;
using Models.Extension;

namespace Models {
    public class StageFieldFactory : IFieldFactory {
        private readonly IMiningAlgorithm _miningAlgorithm;

        public StageFieldFactory(IMiningAlgorithm miningAlgorithm) {
            _miningAlgorithm = miningAlgorithm;
        }

        public virtual Field Create(GameSettings settings) {
            var result = new Field(settings.Rows, settings.Columns);

            BeforeMining(result, settings);
            _miningAlgorithm.DropMines(result, settings.MinesCount);
            AfterMining(result, settings);

            Debug.WriteLine(result.ToString());
            return result;
        }

        protected virtual void AfterMining(Field field, GameSettings settings) {
            field.RecalculateMinesAroundCount();
        }

        protected virtual void BeforeMining(Field field, GameSettings settings) { }
    }
}