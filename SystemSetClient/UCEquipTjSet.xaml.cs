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
    /// UCEquipTjSet.xaml 的交互逻辑
    /// </summary>
    public partial class UCEquipTjSet : UserControl
    {
        ObservableCollection<EquipTjSet> EquipTjSets { get; set; }
        EquipTjSetService EquipTjSetService { get; set; }

        public UCEquipTjSet()
        {
            InitializeComponent();
            Loaded += UCEquipTjSet_Loaded;
        }

        private void UCEquipTjSet_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= UCEquipTjSet_Loaded;
            EquipTjSets = new ObservableCollection<EquipTjSet>();
            EquipTjSetService = new EquipTjSetService();
            datagrid.ItemsSource = EquipTjSets;
            GetData();
        }
        /// <summary>
        /// 查询
        /// </summary>
        public void GetData()
        {
            EquipTjSets.Clear();
            HttpResult httpResult = EquipTjSetService.GetEquipTjSetList();
            List<EquipTjSet> _EquipTjSets = (List<EquipTjSet>)httpResult.Data;
            if (httpResult.Status)
            {
                for (int i = 0; i < _EquipTjSets.Count; i++)
                {
                    EquipTjSets.Add(_EquipTjSets[i]);
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
            bool empty = JudgeEmpty();
            if (!empty)
            {
                EquipTjSets.ToList().FindAll(n => n.DataStatus != DataStatus.NONE).ForEach(n =>
                {
                    switch (n.DataStatus)
                    {
                        case DataStatus.ADD:
                            httpResult = EquipTjSetService.AddEquipTjSet(n);
                            break;
                        case DataStatus.MOD:
                            httpResult = EquipTjSetService.ModEquipTjSet(n);
                            break;
                        case DataStatus.DEL:
                            httpResult = EquipTjSetService.DelEquipTjSet(n);
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
            bool a = EquipTjSets.ToList().Exists(n => string.IsNullOrWhiteSpace(n.SpotNO) || n.EquipType == 0
            || string.IsNullOrWhiteSpace(n.Tjlx) || string.IsNullOrWhiteSpace(n.Tjzt) || string.IsNullOrWhiteSpace(n.Data) || string.IsNullOrWhiteSpace(n.Tjtj));
            if (a)
            {
                MessageBox.Show("设备统计信息存在空值！请检查！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            return a;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (datagrid.SelectedItem != null && datagrid.SelectedItem is EquipTjSet)
            {
                EquipTjSet EquipTjSet = (EquipTjSet)datagrid.SelectedItem;
                EquipTjSet.DataStatus = DataStatus.DEL;
                datagrid.Items.Filter = (a) =>
                {
                    EquipTjSet _EquipTjSet = (EquipTjSet)a;
                    return _EquipTjSet.DataStatus != DataStatus.DEL;
                };
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            EquipTjSets.Add(new EquipTjSet());
        }
    }
}
