using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Models;

namespace WpfApplication.Views {
    public class MatrixView : FrameworkElement, IMatrixView {
        /// <summary>Rows Dependency Property</summary>
        public static readonly DependencyProperty RowsProperty =
            DependencyProperty.Register(
                nameof(Rows),
                typeof(byte),
                typeof(MatrixView),
                new FrameworkPropertyMetadata(
                    byte.MinValue,
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange,
                    OnRowsChanged));

        /// <summary>Rows Dependency Property</summary>
        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.Register(
                nameof(Columns),
                typeof(byte),
                typeof(MatrixView),
                new FrameworkPropertyMetadata(
                    byte.MinValue, // default value
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange,
                    OnColumnsChanged));

        private Button[,] _buttons;

        /// <summary>
        ///     Style property
        /// </summary>
        public byte Rows {
            get => (byte) GetValue(RowsProperty);
            set => SetValue(RowsProperty, value);
        }

        /// <summary>
        ///     Style property
        /// </summary>
        public byte Columns {
            get => (byte) GetValue(ColumnsProperty);
            set => SetValue(ColumnsProperty, value);
        }

        protected override int VisualChildrenCount => _buttons?.Length ?? 0;

        public event OnCellPressedEventHandler OnCellPressed;

        public void ChangeCellState(byte row, byte column, CellState newState, string displayString) {
            string content = null;
            Brush newBrush = null;

            var button = _buttons[row, column];
            switch (newState) {
                case CellState.Closed:
                    content = "";
                    break;
                case CellState.Opened:
                    content = displayString;
                    if (content == "M") {
                        newBrush = Brushes.Black;
                        break;
                    }

                    var val = byte.Parse(content);
                    if (val > 0) {
                        newBrush = Brushes.Firebrick;
                        break;
                    }

                    newBrush = Brushes.Aquamarine;
                    break;
                case CellState.MineHere:
                    content = "!";
                    break;
                case CellState.Undefined:
                    content = "?";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (newBrush != null) {
                button.Background = newBrush;
            }

            button.Content = content;
            
        }

        public void CreateField(byte rows, byte columns) {
            if (_buttons != null) {
                foreach (var button in _buttons) {
                    RemoveVisualChild(button);
                }
            }

            _buttons = new Button[rows, columns];
            for (byte row = 0; row < rows; row++) {
                for (byte col = 0; col < columns; col++) {
                    var btn = new Button {
                        DataContext = $"{row};{col}"
                    };

                    btn.PreviewMouseDown += (sender, args) => {
                        var context = (string) (sender as Button).DataContext;
                        var r = byte.Parse(context.Split(';')[0]);
                        var c = byte.Parse(context.Split(';')[1]);
                        OnCellPressed?.Invoke(this, new OnCellPressedEventHandlerArgs(r, c, args.ChangedButton));
                    };
                    _buttons[row, col] = btn;
                    AddVisualChild(btn);
                }
            }

            Rows = rows;
            Columns = columns;
        }

        private static void OnRowsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if (d is MatrixView testCustomControl) {
                testCustomControl.Rows = (byte) e.NewValue;
            }
        }

        private static void OnColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if (d is MatrixView testCustomControl) {
                testCustomControl.Columns = (byte) e.NewValue;
            }
        }

        protected override Visual GetVisualChild(int index) {
            return _buttons[index / Columns, index % Columns];
        }

        protected override Size MeasureOverride(Size constraint) {
            if (IsEmpty()) {
                return Size.Empty;
            }

            return constraint;
        }

        protected override Size ArrangeOverride(Size arrangeBounds) {
            if (IsEmpty()) {
                return new Size(0, 0);
            }

            var itemWidth = arrangeBounds.Width / Columns;
            var itemHeight = arrangeBounds.Height / Rows;

            for (byte row = 0; row < Rows; row++) {
                for (byte col = 0; col < Columns; col++) {
                    var rect = new Rect(col * itemWidth, row * itemHeight, itemWidth, itemHeight);
                    _buttons[row, col].Arrange(rect);
                }
            }

            return arrangeBounds;
        }

        private bool IsEmpty() {
            return Rows == byte.MinValue || Columns == byte.MinValue || _buttons == null;
        }

        protected override Geometry GetLayoutClip(Size layoutSlotSize) {
            return ClipToBounds ? new RectangleGeometry(new Rect(RenderSize)) : null;
        }
    }
}