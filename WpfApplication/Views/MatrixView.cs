using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Models;
using WpfApplication.Views.Cell;
using WpfApplication.Views.Cell.Events;

namespace WpfApplication.Views {
    public class MatrixView<T>: FrameworkElement, IMatrixView where T: BaseCellView, new() {
        public static readonly DependencyProperty RowsProperty =
            DependencyProperty.Register(
                nameof(Rows),
                typeof(byte),
                typeof(MatrixView<T>),
                new FrameworkPropertyMetadata(
                    byte.MinValue,
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange,
                    OnRowsChanged));

        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.Register(
                nameof(Columns),
                typeof(byte),
                typeof(MatrixView<T>),
                new FrameworkPropertyMetadata(
                    byte.MinValue, // default value
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange,
                    OnColumnsChanged));

        public static readonly DependencyProperty CellHeightProperty =
            DependencyProperty.Register(
                nameof(CellHeight),
                typeof(int),
                typeof(MatrixView<T>),
                new FrameworkPropertyMetadata(
                    0,
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange,
                    OnCellHeightChanged));

        public static readonly DependencyProperty CellWidthProperty =
            DependencyProperty.Register(
                nameof(CellWidth),
                typeof(int),
                typeof(MatrixView<T>),
                new FrameworkPropertyMetadata(
                    0,
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange,
                    OnCellWidthChanged));

        private BaseCellView[,] _cells;

        /// <summary>
        /// Высота ячейки
        /// </summary>
        public int CellHeight {
            get => (int) GetValue(CellHeightProperty);
            set => SetValue(CellHeightProperty, value);
        }

        /// <summary>
        /// Ширина ячейки
        /// </summary>
        public int CellWidth {
            get => (int) GetValue(CellWidthProperty);
            set => SetValue(CellWidthProperty, value);
        }

        /// <summary>
        /// Количество строк
        /// </summary>
        private byte Rows {
            get => (byte) GetValue(RowsProperty);
            set => SetValue(RowsProperty, value);
        }

        /// <summary>
        /// Количество столбцов
        /// </summary>
        private byte Columns {
            get => (byte) GetValue(ColumnsProperty);
            set => SetValue(ColumnsProperty, value);
        }

        protected override int VisualChildrenCount => _cells?.Length ?? 0;

        /// <inheritdoc />
        public event OnCellPressedEventHandler OnCellPressed;

        /// <inheritdoc />
        public void ChangeCellState(Position position, CellData newState) {
            var cell = _cells[position.Row, position.Column];
            cell.StateData = newState;
        }

        /// <inheritdoc />
        public void CreateField(byte rows, byte columns) {
            if(rows == Rows && columns == Columns) {
                foreach(var cell in _cells) {
                    cell.StateData = new CellData(CellState.Closed);
                }

                return;
            }

            if(_cells != null) {
                foreach(var button in _cells) {
                    RemoveVisualChild(button);
                }
            }

            _cells = new BaseCellView[rows, columns];

            for(byte row = 0;row < rows;row++) {
                for(byte col = 0;col < columns;col++) {
                    var cell = new T {
                        Position = new Position(row, col),
                        StateData = new CellData(CellState.Closed)
                    };

                    cell.PreviewMouseDown += CellPreviewMouseDownEventHandler;
                    _cells[row, col] = cell;
                    AddVisualChild(cell);
                }
            }

            Rows = rows;
            Columns = columns;
        }

        /// <inheritdoc />
        public CellState GetCellState(Position position) {
            return _cells[position.Row, position.Column].StateData.State;
        }

        private void CellPreviewMouseDownEventHandler(object sender, MouseButtonEventArgs args) {
            if(sender is T cell) {
                OnCellPressed?.Invoke(this, new OnCellPressedEventHandlerArgs(cell.Position, args.ChangedButton));
            }
        }

        private static void OnRowsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if(d is MatrixView<T> matrixView) {
                matrixView.Rows = (byte) e.NewValue;
            }
        }

        private static void OnColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if(d is MatrixView<T> matrixView) {
                matrixView.Columns = (byte) e.NewValue;
            }
        }

        private static void OnCellHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if(d is MatrixView<T> matrixView) {
                matrixView.CellHeight = (int) e.NewValue;
            }
        }

        private static void OnCellWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if(d is MatrixView<T> matrixView) {
                matrixView.CellWidth = (int) e.NewValue;
            }
        }

        protected override Visual GetVisualChild(int index) {
            return _cells[index / Columns, index % Columns];
        }

        protected override Size MeasureOverride(Size constraint) {
            return GetSize();
        }

        private Size GetSize() {
            return new Size(CellWidth * Columns, CellHeight * Rows);
        }

        protected override Size ArrangeOverride(Size arrangeBounds) {
            if(IsEmpty()) {
                return new Size(0, 0);
            }

            var result = GetSize();

            var itemWidth = result.Width / Columns;
            var itemHeight = result.Height / Rows;

            for(byte row = 0;row < Rows;row++) {
                for(byte col = 0;col < Columns;col++) {
                    var rect = new Rect(col * itemWidth, row * itemHeight, itemWidth, itemHeight);
                    _cells[row, col].Arrange(rect);
                }
            }

            return result;
        }

        private bool IsEmpty() {
            return Rows == byte.MinValue || Columns == byte.MinValue || _cells == null;
        }

        protected override Geometry GetLayoutClip(Size layoutSlotSize) {
            return ClipToBounds ? new RectangleGeometry(new Rect(RenderSize)) : null;
        }
    }
}
