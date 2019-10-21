using System.Windows.Input;
using Models;
using WpfApplication.Settings;
using WpfApplication.Views;

namespace WpfApplication {
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow {
        private readonly Presenter _presenter;
        private readonly SettingsStorage _settingsStorage;

        public MainWindow() {
            InitializeComponent();
            var mainWindowView = new DebugTitleView();
            var gameManager = new GameManager(new StageFieldFactory(new SimpleMiningAlgorithm()));
            _settingsStorage = SettingsStorage.Load();
            _settingsStorage.OnComplexityChanged += StartGame;

            _presenter = new Presenter(Field, gameManager, mainWindowView, mainWindowView);
            StartGame();
        }

        private void StartGame() {
            _presenter.StartGame(_settingsStorage.Current);
        }

        private void StartNewGame_Executed(object sender, ExecutedRoutedEventArgs e) {
            StartGame();
        }

        private void ChangeComplexity_Executed(object sender, ExecutedRoutedEventArgs e) {
            _settingsStorage.Complexity = (Complexity) e.Parameter;
        }
    }
}
