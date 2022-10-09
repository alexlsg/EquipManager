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
using Tools;

namespace SystemSetClient.Pages
{
    /// <summary>
    /// UCCMDSet.xaml 的交互逻辑
    /// </summary>
    public partial class UCCMDSet : UserControl
    {
        ObservableCollection<CMDSet> sets;
        public UCCMDSet()
        {
            InitializeComponent();
            Loaded += UCCMDSet_Loaded;
        }

        private void UCCMDSet_Loaded(object sender, RoutedEventArgs e)
        {
            Refresh();
        }
        void Refresh()
        {
            List<CMDSet> _sets = CMDSetDal.GetList();
            sets = new ObservableCollection<CMDSet>(_sets);
            dg.ItemsSource = sets;
        }

        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            sets.Add(new CMDSet());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (dg.SelectedItem != null)
            {
                foreach (var item in dg.SelectedItems)
                {
                    CMDSet _es = item as CMDSet;
                    if (_es.ID > 0)
                        _es.State = DataStatus.DEL;
                    else
                        sets.Remove(_es);
                }
                dg.Items.Filter = (n) =>
                {
                    return (n as CMDSet).State != DataStatus.DEL;
                };
            }
        }
       

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var item in sets)
                {
                    CMDSetDal.SaveCMDSet(item);
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
