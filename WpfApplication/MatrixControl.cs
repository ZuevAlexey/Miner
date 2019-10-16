using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Models;

namespace WpfApplication {
    public class MatrixControl : FrameworkElement {
        /// <summary>Rows Dependency Property</summary>
        public static readonly DependencyProperty RowsProperty =
            DependencyProperty.Register(
                nameof(Rows),
                typeof(byte),
                typeof(MatrixControl),
                new FrameworkPropertyMetadata(
                    byte.MinValue,
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange,
                    OnRowsChanged));

        /// <summary>Rows Dependency Property</summary>
        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.Register(
                nameof(Columns),
                typeof(byte),
                typeof(MatrixControl),
                new FrameworkPropertyMetadata(
                    byte.MinValue, // default value
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange,
                    OnColumnsChanged));

        private readonly IGameManager _gameManager;

        private Button[,] _buttons;

        public MatrixControl() {
            _gameManager = new GameManager(new FieldFactory(new SimpleMiningAlgorithm()));
            _gameManager.OnGameFinished += (sender, args) => {
                Debug.WriteLine($"GameFinished Fired - {args.IsVictory}");
                MessageBox.Show(Application.Current.MainWindow, args.IsVictory ? "Game Victory!" : "You loser!");
            };
            _gameManager.OnCellStateChanged += (sender, args) => {
                Debug.WriteLine($"OnCellStateChanged Fired - {args}");
                var btn = _buttons[args.Row, args.Column];
                string content = null;
                Brush newBrush = null;
                switch (args.NewState) {
                    case CellState.Closed:
                        content = "";
                        break;
                    case CellState.Opened:
                        content = args.DisplayString;
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
                    btn.Background = newBrush;
                }
                
                btn.Content = content;
            };
            CreateField();
        }

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

        private static void OnRowsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if (d is MatrixControl testCustomControl) {
                testCustomControl.Rows = (byte) e.NewValue;
                testCustomControl.CreateField();
            }
        }

        private static void OnColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if (d is MatrixControl testCustomControl) {
                testCustomControl.Columns = (byte) e.NewValue;
                testCustomControl.CreateField();
            }
        }

        private void CreateField() {
            if (_buttons != null) {
                foreach (var button in _buttons) {
                    RemoveVisualChild(button);
                }
            }

            if (IsEmpty()) {
                _buttons = null;
                return;
            }

            var mineCount = (int) (Columns * Rows * 0.15);
            Application.Current.MainWindow.Title = $"Количество мин - {mineCount}";
            _gameManager.StartGame(new PlaySettings {
                Columns = Columns,
                Rows = Rows,
                MineCount = mineCount
            });

            _buttons = new Button[Rows, Columns];
            for (byte row = 0; row < Rows; row++) {
                for (byte col = 0; col < Columns; col++) {
                    var btn = new Button {
                        DataContext = $"{row};{col}"
                    };

                    btn.PreviewMouseDown += (sender, args) => {
                        Debug.WriteLine("MouseLeftButtonDown event fired");
                        var context = (string) (sender as Button).DataContext;
                        var r = byte.Parse(context.Split(';')[0]);
                        var c = byte.Parse(context.Split(';')[1]);
                        _gameManager.ChangeState(r, c, CellState.Opened);
                    };
                    _buttons[row, col] = btn;
                    AddVisualChild(btn);
                }
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
            return Rows == byte.MinValue || Columns == byte.MinValue;
        }

        protected override Geometry GetLayoutClip(Size layoutSlotSize) {
            return ClipToBounds ? new RectangleGeometry(new Rect(RenderSize)) : null;
        }
    }
}