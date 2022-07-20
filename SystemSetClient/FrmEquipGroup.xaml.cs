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
    /// FrmEquipGroup.xaml 的交互逻辑
    /// </summary>
    public partial class FrmEquipGroup : Window
    {
        ObservableCollection<EquipGroup> EquipGroups { get; set; }
        EquipGroupService EquipGroupService { get; set; }
        public FrmEquipGroup()
        {
            InitializeComponent();
            Loaded += UCEquipGroup_Loaded;
        }

        private void UCEquipGroup_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= UCEquipGroup_Loaded;
            EquipGroups = new ObservableCollection<EquipGroup>();
            EquipGroupService = new EquipGroupService();
            GetData();
        }
        /// <summary>
        /// 查询
        /// </summary>
        public void GetData()
        {
            EquipGroups.Clear();
            datagrid.ItemsSource = EquipGroups;
            HttpResult httpResult = EquipGroupService.GetEquipGroupList();
            if (httpResult.Status)
            {
                List<EquipGroup> _EquipGroups = httpResult.Data as List<EquipGroup>;
                for (int i = 0; i < _EquipGroups.Count; i++)
                {
                    EquipGroups.Add(_EquipGroups[i]);
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
                EquipGroups.ToList().FindAll(n => n.DataStatus != DataStatus.NONE).ForEach(n =>
                {
                    switch (n.DataStatus)
                    {
                        case DataStatus.ADD:
                            httpResult = EquipGroupService.AddEquipGroup(n);
                            break;
                        case DataStatus.MOD:
                            httpResult = EquipGroupService.ModEquipGroup(n);
                            break;
                        case DataStatus.DEL:
                            httpResult = EquipGroupService.DelEquipGroup(n);
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
            bool a = EquipGroups.ToList().Exists(n => string.IsNullOrWhiteSpace(n.EquipGroupName));
            if (a)
            {
                MessageBox.Show("设备组信息存在空值！请检查！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            return a;
        }
        /// <summary>
        /// 判断重复
        /// </summary>
        /// <returns></returns>
        private bool JudgeRepeat()
        {
            int a = EquipGroups.ToList().Count(n => n.DataStatus != DataStatus.DEL);
            int b = EquipGroups.ToList().FindAll(n => n.DataStatus != DataStatus.DEL).Select(n => n.EquipGroupName).Distinct().Count();
            bool c = a == b;
            if (!c)
            {
                MessageBox.Show("设备组名称不能重复！请检查！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
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
            if (datagrid.SelectedItem != null && datagrid.SelectedItem is EquipGroup)
            {
                EquipGroup EquipGroup = (EquipGroup)datagrid.SelectedItem;
                EquipGroup.DataStatus = DataStatus.DEL;
                datagrid.Items.Filter = (a) =>
                {
                    EquipGroup _EquipGroup = (EquipGroup)a;
                    return _EquipGroup.DataStatus != DataStatus.DEL;
                };
            }
        }
    }
}
