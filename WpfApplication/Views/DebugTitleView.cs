using System;
using System.Diagnostics;
using System.Timers;
using System.Windows;

namespace WpfApplication.Views {
    /// <summary>
    /// Простое отображение таймера и счетчика бомб в заголовке главного окна. Подойдет для ручного тестирования логики игры и логики интерфейса
    /// </summary>
    public class DebugTitleView: IMinesCountView, ITimerView {
        private readonly Stopwatch _stopwatch;
        private readonly Timer _timer;
        private int _minesCount;

        public DebugTitleView() {
            _timer = new Timer(TimeSpan.FromSeconds(1).TotalMilliseconds);
            _timer.Elapsed += (sender, args) => { Show(); };

            _stopwatch = new Stopwatch();
        }

        /// <inheritdoc/>
        public int MinesCount {
            get => _minesCount;
            set {
                _minesCount = value;
                Show();
            }
        }

        /// <inheritdoc/>
        public void Start() {
            _timer.Start();
            _stopwatch.Restart();
        }

        /// <inheritdoc/>
        public void Stop() {
            _timer.Stop();
            _stopwatch.Stop();
            Show();
        }

        /// <inheritdoc/>
        public void Reset() {
            _timer.Stop();
            _stopwatch.Reset();
            Show();
        }

        private void Show() {
            var newTitle = $"Mines count = {_minesCount}; Time elapsed = {(int) _stopwatch.Elapsed.TotalSeconds}";
            Application.Current.Dispatcher.Invoke(() => { Application.Current.MainWindow.Title = newTitle; });
        }
    }
}
