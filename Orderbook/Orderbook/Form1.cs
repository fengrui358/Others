using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Orderbook
{
    public partial class Form1 : Form
    {
        //控制暂停启用的信号量
        private readonly ManualResetEventSlim _updateEventSlim;

        enum OrderSide
        {
            Buy = 1,
            Sell = -1
        }

        enum OrderStatus
        {
            Working = 1,
            Filled = 2,
            Canceled = 3
        }

        private class OrderUpdate
        {
            public int OrderID { get; set; }
            public OrderSide Side { get; set; }
            public double Price { get; set; }
            public int Quantity { get; set; }
            public OrderStatus Status { get; set; }
        }

        public Form1()
        {
            InitializeComponent();

            //本例中只有一个额外线程操作界面，可安全禁用CheckForIllegalCrossThreadCalls
            CheckForIllegalCrossThreadCalls = false; 
            _updateEventSlim = new ManualResetEventSlim(true);

            _incomingOrderThread = new Thread(InComingWorkerProcessor);
            _incomingOrderThread.IsBackground = true;
            _incomingOrderThread.Start();
        }

        private Thread _incomingOrderThread;

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
                int randNum = rand.Next()%3000;
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "暂停")
            {
                //暂停 grid update
                _updateEventSlim.Reset();
                button1.Text = "继续";
            }
            else if(button1.Text == "继续")
            {
                //继续 grid update
                _updateEventSlim.Set();
                button1.Text = "暂停";
            }
        }

        private void OnOrderUpdate(OrderUpdate update)
        {
            if (!TryUpdateDataGridViewRow(update))
            {
                var row = GetDataGridViewRow(update);

                _dgvOrderbook.Rows.Add(row);
            }
        }

        private bool TryUpdateDataGridViewRow(OrderUpdate update)
        {
            for (int i = 0; i < _dgvOrderbook.Rows.Count; i++)
            {
                var orderId = _dgvOrderbook.Rows[i].Cells[0].Value;
                if (orderId != null)
                {
                    if (update.OrderID == (int) orderId)
                    {
                        //更新行背景色
                        _dgvOrderbook.Rows[i].DefaultCellStyle = new DataGridViewCellStyle
                        {
                            BackColor = GetStatusBackColor(update.Status)
                        };

                        //发现相同数据，直接更新
                        _dgvOrderbook.Rows[i].Cells["Side"].Value = update.Side;
                        _dgvOrderbook.Rows[i].Cells["Side"].Style = new DataGridViewCellStyle
                        {
                            BackColor = GetOrderSideBackColor(update.Side)
                        };

                        _dgvOrderbook.Rows[i].Cells["Price"].Value = update.Price;
                        _dgvOrderbook.Rows[i].Cells["Quantity"].Value = update.Quantity;
                        _dgvOrderbook.Rows[i].Cells["Status"].Value = update.Status;

                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 将实体转换为DataGridViewRow
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        private DataGridViewRow GetDataGridViewRow(OrderUpdate update)
        {
            var row = new DataGridViewRow();

            //根据OrderStatus设置行背景色
            row.DefaultCellStyle = new DataGridViewCellStyle {BackColor = GetStatusBackColor(update.Status)};

            var orderIDCell = new DataGridViewTextBoxCell {Value = update.OrderID};
            row.Cells.Add(orderIDCell);
            orderIDCell.ReadOnly = true;

            var sideCell = new DataGridViewTextBoxCell
            {                
                Value = update.Side,
                Style = new DataGridViewCellStyle {BackColor = GetOrderSideBackColor(update.Side) } //根据Side设置单元格颜色
            };
            row.Cells.Add(sideCell);
            orderIDCell.ReadOnly = true;

            var priceCell = new DataGridViewTextBoxCell { Value = update.Price };
            row.Cells.Add(priceCell);
            orderIDCell.ReadOnly = true;

            var quantityCell = new DataGridViewTextBoxCell { Value = update.Quantity };
            row.Cells.Add(quantityCell);
            orderIDCell.ReadOnly = true;

            var statusCell = new DataGridViewTextBoxCell { Value = update.Status };
            row.Cells.Add(statusCell);
            orderIDCell.ReadOnly = true;

            return row;
        }

        private Color GetStatusBackColor(OrderStatus status)
        {
            Color result = Color.Transparent;

            //根据Status设置行的颜色
            switch (status)
            {
                case OrderStatus.Working:
                    result = Color.White;
                    break;
                case OrderStatus.Filled:
                    result = Color.LightGray;
                    break;
                case OrderStatus.Canceled:
                    result = Color.Gray;
                    break;
            }

            return result;
        }

        private Color GetOrderSideBackColor(OrderSide orderSide)
        {
            return orderSide == OrderSide.Buy ? Color.Red : Color.Green;
        }
    }
}
