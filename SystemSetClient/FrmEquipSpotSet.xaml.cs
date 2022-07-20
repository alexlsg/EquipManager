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

namespace SystemSetClient
{
    /// <summary>
    /// UCEquipSpotSet.xaml 的交互逻辑
    /// </summary>
    public partial class FrmEquipSpotSet : Window
    {
        ObservableCollection<EquipSpotSet> EquipSpotSets { get; set; }
        EquipSpotSetService EquipSpotSetService { get; set; }

        Equip CurrentEquip { get; set; }
        public FrmEquipSpotSet(Equip CurrentEquip)
        {
            InitializeComponent();
            this.CurrentEquip = CurrentEquip;
            Loaded += UCEquipSpotSet_Loaded;
        }

        private void UCEquipSpotSet_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= UCEquipSpotSet_Loaded;
            EquipSpotSets = new ObservableCollection<EquipSpotSet>();
            EquipSpotSetService = new EquipSpotSetService();
            GetData();
        }
        /// <summary>
        /// 查询
        /// </summary>
        public void GetData()
        {
            EquipSpotSets.Clear();
            datagrid.ItemsSource = EquipSpotSets;
            HttpResult httpResult = EquipSpotSetService.GetEquipSpotSetListByEquipId(CurrentEquip.Id);
            if (httpResult.Status)
            {
                List<EquipSpotSet> equipSpotSets = httpResult.Data as List<EquipSpotSet>;
                for (int i = 0; i < equipSpotSets.Count; i++)
                {
                    EquipSpotSets.Add(equipSpotSets[i]);
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
            bool repea1 = JudgeSpotNORepeat();
            bool repeat2 = JudgeSpotNmRepeat();
            bool empty = JudgeEmpty();
            if (repea1 && repeat2 && !empty)
            {
                EquipSpotSets.ToList().FindAll(n => n.DataStatus != DataStatus.NONE).ForEach(n =>
                {
                    n.EquipId = CurrentEquip.Id;
                    switch (n.DataStatus)
                    {
                        case DataStatus.ADD:
                            httpResult = EquipSpotSetService.AddEquipSpotSet(n);
                            break;
                        case DataStatus.MOD:
                            httpResult = EquipSpotSetService.ModEquipSpotSet(n);
                            break;
                        case DataStatus.DEL:
                            httpResult = EquipSpotSetService.DelEquipSpotSet(n);
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
            bool a = EquipSpotSets.ToList().Exists(n => string.IsNullOrWhiteSpace(n.SpotNO) || string.IsNullOrWhiteSpace(n.DataType) || string.IsNullOrWhiteSpace(n.SpotNm));
            if (a)
            {
                MessageBox.Show("设备测点信息存在空值！请检查！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            return a;
        }
        /// <summary>
        /// 判断重复
        /// </summary>
        /// <returns></returns>
        private bool JudgeSpotNORepeat()
        {
            int a = EquipSpotSets.ToList().Count(n => n.DataStatus != DataStatus.DEL);
            int b = EquipSpotSets.ToList().FindAll(n => n.DataStatus != DataStatus.DEL).Select(n => n.SpotNO).Distinct().Count();
            bool c = a == b;
            if (!c)
            {
                MessageBox.Show("测点编号不能重复！请检查！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            return c;
        }
        /// <summary>
        /// 判断重复
        /// </summary>
        /// <returns></returns>
        private bool JudgeSpotNmRepeat()
        {
            int a = EquipSpotSets.ToList().Count(n => n.DataStatus != DataStatus.DEL);
            int b = EquipSpotSets.ToList().FindAll(n => n.DataStatus != DataStatus.DEL).Select(n => n.SpotNm).Distinct().Count();
            bool c = a == b;
            if (!c)
            {
                MessageBox.Show("测点名称不能重复！请检查！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
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
            if (datagrid.SelectedItem != null && datagrid.SelectedItem is EquipSpotSet)
            {
                EquipSpotSet EquipSpotSet = (EquipSpotSet)datagrid.SelectedItem;
                EquipSpotSet.DataStatus = DataStatus.DEL;
                datagrid.Items.Filter = (a) =>
                {
                    EquipSpotSet _EquipSpotSet = (EquipSpotSet)a;
                    return _EquipSpotSet.DataStatus != DataStatus.DEL;
                };
            }
        }
    }
}
