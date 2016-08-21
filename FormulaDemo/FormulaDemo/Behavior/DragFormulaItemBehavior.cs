using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using FormulaDemo.Adorners;
using FormulaDemo.Models;
using FormulaDemo.UiHelper;

namespace FormulaDemo.Behavior
{
    public class DragFormulaItemBehavior : Behavior<UIElement>
    {
        DateTime mStartHoverTime = DateTime.MinValue;
        TreeViewItem mHoveredItem = null;
        AdornerLayer mAdornerLayer = null;

        protected override void OnAttached()
        {
            //Window parent = Application.Current.MainWindow;
            //AssociatedObject.RenderTransform = transform;

            //AssociatedObject.MouseLeftButtonDown += OnMouseLeftButtonDown;

            //AssociatedObject.MouseLeftButtonUp += (sender, e) =>
            //{
            //    AssociatedObject.ReleaseMouseCapture();
            //};

            //AssociatedObject.MouseMove += (sender, e) =>
            //{
            //    Vector diff = e.GetPosition(parent) - mouseStartPosition;
            //    if (AssociatedObject.IsMouseCaptured)
            //    {
            //        transform.X = diff.X;
            //        transform.Y = diff.Y;
            //    }
            //};

            AssociatedObject.PreviewMouseMove += AssociatedObjectOnPreviewMouseMove;

            AssociatedObject.QueryContinueDrag += AssociatedObjectOnQueryContinueDrag;
        }

        private void AssociatedObjectOnQueryContinueDrag(object sender, QueryContinueDragEventArgs queryContinueDragEventArgs)
        {
            //mAdornerLayer.Update();
        }

        private void AssociatedObjectOnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton != MouseButtonState.Pressed)
                return;

            var associatedObject = (FrameworkElement) sender;

            var panel = Utility.FindVisualParent<Panel>(associatedObject);

            //Point pos = e.GetPosition(panel);
            //HitTestResult result = VisualTreeHelper.HitTest(panel, pos);
            //if (result == null)
            //    return;

            //ListBoxItem listBoxItem = Utils.FindVisualParent<ListBoxItem>(result.VisualHit); // Find your actual visual you want to drag
            //if (listBoxItem == null || listBoxItem.Content != mListBox.SelectedItem || !(mListBox.SelectedItem is DataItem))
            //    return;

            var dataItem = associatedObject.DataContext as FormulaItemModel;
            if (dataItem == null)
            {
                return;
            }

            DragDropAdorner adorner = new DragDropAdorner(associatedObject);
            mAdornerLayer = AdornerLayer.GetAdornerLayer(Utility.FindVisualParent<Grid>(associatedObject)); // Window class do not have AdornerLayer
            //mAdornerLayer.Add(adorner);

            if (!dataItem.IsActive)
            {
                dataItem = (FormulaItemModel) dataItem.Clone();
            }
            //DataItem dataItem = listBoxItem.Content as DataItem;
            //DataObject dataObject = new DataObject(dataItem.Clone());
            //// Here, we should notice that dragsource param will specify on which 
            //// control the drag&drop event will be fired
            System.Windows.DragDrop.DoDragDrop(panel, dataItem, DragDropEffects.Copy);

            //mStartHoverTime = DateTime.MinValue;
            //mHoveredItem = null;
            //mAdornerLayer.Remove(adorner);
            //mAdornerLayer = null;
        }

        protected override void OnDetaching()
        {
            
        }        
    }
}
