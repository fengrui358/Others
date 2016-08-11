using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using OrderbookWPF.Models;

namespace OrderbookWPF.UIConverters
{
    public class OrderStatusConvertToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var orderStatus = (OrderStatus) value;
            var color = Colors.Transparent;

            //根据Status设置行的颜色
            switch (orderStatus)
            {
                case OrderStatus.Working:
                    color = Colors.White;
                    break;
                case OrderStatus.Filled:
                    color = Colors.LightGray;
                    break;
                case OrderStatus.Canceled:
                    color = Colors.Gray;
                    break;
            }

            return new SolidColorBrush(color);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SideConvertToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = ((OrderSide) value) == OrderSide.Buy ? Colors.Red : Colors.Green;
            return new SolidColorBrush(color);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StartConvertToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool) value)
            {
                return "暂停";
            }
            else
            {
                return "继续";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
