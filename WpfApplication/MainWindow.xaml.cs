using System;
using System.Windows.Input;

namespace WpfApplication {
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow {
        public MainWindow() {
            InitializeComponent();
            KeyDown += (sender, args) => {
                switch (args.Key) {
                    case Key.Down:
                        Field.Rows++;
                        break;
                    case Key.Up:
                        Field.Rows--;
                        break;
                    case Key.Left:
                        Field.Columns--;
                        break;
                    case Key.Right:
                        Field.Columns++;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            };
        }
    }
}