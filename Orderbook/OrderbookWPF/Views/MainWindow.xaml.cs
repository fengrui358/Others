using System.Windows;
using OrderbookWPF.ViewModels;

namespace OrderbookWPF.Views
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainWindowModel();
        }
    }
}
