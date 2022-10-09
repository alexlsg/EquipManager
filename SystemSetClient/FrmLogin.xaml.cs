using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Tools;
using UserManagement;

namespace SystemSetClient
{
    /// <summary>
    /// FrmLogin.xaml 的交互逻辑
    /// </summary>
    public partial class FrmLogin : Window
    {
        public FrmLogin()
        {
            InitializeComponent();
            Closing += FrmLogin_Closing;
        }

        private void FrmLogin_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!ok)
            {
                Process.GetCurrentProcess().Kill();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        bool ok = false;
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            UserService _us = new UserService();
            HttpResult _r = _us.Login(new User()
            {
                UserName = tb_user.Text,
                PassWord = Tools.DES.MD5E(tb_pwd.Password)
            });
            if (_r.Status)
            {
                ok = true;
                Close();
            }
            else
            {
                MessageBox.Show(_r.Message);
            }
        }
    }
}
