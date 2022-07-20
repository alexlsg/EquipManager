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
    /// UCEquipType.xaml 的交互逻辑
    /// </summary>
    public partial class UCEquipType : UserControl
    {
        ObservableCollection<EquipType> EquipTypes { get; set; }
        EquipTypeService EquipTypeService { get; set; }
        public UCEquipType()
        {
            InitializeComponent();
            Loaded += UCEquipType_Loaded;
        }

        private void UCEquipType_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= UCEquipType_Loaded;
            EquipTypes = new ObservableCollection<EquipType>();
            EquipTypeService = new EquipTypeService();
            GetData();
        }
        /// <summary>
        /// 查询
        /// </summary>
        public void GetData()
        {
            EquipTypes.Clear();
            datagrid.ItemsSource = EquipTypes;
            HttpResult httpResult = EquipTypeService.GetEquipTypeList();
            if (httpResult.Status)
            {
                List<EquipType> equipTypes = httpResult.Data as List<EquipType>;
                for (int i = 0; i < equipTypes.Count; i++)
                {
                    EquipTypes.Add(equipTypes[i]);
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
                EquipTypes.ToList().FindAll(n => n.DataStatus != DataStatus.NONE).ForEach(n =>
                {
                    switch (n.DataStatus)
                    {
                        case DataStatus.ADD:
                            httpResult = EquipTypeService.AddEquipType(n);
                            break;
                        case DataStatus.MOD:
                            httpResult = EquipTypeService.ModEquipType(n);
                            break;
                        case DataStatus.DEL:
                            httpResult = EquipTypeService.DelEquipType(n);
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
            bool a = EquipTypes.ToList().Exists(n => string.IsNullOrWhiteSpace(n.EquipTypeName));
            if (a)
            {
                MessageBox.Show("设备类型信息存在空值！请检查！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            return a;
        }
        /// <summary>
        /// 判断重复
        /// </summary>
        /// <returns></returns>
        private bool JudgeRepeat()
        {
            int a = EquipTypes.ToList().Count(n => n.DataStatus != DataStatus.DEL);
            int b = EquipTypes.ToList().FindAll(n => n.DataStatus != DataStatus.DEL).Select(n => n.EquipTypeName).Distinct().Count();
            bool c = a == b;
            if (!c)
            {
                MessageBox.Show("设备类型名称不能重复！请检查！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
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
            if (datagrid.SelectedItem != null && datagrid.SelectedItem is EquipType)
            {
                EquipType EquipType = (EquipType)datagrid.SelectedItem;
                EquipType.DataStatus = DataStatus.DEL;
                datagrid.Items.Filter = (a) =>
                {
                    EquipType _EquipType = (EquipType)a;
                    return _EquipType.DataStatus != DataStatus.DEL;
                };
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            EquipTypes.Add(new EquipType());
        }
    }
}
