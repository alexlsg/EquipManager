using EquipDataManager.Dal;
using EquipModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace SystemSetClient.Pages
{
    /// <summary>
    /// UCEventYzSet.xaml 的交互逻辑
    /// </summary>
    public partial class UCEventYzSet : UserControl
    {
        ObservableCollection<EventYzSet> sets;
        public UCEventYzSet()
        {
            InitializeComponent();
            Loaded += UCEventYzSet_Loaded;
        }

        private void UCEventYzSet_Loaded(object sender, RoutedEventArgs e)
        {
            HttpResult httpResult = new EquipService().GetEquipList();
            if (httpResult.Status)
            {
                btn1.DataContext = httpResult.Data as List<SystemSet.Equip>;
            }
            btn2.DataContext = CMDSetDal.GetList();
            Refresh();
        }
        void Refresh()
        {
            List<EventYzSet> _sets = EventYzSetDal.GetList();
            sets = new ObservableCollection<EventYzSet>(_sets);
            dg.ItemsSource = sets;
        }

        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            sets.Add(new EventYzSet());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (dg.SelectedItem != null)
            {
                foreach (var item in dg.SelectedItems)
                {
                    EventYzSet _es = item as EventYzSet;
                    if (_es.ID > 0)
                        _es.State = DataStatus.DEL;
                    else
                        sets.Remove(_es);
                }
                dg.Items.Filter = (n) =>
                {
                    return (n as EventYzSet).State != DataStatus.DEL;
                };
            }
        }
       
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var item in sets)
                {
                    EventYzSetDal.SaveEventYzSet(item);
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
