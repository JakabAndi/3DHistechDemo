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
using Global;

namespace _3DHistechDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Engine engineX = new Engine(AxisEnum.X);
            Engine engineY = new Engine(AxisEnum.Y);
            Engine engineZ = new Engine(AxisEnum.Z);
            Table table = new Table(100, 100);
            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel(new List<IEngine>() { engineX, engineY, engineZ }, table);

            DataContext = mainWindowViewModel;
            InitializeComponent();
        }
    }
}