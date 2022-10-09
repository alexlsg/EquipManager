using EquipDataManager.Bll;
using EquipDataManager.Dal;
using EquipModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using SystemSet;
using Tools;

namespace SystemSetClient.Pages
{
    /// <summary>
    /// UCEventSet.xaml 的交互逻辑
    /// </summary>
    public partial class UCEventSet : UserControl
    {
        ObservableCollection<EventSet> sets;
        public UCEventSet()
        {
            InitializeComponent();
            Loaded += UCEventSet_Loaded;
        }

        private void UCEventSet_Loaded(object sender, RoutedEventArgs e)
        {
            HttpResult httpResult = new EquipTypeService().GetEquipTypeList();
            if (httpResult.Status)
            {
                btn1.DataContext = httpResult.Data as List<EquipType>;
            }
            btn2.DataContext = CMDSetDal.GetList();
            Refresh();
        }
        void Refresh()
        {
            copysets.Clear();
            List<EventSet> _sets = EventSetDal.GetList();
            sets = new ObservableCollection<EventSet>(_sets);
            dg.ItemsSource = sets;
        }

        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            sets.Add(new EventSet());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (dg.SelectedItem != null)
            {
                foreach (var item in dg.SelectedItems)
                {
                    EventSet _es = item as EventSet;
                    if (_es.ID > 0)
                        _es.State = DataStatus.DEL;
                    else
                        sets.Remove(_es);
                }
                dg.Items.Filter = (n) =>
                {
                    return (n as EventSet).State != DataStatus.DEL;
                };
            }
        }
        List<EventSet> copysets = new List<EventSet>();
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            copysets.Clear();
            if (dg.SelectedItem != null)
            {
                foreach (var item in dg.SelectedItems)
                {
                    EventSet _es = item as EventSet;
                    copysets.Add(_es);
                }
            }
            MessageBox.Show($"已复制{copysets.Count}条数据!");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //if (copysets.Count > 0)
            //{
            //    foreach (var item in copysets)
            //    {
            //        sets.Add(new EventSet()
            //        {
            //            Condition = item.Condition,
            //            DataType = item.DataType,
            //            Desc = item.Desc,
            //            EquipID = item.EquipID,
            //            SpotNO = item.SpotNO,
            //            Value = item.Value,
            //        });
            //    }
            //}
            //else
            //    MessageBox.Show("没有复制的数据!");
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var item in sets)
                {
                    EventSetDal.SaveEventSet(item);
                }
                Refresh();
                MessageBox.Show("保存成功!");
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
