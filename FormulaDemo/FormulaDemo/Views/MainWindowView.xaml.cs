using System.Windows;
using System.Windows.Input;
using FormulaDemo.ViewModels;

namespace FormulaDemo.Views
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainWindowViewModel();
        }

        //private void UIElement_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    DataObject dragData = new DataObject("FormulaItem", ((FrameworkElement)sender).DataContext);

        //    DragDrop.DoDragDrop(ItemsContainer, dragData, DragDropEffects.Move);
        //}

        //private void UIElement_OnDragEnter(object sender, DragEventArgs e)
        //{

        //}

        //private void UIElement_OnDrop(object sender, DragEventArgs e)
        //{
            
        //}
    }
}
