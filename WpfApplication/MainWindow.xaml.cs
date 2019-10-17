using Models;

namespace WpfApplication {
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow {
        private readonly Presenter _presenter;
        
        public MainWindow() {
            InitializeComponent();
            _presenter = new Presenter(Field, new GameManager(new FieldFactory(new SimpleMiningAlgorithm())));
            _presenter.StartGame();
        }
    }
}