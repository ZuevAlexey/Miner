using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Models;
using WpfApplication.Views.Events;

namespace WpfApplication.Views {
    public class MatrixView<T> : FrameworkElement, IMatrixView where T:BaseCellView, new() {
        /// <summary>Rows Dependency Property</summary>
        public static readonly DependencyProperty RowsProperty =
            DependencyProperty.Register(
                nameof(Rows),
                typeof(byte),
                typeof(MatrixView<T>),
                new FrameworkPropertyMetadata(
                    byte.MinValue,
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange,
                    OnRowsChanged));

        /// <summary>Rows Dependency Property</summary>
        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.Register(
                nameof(Columns),
                typeof(byte),
                typeof(MatrixView<T>),
                new FrameworkPropertyMetadata(
                    byte.MinValue, // default value
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange,
                    OnColumnsChanged));

        private BaseCellView[,] _cells;

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

        protected override int VisualChildrenCount => _cells?.Length ?? 0;

        public event OnCellPressedEventHandler OnCellPressed;

        public void ChangeCellState(Position position, CellState newState, bool isMineHere, byte minesAroundCount) {
            var cell = _cells[position.Row, position.Column];
            cell.MinesAroundCount = minesAroundCount;
            cell.IsMineHere = isMineHere;
            cell.State = newState;
        }

        public void CreateField(byte rows, byte columns) {
            if (rows == Rows && columns == Columns) {
                foreach (var cell in _cells) {
                    cell.State = CellState.Closed;
                }

                return;
            }
            
            if (_cells != null) {
                foreach (var button in _cells) {
                    RemoveVisualChild(button);
                }
            }

            _cells = new BaseCellView[rows, columns];
            
            for (byte row = 0; row < rows; row++) {
                for (byte col = 0; col < columns; col++) {
                    var cell = new T {
                        Position = new Position(row, col),
                        State = CellState.Closed
                    };

                    cell.PreviewMouseDown += CellPreviewMouseDownEventHandler;
                    _cells[row, col] = cell;
                    AddVisualChild(cell);
                }
            }

            Rows = rows;
            Columns = columns;
        }

        public CellState GetCellState(Position position) {
            return _cells[position.Row, position.Column].State;
        }

        private void CellPreviewMouseDownEventHandler(object sender, MouseButtonEventArgs args) {
            var cell = (sender as T);
            OnCellPressed?.Invoke(this, new OnCellPressedEventHandlerArgs(cell.Position, args.ChangedButton));
        }

        private static void OnRowsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if (d is MatrixView<T> matrixView) {
                matrixView.Rows = (byte) e.NewValue;
            }
        }

        private static void OnColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if (d is MatrixView<T> matrixView) {
                matrixView.Columns = (byte) e.NewValue;
            }
        }

        protected override Visual GetVisualChild(int index) {
            return _cells[index / Columns, index % Columns];
        }

        protected override Size MeasureOverride(Size constraint) {
            if (constraint.Width < 50 || constraint.Height < 50) {
                return new Size(50, 50); 
            }
            
            return constraint;
        }

        protected override Size ArrangeOverride(Size arrangeBounds) {
            if (IsEmpty()) {
                return new Size(0, 0);
            }
            
            var result = new Size(Math.Max(500, arrangeBounds.Width), Math.Max(500, arrangeBounds.Height));

            var itemWidth = result.Width / Columns;
            var itemHeight = result.Height / Rows;

            for (byte row = 0; row < Rows; row++) {
                for (byte col = 0; col < Columns; col++) {
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