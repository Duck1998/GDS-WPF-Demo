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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GDS_WPF_Demo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        #region non-stupid stuff

        string[] GameDataList = new string[99]; //stupid but works :D

        string LOC = System.Environment.CurrentDirectory;

        //default setting
        string[] default_setting = new string[5]
        {
            "Hardware = 0",
            "Window = 0",
            "X64 = 1",
            "Exit = 0",
            "BackupLog = 0"
        };

        //files of CKAN
        string[] registry_json = new string[15]
        {   "{",
            "	\"registry_version\": 3,",
            "	\"sorted_repositories\": {",
            "		\"default\": {",
            "			\"name\": \"default\",",
            "			\"uri\": \"https://github.com/KSP-CKAN/CKAN-meta/archive/master.tar.gz\",",
            "			\"priority\": 0,",
            "			\"ckan_mirror\": false",
            "		}",
            "	},",
            "	\"available_modules\": {},",
            "	\"installed_dlls\": {},",
            "	\"installed_modules\": {},",
            "	\"installed_files\": {}",
            "}"
        };
        string[] installed_default_ckan = new string[10]
        {
            "{",
            "	\"kind\": \"metapackage\",",
            "	\"abstract\": \"A list of modules installed on the default KSP instance\",",
            "	\"name\": \"installed-default\",",
            "	\"license\": \"unknown\",",
            "	\"version\": \"1970.01.01.00.00.00\",",
            "	\"identifier\": \"installed-default\",",
            "	\"spec_version\": \"v0.0\",",
            "	\"depends\": []",
            "}"
        };

        #endregion

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BackupLogs_Click(object sender, RoutedEventArgs e)
        {
            if(BackupLogs.IsChecked)
            {

            }
            else
            {

            }
        }

        private void DeleteAllLogs_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ReportIssue_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Duck1998/GDS_WPF_Demo/issues/new");
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow window = new AboutWindow();
            window.Show();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
