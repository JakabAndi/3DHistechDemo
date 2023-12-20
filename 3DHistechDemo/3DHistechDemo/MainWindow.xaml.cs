using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _3DHistechDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Engine engineX = new Engine();
            Engine engineY = new Engine();
            Engine engineZ = new Engine();
            Table table = new Table();
            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel(new List<IEngine>() { engineX, engineY, engineZ }, table);

            this.DataContext = mainWindowViewModel;
            InitializeComponent();
        }
    }
}