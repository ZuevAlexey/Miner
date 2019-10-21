using System.Diagnostics;
using Models.Extension;

namespace Models {
    /// <summary>
    ///     Фабрика для создания поля со стадиями. Позволяет в дочерних классах переопределить поведение до и полсе минирования
    /// </summary>
    public class StageFieldFactory: IFieldFactory {
        private readonly IMiningAlgorithm _miningAlgorithm;

        /// <summary>
        ///     Конструктор
        /// </summary>
        /// <param name="miningAlgorithm">Алгоритм минирования</param>
        public StageFieldFactory(IMiningAlgorithm miningAlgorithm) {
            _miningAlgorithm = miningAlgorithm;
        }

        /// <inheritdoc />
        public virtual Field Create(GameSettings settings) {
            var result = new Field(settings.Rows, settings.Columns);

            BeforeMining(result, settings);
            _miningAlgorithm.DropMines(result, settings.MinesCount);
            AfterMining(result, settings);

            Debug.WriteLine(result.ToString());
            return result;
        }

        /// <summary>
        ///     Представляет собой добполнительные действия над полем после минирования
        /// </summary>
        /// <param name="field">Поле</param>
        /// <param name="settings">Настройки игры</param>
        protected virtual void AfterMining(Field field, GameSettings settings) {
            field.RecalculateMinesAroundCount();
        }

        /// <summary>
        ///     Представляет собой добполнительные действия над полем перед минированием
        /// </summary>
        /// <param name="field">Поле</param>
        /// <param name="settings">Настройки игры</param>
        protected virtual void BeforeMining(Field field, GameSettings settings) { }
    }
}
