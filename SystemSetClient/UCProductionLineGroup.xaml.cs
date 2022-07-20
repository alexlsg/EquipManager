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
    /// UCProductionLineGroup.xaml 的交互逻辑
    /// </summary>
    public partial class UCProductionLineGroup : UserControl
    {
        List<ProductionLine> ProductionLines { get; set; }
        ProductionLine CurrentProductionLine { get; set; }
        Equip CurrentEquip { get; set; }
        public UCProductionLineGroup()
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
            ProductionLineService productionLineService = new ProductionLineService();
            HttpResult httpProductionLineResult = productionLineService.GetProductionLineList();
            ProductionLines = (List<ProductionLine>)httpProductionLineResult.Data;

            ProductionLineGroupService productionLineGroupService = new ProductionLineGroupService();
            HttpResult httpProductionLineGroupResult = productionLineGroupService.GetProductionLineGroupList();
            List<ProductionLineGroup> ProductionLineGroups = (List<ProductionLineGroup>)httpProductionLineGroupResult.Data;

            EquipTypeService EquipTypeService = new EquipTypeService();
            HttpResult httpEquipTypeResult = EquipTypeService.GetEquipTypeList();
            List<EquipType> EquipTypes = (List<EquipType>)httpEquipTypeResult.Data;

            EquipService EquipService = new EquipService();
            HttpResult httpEquipResult = EquipService.GetEquipList();
            List<Equip> EquipList = (List<Equip>)httpEquipResult.Data;

            EquipSpotSetService equipSpotSetService = new EquipSpotSetService();
            HttpResult httpEquipSpotSetResult = equipSpotSetService.GetEquipSpotSetList();
            List<EquipSpotSet> EquipSpotSets = (List<EquipSpotSet>)httpEquipSpotSetResult.Data;

            EquipList.ForEach(a => a.EquipSpotSets = EquipSpotSets.FindAll(b => b.EquipId == a.Id));


            for (int i = 0; i < ProductionLines.Count; i++)
            {
                List<ProductionLineGroup> productionLineGroups = ProductionLineGroups.FindAll(a => a.ProductionLineId == ProductionLines[i].ProductionLineId);
                List<Equip> equips = EquipList.FindAll(a => productionLineGroups.Exists(b => b.EquipNO == a.EquipNO));
                List<EquipType> equipTypes = EquipTypes.FindAll(a => equips.Exists(b => b.TypeBinding == a.EquipTypeId));
                equipTypes.ForEach(a => a.Equips = equips.FindAll(n => n.TypeBinding == a.EquipTypeId));
                ProductionLines[i].EquipTypes = equipTypes;
                ProductionLines[i].Equips = equips;
            }

            treeview.ItemsSource = ProductionLines;
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            FrmProductionLine frmProductionLine = new FrmProductionLine();
            frmProductionLine.ShowDialog();
            GetData();
        }

        private void treeview_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (treeview.SelectedItem is ProductionLine)
            {
                CurrentProductionLine = (ProductionLine)treeview.SelectedItem;
            }
            else if (treeview.SelectedItem is Equip)
            {
                CurrentEquip = (Equip)treeview.SelectedItem;
            }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            if (menuItem.Tag != null)
            {
                if (menuItem.Tag.ToString() == "1" && CurrentProductionLine != null)
                {
                    ChooseEquip chooseEquip = new ChooseEquip(CurrentProductionLine);
                    chooseEquip.ShowDialog();
                }
                else if (menuItem.Tag.ToString() == "2" && CurrentEquip != null)
                {
                    FrmEquipSpotSet frmEquipSpotSet = new FrmEquipSpotSet(CurrentEquip);
                    frmEquipSpotSet.ShowDialog();
                }
            }
            GetData();
        }
    }
}
