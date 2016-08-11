using System.ComponentModel;
using GalaSoft.MvvmLight;

namespace OrderbookWPF.Models
{
    public enum OrderSide
    {
        Buy = 1,
        Sell = -1
    }

    public enum OrderStatus
    {
        Working = 1,
        Filled = 2,
        Canceled = 3
    }

    public class OrderUpdate : ObservableObject
    {
        private int _orderId;
        private OrderSide _side;
        private double _price;
        private int _quantity;
        private OrderStatus _status;

        public int OrderID
        {
            get { return _orderId; }
            set { _orderId = value; }
        }

        public OrderSide Side
        {
            get { return _side; }
            set
            {
                _side = value;
                RaisePropertyChanged(() => Side);
            }
        }

        public double Price
        {
            get { return _price; }
            set
            {
                _price = value;
                RaisePropertyChanged(() => Price);
            }
        }

        public int Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                RaisePropertyChanged(() => Quantity);
            }
        }

        public OrderStatus Status
        {
            get { return _status; }
            set
            {
                _status = value;
                RaisePropertyChanged(() => Status);
            }
        }
    }
}
