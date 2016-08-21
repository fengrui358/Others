using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using FormulaDemo.Models;
using FormulaDemo.Models.Numbsers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace FormulaDemo.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// 全部公式项
        /// </summary>
        public List<FormulaItemModel> AllFormulaItems { get; set; }

        /// <summary>
        /// 拖拽开始命令
        /// </summary>
        public RelayCommand<object> PreviewDragEnterCommand { get; private set; }

        public RelayCommand<DragEventArgs> EditContainerDragEnterCommand { get; private set; }

        public MainWindowViewModel()
        {
            PreviewDragEnterCommand = new RelayCommand<object>(PreviewDragEnterHandler);
            EditContainerDragEnterCommand = new RelayCommand<DragEventArgs>(EditContainerDragEnterHnadler);

            InitFormulaItems();
        }        

        private void InitFormulaItems()
        {
            AllFormulaItems = new List<FormulaItemModel>
            {
                new NumOneFormulaItemModel(),
                new NumOneFormulaItemModel(),
                new NumOneFormulaItemModel(),
                new NumOneFormulaItemModel()
            };
        }

        private void PreviewDragEnterHandler(object obj)
        {
            var x = obj;
        }

        private void EditContainerDragEnterHnadler(DragEventArgs obj)
        {
            
        }
    }
}
