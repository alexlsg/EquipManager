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
            //设置一个值，此值指示数据绑定 TextBox 是否应显示与源的 Text 属性值一致的字符串 为false
            System.Windows.FrameworkCompatibilityPreferences.KeepTextBoxDisplaySynchronizedWithTextProperty = false;
            base.OnStartup(e);
            Tools.CheckAuthor.Check(null);
            MainWindow1 _main = new MainWindow1();
            FrmLogin _login = new FrmLogin();
            _login.ShowDialog();
            _main.ShowDialog();
        }
    }
}
