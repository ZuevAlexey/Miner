using System.Windows;
using Models;

namespace WpfApplication.Views.Cell {
    /// <summary>
    /// Базовый элемент представления ячейки. Дочерние классы должны дополнительно переопределеить метод <see cref="UIElement.OnRender"/>
    /// </summary>
    public abstract class BaseCellView: FrameworkElement {
        public static readonly DependencyProperty StateDataProperty =
            DependencyProperty.Register(
                nameof(StateData),
                typeof(CellData),
                typeof(BaseCellView),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange |
                    FrameworkPropertyMetadataOptions.AffectsRender,
                    OnStateDataChanged));

        /// <summary>
        /// Позиция ячейки
        /// </summary>
        public Position Position { get; set; }

        /// <summary>
        /// Данные ячейки
        /// </summary>
        public CellData StateData {
            get => (CellData) GetValue(StateDataProperty);
            set => SetValue(StateDataProperty, value);
        }

        private static void OnStateDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var cell = d as BaseCellView;
            if(cell != null) {
                cell.StateData = (CellData) e.NewValue;
                cell.OnStateChangedInternal();
            }
        }

        /// <summary>
        /// Изменение состояния ячейки. Дочерний класс должен изменить свое визуальное отображение на основе нового соcтояния
        /// </summary>
        protected abstract void OnStateChangedInternal();
    }
}
