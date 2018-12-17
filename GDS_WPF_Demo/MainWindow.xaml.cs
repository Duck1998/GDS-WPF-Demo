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
using System.IO;
using System.Text.RegularExpressions;

namespace GDS_WPF_Demo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        #region non-stupid stuff

        string[] GameDataList = new string[99]; //stupid but works :D

        string LOC = Environment.CurrentDirectory;

        //default setting
        string[] default_setting_x64 = new string[5]
        {
            "Hardware = 0",
            "Window = 0",
            "X64 = 1",
            "Exit = 0",
            "BackupLog = 0"
        };
        string[] default_setting = new string[5]
        {
            "Hardware = 0",
            "Window = 0",
            "X64 = 0",
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

        public void ReloadList()
        {
            ListGD.Items.Clear();
            int Counter = 0;
            DirectoryInfo dir = new DirectoryInfo(@".//");
            foreach (DirectoryInfo dChild in dir.GetDirectories("GameData*"))
            {
                if (File.Exists(LOC + "/" + dChild + "/GameDataData.data"))
                {
                    GameDataList[Counter] = dChild.Name;
                    ListGD.Items.Add(dChild.Name);
                    Counter++;
                }
            }

            //reset button & textbox
            TextBoxGD.IsEnabled = false;
            TextBoxGD.Text = "";
            TextBoxName.IsEnabled = false;
            TextBoxName.Text = "";
            ViewGD.IsEnabled = false;
            RenameGD.IsEnabled = false;
            SetDefaultGD.IsEnabled = false;
            CloneGD.IsEnabled = false;
            DeleteGD.IsEnabled = false;          
        }

        private void MainWindow_Init(object sender, EventArgs e)
        {
            //check environment
            if (!File.Exists(LOC + "/KSP_x64.exe") && !File.Exists(LOC + "/KSP.exe"))
            {
                MessageBox.Show("Cannot found executable file of KSP, please make sure this application is under KSP root folder, and try again.\nApplication will exit." , "File Not Found" , MessageBoxButton.OK , MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
            if (File.Exists(LOC + "/KSP_x64.exe") && !File.Exists(LOC + "/KSP.exe"))    //after 1.5
            {
                CheckBox64.IsEnabled = false;
                CheckBox64.Content = "x64 only";
                if (!Environment.Is64BitOperatingSystem)
                {
                    LaunchKSP.IsEnabled = false;
                    LaunchKSP.Content = "Require 64-bit OS to launch";
                }
            }
            else if (!File.Exists(LOC + "/KSP_x64.exe") || !Environment.Is64BitOperatingSystem)
            {
                CheckBox64.IsChecked = false;
                CheckBox64.IsEnabled = false;
                CheckBox64.Content = "no x64";
            }

            //check setting
            string _buff;
            if (File.Exists(LOC + "/GameDataSwitcherSetting.data"))
            {
                //read setting
                using (StreamReader setting = new StreamReader(LOC + "/GameDataSwitcherSetting.data"))
                {
                    _buff = setting.ReadToEnd();
                }
                Regex _reg = new Regex(@"[a-zA-Z_]\w*\s*=\s*(\d+(?!\.|x|e|d|m)u?)|^0x([\da-f]+(?!\.|x|m)u?)");
                MatchCollection mc = _reg.Matches(_buff);
                Dictionary<string, int> _mydic = new Dictionary<string, int>();
                foreach (Match nObj in mc)
                {
                    string _obj = nObj.Value;
                    _obj = _obj.Replace(" ", "");
                    _mydic.Add(_obj.Split('=')[0], Convert.ToInt32(_obj.Split('=')[1]));
                }

                //apply setting
                switch (_mydic["Hardware"])
                {
                    case 0:
                    case 2:
                        GraphicAPI.SelectedIndex = 0;   //default(DX9), 2 cases for backward compatibility
                        break;
                    case 1:
                        GraphicAPI.SelectedIndex = 3;   //OpenGL
                        break;
                    case 3:
                        GraphicAPI.SelectedIndex = 1;   //DX11
                        break;
                    case 4:
                        GraphicAPI.SelectedIndex = 2;   //DX12
                        break;
                    case 5:
                        GraphicAPI.SelectedIndex = 4;   //OpenGL(core)
                        break;
                    default:
                        GraphicAPI.SelectedIndex = 0;   //fallback
                        break;
                }
                switch (_mydic["Window"])
                {
                    case 0:
                    case 1:
                        WindowBehaviour.SelectedIndex = _mydic["Window"];
                        break;
                    default:
                        WindowBehaviour.SelectedIndex = 0;  //fullscreen deleted, fallback
                        break;
                }
                if (_mydic["X64"] == 0)
                {
                    if(CheckBox64.IsEnabled)    //only when possible to change
                    {
                        CheckBox64.IsChecked = false;   //default = true, or controlled by check environment
                    }
                }
                if (_mydic["Exit"] == 1)
                {
                    CheckBoxExit.IsChecked = true;  //default = false
                }
                try
                {
                    if (_mydic["BackupLog"] == 1)
                    {
                        BackupLogs.IsChecked = true;    //default = false
                    }
                }
                catch
                {
                    File.AppendAllText(LOC + "/GameDataSwitcherSetting.data", "BackupLog = 0"); //backward compatibility
                }
            }
            else
            {
                //create default setting
                if (!File.Exists(LOC + "/KSP_x64.exe") || !Environment.Is64BitOperatingSystem)
                {
                    File.WriteAllLines(LOC + "/GameDataSwitcherSetting.data", default_setting);
                }
                else
                {
                    File.WriteAllLines(LOC + "/GameDataSwitcherSetting.data", default_setting_x64);
                }
            }

            //check GameData
            if (!File.Exists(LOC + "/GameData/GameDataData.data"))
            {
                File.WriteAllText(LOC + "/GameData/GameDataData.data", "original");
            }

            //load list
            ReloadList();
        }

        private void ListGD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListGD.SelectedIndex != -1) //not after refresh
            {
                //enable button & textbox
                TextBoxGD.IsEnabled = true;
                TextBoxGD.Text = GameDataList[ListGD.SelectedIndex];
                TextBoxName.IsEnabled = true;
                TextBoxName.Text = File.ReadAllText(LOC + "/" + TextBoxGD.Text + "/GameDataData.data");
                ViewGD.IsEnabled = true;
                RenameGD.IsEnabled = true;
                SetDefaultGD.IsEnabled = true;
                CloneGD.IsEnabled = true;
                DeleteGD.IsEnabled = true;
                //exclusion for default GD
                if (GameDataList[ListGD.SelectedIndex] == "GameData")
                {
                    SetDefaultGD.IsEnabled = false;
                    DeleteGD.IsEnabled = false;
                }
            }           
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            ReloadList();
        }

        private void BackupLogs_Click(object sender, RoutedEventArgs e)
        {
            if (BackupLogs.IsChecked)
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
