using System;
using System.Collections.Generic;
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
    /// UCEquipGroup.xaml 的交互逻辑
    /// </summary>
    public partial class UCEquipGroup : UserControl
    {
        List<EquipGroup> EquipGroups { get; set; }
        EquipGroup CurrentEquipGroup { get; set; }
        Gateway CurrentGateway { get; set; }
        public UCEquipGroup()
        {
            InitializeComponent();
            Loaded += UCEquipGroup_Loaded;
        }

        private void UCEquipGroup_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= UCEquipGroup_Loaded;
            GetData();
        }
        private void GetData()
        {
            EquipGroupService EquipGroupService = new EquipGroupService();
            HttpResult httpEquipGroupResult = EquipGroupService.GetEquipGroupList();
            EquipGroups = (List<EquipGroup>)httpEquipGroupResult.Data;
            GatewayService gatewayService = new GatewayService();
            HttpResult httpGatewayResult = gatewayService.GetGatewayList();
            List<Gateway> gatewayList = (List<Gateway>)httpGatewayResult.Data;
            EquipService EquipService = new EquipService();
            HttpResult httpEquipResult = EquipService.GetEquipList();
            List<Equip> EquipList = (List<Equip>)httpEquipResult.Data;
            gatewayList.ForEach(gateway => gateway.Equips = EquipList.FindAll(n => n.GatewayId == gateway.Id));
            treeview.ItemsSource = EquipGroups;
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            FrmEquipGroup frmEquipGroup = new FrmEquipGroup();
            frmEquipGroup.ShowDialog();
            GetData();
        }

        private void treeview_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (treeview.SelectedItem is EquipGroup)
            {
                CurrentEquipGroup = (EquipGroup)treeview.SelectedItem;
            }
            else if (treeview.SelectedItem is Gateway)
            {
                CurrentGateway = (Gateway)treeview.SelectedItem;
            }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            if (menuItem.Tag != null)
            {
                if (menuItem.Tag.ToString() == "1" && CurrentEquipGroup != null)
                {
                    FrmGateway frmGateway = new FrmGateway(CurrentEquipGroup);
                    frmGateway.ShowDialog();
                }
                else if (menuItem.Tag.ToString() == "2" && CurrentGateway != null)
                {
                    FrmEquip frmEquip = new FrmEquip(CurrentGateway);
                    frmEquip.ShowDialog();
                }
            }
            GetData();
        }
    }
}
