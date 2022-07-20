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
using System.Windows.Shapes;
using SystemSet;
using Tools;

namespace SystemSetClient
{
    /// <summary>
    /// ChooseEquip.xaml 的交互逻辑
    /// </summary>
    public partial class ChooseEquip : Window
    {
        EquipService EquipService { get; set; }
        ProductionLine CurrentProductionLine { get; set; }
        List<Equip> Equips { get; set; }
        public ChooseEquip(ProductionLine CurrentProductionLine)
        {
            InitializeComponent();
            this.CurrentProductionLine = CurrentProductionLine;
            Loaded += ChooseEquip_Loaded;
        }

        private void ChooseEquip_Loaded(object sender, RoutedEventArgs e)
        {
            EquipService = new EquipService();
            HttpResult httpResult = EquipService.GetEquipList();
            Equips = (List<Equip>)httpResult.Data;
            Equips.ForEach(e => e.Selected = CurrentProductionLine.Equips.Exists(b => b.EquipNO == e.EquipNO));
            datagrid.ItemsSource = Equips;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            HttpResult httpResult = null;
            ProductionLineGroupService productionLineGroupService = new ProductionLineGroupService();
            productionLineGroupService.DelProductionLineGroup(CurrentProductionLine.ProductionLineId);
            Equips.FindAll(n => n.Selected).ForEach(n =>
            {
                ProductionLineGroup ProductionLineGroup = new ProductionLineGroup() { ProductionLineId = CurrentProductionLine.ProductionLineId, EquipNO = n.EquipNO };
                httpResult = productionLineGroupService.AddProductionLineGroup(ProductionLineGroup);
                if (!httpResult.Status)
                {
                    MessageBox.Show(httpResult.Message, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            });
            if (httpResult != null && httpResult.Status)
            {
                MessageBox.Show("保存成功!");
            }
        }
    }
}
