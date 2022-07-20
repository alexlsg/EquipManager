using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
using SystemSet;
using Tools;

namespace SystemSetClient.Pages.ComSet
{
    /// <summary>
    /// UCComSet.xaml 的交互逻辑
    /// </summary>
    public partial class UCComSet : UserControl
    {
        public UCComSet()
        {
            InitializeComponent();
            Loaded += UCGateway_Loaded;
        }
        ObservableCollection<Gateway> Gateways { get; set; }
        GatewayService gatewayService { get; set; }
        private void UCGateway_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= UCGateway_Loaded;
            Gateways = new ObservableCollection<Gateway>();
            gatewayService = new GatewayService();
            GetData();
        }
        /// <summary>
        /// 查询
        /// </summary>
        public void GetData()
        {
            Gateways.Clear();
            datagrid.ItemsSource = Gateways;
            HttpResult httpResult = gatewayService.GetGatewayList();
            if (httpResult.Status)
            {
                List<Gateway> gateways = httpResult.Data as List<Gateway>;
                for (int i = 0; i < gateways.Count; i++)
                {
                    Gateways.Add(gateways[i]);
                }
            }
            else
            {
                MessageBox.Show(httpResult.Message, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            HttpResult httpResult = null;
            bool repeat = JudgeRepeat();
            bool empty = JudgeEmpty();
            if (repeat && !empty)
            {
                Gateways.ToList().FindAll(n => n.DataStatus != DataStatus.NONE).ForEach(n =>
                {
                    switch (n.DataStatus)
                    {
                        case DataStatus.ADD:
                            httpResult = gatewayService.AddGateway(n);
                            break;
                        case DataStatus.MOD:
                            httpResult = gatewayService.ModGateway(n);
                            break;
                        case DataStatus.DEL:
                            httpResult = gatewayService.DelGateway(n);
                            break;
                        default:
                            break;
                    }
                    if (!httpResult.Status)
                    {
                        MessageBox.Show(httpResult.Message, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                });
                if (httpResult != null && httpResult.Status)
                {
                    MessageBox.Show("保存成功!");
                    GetData();
                }
            }
        }
        /// <summary>
        /// 判断空值
        /// </summary>
        /// <returns></returns>
        private bool JudgeEmpty()
        {
            bool a = Gateways.ToList().Exists(n => string.IsNullOrWhiteSpace(n.Name) || string.IsNullOrWhiteSpace(n.IP) || string.IsNullOrWhiteSpace(n.PORT));
            if (a)
            {
                MessageBox.Show("网关信息存在空值！请检查！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            return a;
        }
        /// <summary>
        /// 判断重复
        /// </summary>
        /// <returns></returns>
        private bool JudgeRepeat()
        {
            int a = Gateways.ToList().Count(n => n.DataStatus != DataStatus.DEL);
            int b = Gateways.ToList().FindAll(n => n.DataStatus != DataStatus.DEL).Select(n => n.Name).Distinct().Count();
            bool c = a == b;
            if (!c)
            {
                MessageBox.Show("网关名称不能重复！请检查！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            return c;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (datagrid.SelectedItem != null && datagrid.SelectedItem is Gateway)
            {
                Gateway gateway = (Gateway)datagrid.SelectedItem;
                gateway.DataStatus = DataStatus.DEL;
                datagrid.Items.Filter = (a) =>
                {
                    Gateway _gateway = (Gateway)a;
                    return _gateway.DataStatus != DataStatus.DEL;
                };
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Gateways.Add(new Gateway());
        }
    }
}
