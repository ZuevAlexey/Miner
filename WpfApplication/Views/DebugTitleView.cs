using System;
using System.Diagnostics;
using System.Timers;
using System.Windows;

namespace WpfApplication.Views {
    public class DebugTitleView : IMinesCountView, ITimerView {
        private readonly Stopwatch _stopwatch;
        private readonly Timer _timer;
        private int _minesCount;

        public DebugTitleView() {
            _timer = new Timer(TimeSpan.FromSeconds(1).TotalMilliseconds);
            _timer.Elapsed += (sender, args) => { Show(); };

            _stopwatch = new Stopwatch();
        }

        public int MinesCount {
            get => _minesCount;
            set {
                _minesCount = value;
                Show();
            }
        }

        public void Start() {
            _timer.Start();
            _stopwatch.Restart();
        }

        public void Stop() {
            _timer.Stop();
            _stopwatch.Stop();
        }

        private void Show() {
            var newTitle = $"Mines count = {_minesCount}; Time elapsed = {(int) _stopwatch.Elapsed.TotalSeconds}";
            Application.Current.Dispatcher.Invoke(() => { Application.Current.MainWindow.Title = newTitle; });
        }
    }
}