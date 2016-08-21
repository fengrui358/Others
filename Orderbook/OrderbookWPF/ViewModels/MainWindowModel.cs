using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using Nelibur.ObjectMapper;
using OrderbookWPF.Models;

namespace OrderbookWPF.ViewModels
{
    public class MainWindowModel : ViewModelBase
    {
        private bool _start = true;
        private readonly Thread _incomingOrderThread;

        //控制暂停启用的信号量
        private readonly ManualResetEventSlim _updateEventSlim;

        public bool Start
        {
            get { return _start; }
            set
            {
                _start = value;
                if (_start)
                {
                    _updateEventSlim.Set();
                }
                else
                {
                    _updateEventSlim.Reset();
                }

                RaisePropertyChanged(() => Start);
            }
        }

        public ObservableCollection<OrderUpdate> OrderUpdates { get; set; }

        public RelayCommand SwitchStartStatusCommand { get; private set; }

        public MainWindowModel()
        {
            OrderUpdates = new ObservableCollection<OrderUpdate>();

            _updateEventSlim = new ManualResetEventSlim(true);

            SwitchStartStatusCommand = new RelayCommand(() =>
            {
                Start = !Start;
            });

            _incomingOrderThread = new Thread(InComingWorkerProcessor);
            _incomingOrderThread.IsBackground = true;
            _incomingOrderThread.Start();
        }

        private void InComingWorkerProcessor()
        {
            var filestream = new System.IO.FileStream("OrderUpdates.txt",
                                          System.IO.FileMode.Open,
                                          System.IO.FileAccess.Read,
                                          System.IO.FileShare.ReadWrite);
            var file = new System.IO.StreamReader(filestream);
            String line;

            Random rand = new Random();
            while ((line = file.ReadLine()) != null)
            {                
                int randNum = rand.Next() % 3000;
                Thread.Sleep(randNum);
                var splits = line.Split(',');
                OrderUpdate update = new OrderUpdate();
                update.OrderID = Convert.ToInt32(splits[0]);
                update.Side = (OrderSide)Enum.Parse(typeof(OrderSide), splits[1]);
                update.Price = Convert.ToDouble(splits[2]);
                update.Quantity = Convert.ToInt32(splits[3]);
                update.Status = (OrderStatus)Enum.Parse(typeof(OrderStatus), splits[4]);

                _updateEventSlim.Wait();
                OnOrderUpdate(update);
            }
        }

        private void OnOrderUpdate(OrderUpdate update)
        {
            var exist = OrderUpdates.FirstOrDefault(s => s.OrderID == update.OrderID);
            if (exist != null)
            {
                TinyMapper.Map(update, exist);
            }
            else
            {
                DispatcherHelper.UIDispatcher.Invoke(
                    new Action(() => OrderUpdates.Add(update)));
            }
        }
    }
}
