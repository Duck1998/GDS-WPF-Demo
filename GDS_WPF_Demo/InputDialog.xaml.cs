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
    /// InputDialog.xaml 的交互逻辑
    /// </summary>
    public partial class InputDialog : Window
    {
        public InputDialog(string windowtitle)
        {
            InitializeComponent();
            Title = windowtitle;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            Answer.Focus();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        public string ReturnGDName
        {
            get
            {
                return Answer.Text;
            }
        }
    }
}
