using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using Models;

namespace WpfApplication.Views {
    public class DebugCellView : BaseCellView {
        private static readonly Brush _defaultBrush = Brushes.Cornsilk;

        private Brush _brush = _defaultBrush;
        private string _content = "";

        protected override void OnRender(DrawingContext drawingContext) {
            base.OnRender(drawingContext);

            drawingContext.DrawRectangle(_brush, new Pen(Brushes.Black, 3),
                new Rect(new Point(3, 3), new Point(ActualWidth - 3, ActualHeight - 3)));
            var formattedText = new FormattedText(
                _content,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface("Times new roman"),
                Math.Pow(Math.Pow(ActualWidth, 2) + Math.Pow(ActualHeight, 2), 0.5) * 0.4,
                Brushes.Black, VisualTreeHelper.GetDpi(this).PixelsPerDip);

            var textLocation = new Point(ActualWidth / 2 - formattedText.WidthIncludingTrailingWhitespace / 2,
                ActualHeight / 2 - formattedText.Height / 2);
            drawingContext.DrawText(formattedText, textLocation);
        }

        protected override void OnStateChangedInternal() {
            switch (State) {
                case CellState.Closed:
                    _content = "";
                    _brush = _defaultBrush;
                    break;
                case CellState.Opened:
                    if (IsMineHere) {
                        _brush = Brushes.Black;
                        break;
                    }

                    _brush = MinesAroundCount == 0
                        ? Brushes.Green
                        : Brushes.Brown;
                    _content = MinesAroundCount == 0
                        ? ""
                        : MinesAroundCount.ToString();
                    break;
                case CellState.MineHere:
                    _content = "!";
                    break;
                case CellState.Undefined:
                    _content = "?";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}