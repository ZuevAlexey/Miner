using System.Windows.Input;

namespace WpfApplication {
    public class Commands {
        static Commands() {
            StartNewGame = new RoutedCommand("StartNewGame", typeof(MainWindow));
            ChangeComplexity = new RoutedCommand("StartNewGame", typeof(MainWindow));
        }

        public static RoutedCommand StartNewGame { get; }
        public static RoutedCommand ChangeComplexity { get; }
    }
}
