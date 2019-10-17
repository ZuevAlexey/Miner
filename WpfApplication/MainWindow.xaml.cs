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
            var mainWindowView = new MainWindowTitleView();
            _presenter = new Presenter(Field, new GameManager(new FieldFactory(new SimpleMiningAlgorithm())),
                mainWindowView, mainWindowView);
            _presenter.StartGame(new PlaySettings {
                Columns = 9,
                Rows = 9,
                MineCount = 10,
                CanOpenMineFirstTry = false
            });
        }
    }
}