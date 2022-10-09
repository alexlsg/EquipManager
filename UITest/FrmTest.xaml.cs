using EquipDataManager.Bll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace UITest
{
    /// <summary>
    /// FrmTest.xaml 的交互逻辑
    /// </summary>
    public partial class FrmTest : Window
    {
        public FrmTest()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            running = true;
            rtb.Dispatcher.Invoke(new Action(() =>
            {
                rtb.AppendText("\r\n");
                rtb.AppendText("开始");
            }));
            _time = DateTime.Now;
            _time1 = DateTime.Now;
            Thread _t = new Thread(Cx);
            _t.Start();
            Thread _t1 = new Thread(Gx);
            _t1.Start();
        }
        DateTime _time = DateTime.Now;
        DateTime _time1 = DateTime.Now;
        void Jlrz(string msg)
        {
            if ((DateTime.Now - _time).TotalSeconds > 5)
            {
                _time = DateTime.Now;
                rtb.Dispatcher.Invoke(new Action(() =>
                {
                    rtb.AppendText("\r\n");
                    rtb.AppendText(msg);
                }));
            }
        }
        void Jlrz1(string msg)
        {
            if ((DateTime.Now - _time1).TotalSeconds > 5)
            {
                _time1 = DateTime.Now;
                rtb.Dispatcher.Invoke(new Action(() =>
                {
                    rtb.AppendText("\r\n");
                    rtb.AppendText(msg);
                }));
            }
        }
        List<double> List = new List<double>();
        Random random = new Random();
        private void Gx(object obj)
        {
            while (running)
            {
                try
                {
                    lock (lockobj)
                    {
                        if (List.Count > 0)
                        {
                            List.RemoveAt(0);
                        }
                        for (int i = 0; i < 3; i++)
                        {
                            List.Add(random.NextDouble());
                        }
                    }
                    Jlrz("更新");
                }
                catch (Exception ex)
                {
                    rtb.Dispatcher.Invoke(new Action(() =>
                    {
                        rtb.AppendText("\r\n");
                        rtb.AppendText(ex.Message);
                    }));

                }
            }
        }
        readonly object lockobj = new object();
        bool running = true;
        void Cx()
        {
            while (running)
            {
                try
                {
                    lock (lockobj)
                    {
                        var _temp = from a in List
                                    where a < 0.5
                                    select a;
                        List<double> _a = _temp.ToList();
                        Thread.Sleep(5000);
                    }

                    Jlrz1("查询");
                }
                catch (Exception ex)
                {
                    rtb.Dispatcher.Invoke(new Action(() =>
                    {
                        rtb.AppendText("\r\n");
                        rtb.AppendText(ex.Message);
                    }));

                }
            }
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            running = false; rtb.Dispatcher.Invoke(new Action(() =>
            {
                rtb.AppendText("\r\n");
                rtb.AppendText("结束");
            }));
        }
    }
}
