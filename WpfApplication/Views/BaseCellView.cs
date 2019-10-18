using System;
using System.Windows;
using System.Windows.Controls;
using Models;

namespace WpfApplication.Views {
    public abstract class BaseCellView: FrameworkElement {
        public Position Position { get; set; }
        public bool IsMineHere { get; set; }
        public byte MinesAroundCount { get; set; }

        public CellState State {
            get => (CellState) GetValue(StateProperty);
            set => SetValue(StateProperty, value);
        }

        public static readonly DependencyProperty StateProperty =
            DependencyProperty.Register(
                nameof(State),
                typeof(CellState),
                typeof(BaseCellView),
                new FrameworkPropertyMetadata(
                    CellState.Closed,
                    FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender,
                    OnStateChanged));

        private static void OnStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var cell = d as BaseCellView;
            if (cell != null) {
                cell.State = (CellState) e.NewValue;
                cell.OnStateChangedInternal();
            }
        }

        protected abstract void OnStateChangedInternal();
    }
}