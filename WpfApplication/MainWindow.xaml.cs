using System.Diagnostics;
using Models;
using WpfApplication.Views;

namespace WpfApplication {
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow {
        private readonly Presenter _presenter;

        public MainWindow() {
            InitializeComponent();
            var mainWindowView = new DebugTitleView();
            var gameManager = new GameManager(new StageFieldFactory(new SimpleMiningAlgorithm()));
            gameManager.OnCellOpened += (sender, args) => Debug.WriteLine($"OnCellOpened {args}");
            gameManager.OnGameFinished += (sender, args) => Debug.WriteLine($"GameFinished {args.IsVictory}");
            gameManager.OnGameStarted += (sender, args) => Debug.WriteLine($"Game Started");
            
            
            _presenter = new Presenter(Field, gameManager, mainWindowView, mainWindowView);
            _presenter.StartGame(new PlaySettings {
                Columns = 9,
                Rows = 9,
                MinesCount = 10,
                CanOpenMineFirstTry = false
            });
        }
    }
}