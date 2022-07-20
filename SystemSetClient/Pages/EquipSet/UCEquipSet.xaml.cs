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

namespace SystemSetClient.Pages.EquipSet
{
    /// <summary>
    /// UCEquipSet.xaml 的交互逻辑
    /// </summary>
    public partial class UCEquipSet : UserControl
    {
        public UCEquipSet()
        {
            InitializeComponent();
            Loaded += UCEquip_Loaded;
        }
        ObservableCollection<Equip> Equips { get; set; }
        EquipService EquipService { get; set; }

        List<Gateway> Gateways { get; set; }
        GatewayService gatewayService { get; set; }
        private void UCEquip_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= UCEquip_Loaded;
            Equips = new ObservableCollection<Equip>();
            EquipService = new EquipService();
            datagrid.ItemsSource = Equips;
            gatewayService = new GatewayService();
            HttpResult _hr = gatewayService.GetGatewayList();
            Gateways = _hr.Data as List<Gateway>;
            t_com.DataContext = Gateways;
            GetData();
        }
        /// <summary>
        /// 查询
        /// </summary>
        public void GetData()
        {
            Equips.Clear();
            HttpResult httpResult = EquipService.GetEquipList();
            if (httpResult.Status)
            {
                List<Equip> equips = httpResult.Data as List<Equip>;
                for (int i = 0; i < equips.Count; i++)
                {
                    Equips.Add(equips[i]);
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
            bool repeat2 = JudgeEquipNameRepeat();
            bool empty = JudgeEmpty();
            if (repeat2 && !empty)
            {
                Equips.ToList().FindAll(n => n.DataStatus != DataStatus.NONE).ForEach(n =>
                {
                    switch (n.DataStatus)
                    {
                        case DataStatus.ADD:
                            httpResult = EquipService.AddEquip(n);
                            break;
                        case DataStatus.MOD:
                            httpResult = EquipService.ModEquip(n);
                            break;
                        case DataStatus.DEL:
                            httpResult = EquipService.DelEquip(n);
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
            bool a = Equips.ToList().Exists(n =>
                string.IsNullOrWhiteSpace(n.EquipName) ||
                string.IsNullOrWhiteSpace(n.EquipNO) ||
                n.TypeBinding == 0
            //|| string.IsNullOrWhiteSpace(n.ProductionLineGroupBinding)
            );
            if (a)
            {
                MessageBox.Show("设备信息存在空值！请检查！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            return a;
        }
        /// <summary>
        /// 判断重复
        /// </summary>
        /// <returns></returns>
        private bool JudgeEquipNameRepeat()
        {
            int a = Equips.ToList().Count(n => n.DataStatus != DataStatus.DEL);
            int b = Equips.ToList().FindAll(n => n.DataStatus != DataStatus.DEL).Select(n => n.EquipName).Distinct().Count();
            bool c = a == b;
            if (!c)
            {
                MessageBox.Show("设备名称不能重复！请检查！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
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
            if (datagrid.SelectedItem != null && datagrid.SelectedItem is Equip)
            {
                Equip Equip = (Equip)datagrid.SelectedItem;
                Equip.DataStatus = DataStatus.DEL;
                datagrid.Items.Filter = (a) =>
                {
                    Equip _Equip = (Equip)a;
                    return _Equip.DataStatus != DataStatus.DEL;
                };
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Equips.Add(new Equip());
        }
    }
}
