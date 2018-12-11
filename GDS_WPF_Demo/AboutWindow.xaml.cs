using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GDS_WPF_Demo
{
    /// <summary>
    /// AboutWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
            AppVer.Content = "WPF Demo v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString().Remove(5) + "-alpha";
        }

        private void GitHub_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Duck1998/GDS_WPF_Demo");
        }

        private void Forum_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SpaceDock_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ReportIssue_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Duck1998/GDS_WPF_Demo/issues/new");
        }

        private void Patreon_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
