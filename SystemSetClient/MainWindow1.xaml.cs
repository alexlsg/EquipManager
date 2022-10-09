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
using SystemSetClient.Pages;
using SystemSetClient.Pages.ComSet;
using SystemSetClient.Pages.EquipGroupSet;
using SystemSetClient.Pages.EquipSet;

namespace SystemSetClient
{
    /// <summary>
    /// MainWindow1.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow1 : Window
    {
        public MainWindow1()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            maincc.Content = new UCComSet();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            maincc.Content = new UCEquipType();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            maincc.Content = new UCEquipSet();
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            maincc.Content = new UCEquipGroupSet();
        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            maincc.Content = new UCEquipTjSet();
        }

        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            maincc.Content = new UCSpotSet();
        }

        private void MenuItem_Click_6(object sender, RoutedEventArgs e)
        {
            WinAbout _about = new WinAbout();
            _about.Owner = this;
            _about.ShowDialog();
        }

        private void MenuItem_Click_7(object sender, RoutedEventArgs e)
        {
            maincc.Content = new UCEventSet();
        }

        private void MenuItem_Click_8(object sender, RoutedEventArgs e)
        {
            maincc.Content = new UCCMDSet();
        }

        private void MenuItem_Click_9(object sender, RoutedEventArgs e)
        {
            maincc.Content = new UCEventYzSet();
        }
    }
}
