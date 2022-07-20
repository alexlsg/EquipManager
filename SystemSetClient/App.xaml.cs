using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SystemSetClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Tools.CheckAuthor.Check(null);
            MainWindow1 _main = new MainWindow1();
            FrmLogin _login = new FrmLogin();
            _login.ShowDialog();
            _main.ShowDialog();
        }
    }
}
