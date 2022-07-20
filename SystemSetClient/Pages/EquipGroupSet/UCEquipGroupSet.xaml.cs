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

namespace SystemSetClient.Pages.EquipGroupSet
{
    /// <summary>
    /// UCEquipGroupSet.xaml 的交互逻辑
    /// </summary>
    public partial class UCEquipGroupSet : UserControl
    {
        List<Equip> allequips = new List<Equip>();
        ObservableCollection<Equip> equips = new ObservableCollection<Equip>();
        EquipService equipservice = new EquipService();

        ObservableCollection<EquipGroup> EquipGroups { get; set; }
        EquipGroup CurrentEquipGroup { get; set; }
        EquipGroupService EquipGroupService = new EquipGroupService();
        public UCEquipGroupSet()
        {
            InitializeComponent();
            Loaded += UCEquipGroupSet_Loaded;
        }

        private void UCEquipGroupSet_Loaded(object sender, RoutedEventArgs e)
        {
            GetData();
        }

        private void GetData()
        {
            allequips = equipservice.GetEquipList().Data as List<Equip>;
            equips.Clear();
            allequips.FindAll(n => string.IsNullOrEmpty(n.ProductionLineGroupBinding)).ForEach(n => equips.Add(n));
            dg_sb.ItemsSource = equips;
            EquipGroups = new ObservableCollection<EquipGroup>(EquipGroupService.GetEquipGroupList().Data as List<EquipGroup>);
            foreach (var item in EquipGroups)
            {
                item.Equips = new ObservableCollection<Equip>(allequips.FindAll(n => n.ProductionLineGroupBinding == item.EquipGroupId.ToString()));
            }
            treeview.ItemsSource = EquipGroups;
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Equip _sb = dg_sb.SelectedItem as Equip;
            if (CurrentEquipGroup != null)
            {
                if (CurrentEquipGroup.EquipGroupId == 0)
                {
                    MessageBox.Show("新增的产线组,请保存后再绑定设备.");
                }
                else
                {
                    _sb.ProductionLineGroupBinding = CurrentEquipGroup.EquipGroupId.ToString();
                    CurrentEquipGroup.Equips.Add(_sb);
                    equips.Remove(_sb);
                }
            }
            else
                MessageBox.Show("请先选择产线");
        }

        private void MenuItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem _item = sender as TreeViewItem;
            if ((_item.DataContext) is Equip)
            {
                CurrentEquipGroup = EquipGroups.ToList().Find(n => n.EquipGroupId.ToString() == (_item.DataContext as Equip).ProductionLineGroupBinding);
                Equip _sb = _item.DataContext as Equip;
                _sb.ProductionLineGroupBinding = "";
                CurrentEquipGroup.Equips.Remove(_sb);
                equips.Add(_sb);
            }
        }

        private void MenuItem_Click_1(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem _item = sender as TreeViewItem;
            if ((_item.DataContext) is EquipGroup)
            {
                CurrentEquipGroup = _item.DataContext as EquipGroup;
            }
            else
            {
                try
                {
                    CurrentEquipGroup = EquipGroups.ToList().Find(n => n.EquipGroupId.ToString() == (_item.DataContext as Equip).ProductionLineGroupBinding);
                }
                catch (Exception ex)
                {
                }
            }
            tb.DataContext = CurrentEquipGroup;
            gb1.DataContext = CurrentEquipGroup;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string _name = tname.Text.Trim();
            if (string.IsNullOrEmpty(_name))
            {
                MessageBox.Show("请输入产线名称!");
                return;
            }
            EquipGroups.Add(new EquipGroup() { EquipGroupName = _name });
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (CurrentEquipGroup != null)
            {
                if (CurrentEquipGroup.DataStatus == Tools.DataStatus.ADD)
                {
                    EquipGroups.Remove(CurrentEquipGroup);
                    CurrentEquipGroup = null;
                    gb1.Header = "未分组设备";
                }
                else
                {
                    foreach (var item in CurrentEquipGroup.Equips)
                    {
                        equips.Add(item);
                        item.ProductionLineGroupBinding = "";
                    }
                    CurrentEquipGroup.Equips.Clear();
                    CurrentEquipGroup.DataStatus = Tools.DataStatus.DEL;
                    treeview.Items.Filter = (n) =>
                    {
                        return (n as EquipGroup).DataStatus != Tools.DataStatus.DEL;
                    };
                }
            }
            else
            {
                MessageBox.Show("请先选择产线!");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            SaveSB();
            SaveGroup();
            GetData();
            MessageBox.Show("保存成功");
        }

        private void SaveGroup()
        {
            HttpResult httpResult = null;
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
        }

        private void SaveSB()
        {
            HttpResult httpResult = null;
            allequips.ToList().FindAll(n => n.DataStatus != DataStatus.NONE).ForEach(n =>
            {
                switch (n.DataStatus)
                {
                    case DataStatus.MOD:
                        httpResult = equipservice.ModEquip(n);
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
        }
    }
}
