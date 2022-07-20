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
    /// UCProductionLine.xaml 的交互逻辑
    /// </summary>
    public partial class FrmProductionLine : Window
    {
        ObservableCollection<ProductionLine> ProductionLines { get; set; }
        ProductionLineService ProductionLineService { get; set; }
        public FrmProductionLine()
        {
            InitializeComponent();
            Loaded += UCProductionLine_Loaded;
        }

        private void UCProductionLine_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= UCProductionLine_Loaded;
            ProductionLines = new ObservableCollection<ProductionLine>();
            ProductionLineService = new ProductionLineService();
            GetData();
        }
        /// <summary>
        /// 查询
        /// </summary>
        public void GetData()
        {
            ProductionLines.Clear();
            datagrid.ItemsSource = ProductionLines;
            HttpResult httpResult = ProductionLineService.GetProductionLineList();
            if (httpResult.Status)
            {
                List<ProductionLine> _ProductionLines = httpResult.Data as List<ProductionLine>;
                for (int i = 0; i < _ProductionLines.Count; i++)
                {
                    ProductionLines.Add(_ProductionLines[i]);
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
                ProductionLines.ToList().FindAll(n => n.DataStatus != DataStatus.NONE).ForEach(n =>
                {
                    switch (n.DataStatus)
                    {
                        case DataStatus.ADD:
                            httpResult = ProductionLineService.AddProductionLine(n);
                            break;
                        case DataStatus.MOD:
                            httpResult = ProductionLineService.ModProductionLine(n);
                            break;
                        case DataStatus.DEL:
                            httpResult = ProductionLineService.DelProductionLine(n);
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
            bool a = ProductionLines.ToList().Exists(n => string.IsNullOrWhiteSpace(n.ProductionLineName));
            if (a)
            {
                MessageBox.Show("产线信息存在空值！请检查！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            return a;
        }
        /// <summary>
        /// 判断重复
        /// </summary>
        /// <returns></returns>
        private bool JudgeRepeat()
        {
            int a = ProductionLines.ToList().Count(n => n.DataStatus != DataStatus.DEL);
            int b = ProductionLines.ToList().FindAll(n => n.DataStatus != DataStatus.DEL).Select(n => n.ProductionLineName).Distinct().Count();
            bool c = a == b;
            if (!c)
            {
                MessageBox.Show("产线名称不能重复！请检查！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
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
            if (datagrid.SelectedItem != null && datagrid.SelectedItem is ProductionLine)
            {
                ProductionLine ProductionLine = (ProductionLine)datagrid.SelectedItem;
                ProductionLine.DataStatus = DataStatus.DEL;
                datagrid.Items.Filter = (a) =>
                {
                    ProductionLine _ProductionLine = (ProductionLine)a;
                    return _ProductionLine.DataStatus != DataStatus.DEL;
                };
            }
        }
    }
}
