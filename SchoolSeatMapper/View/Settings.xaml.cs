using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace SchoolSeatMapper
{
    /// <summary>
    /// Settings.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Settings : Page
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void autosave_combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (autosave_combo.SelectedIndex)
            {
                case 0:
                    Config.Set("mode", "0");
                    break;

                case 1:
                    Config.Set("mode", "1");
                    break;

                case 2:
                    Config.Set("mode", "2");
                    break;
            }
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Assembly assembly = Assembly.GetEntryAssembly();
            Setting_Version_Label.Content = $"현재 버전 : {assembly.GetName().Version}";
            switch (Config.Get("mode"))
            {
                case "0":
                    autosave_combo.SelectedIndex = 0;
                    break;

                case "1":
                    autosave_combo.SelectedIndex = 1;
                    break;

                case "2":
                    autosave_combo.SelectedIndex = 2;
                    break;
            }
        }

        private void Setting_Reset_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("모든 설정을 초기화 하시겠습니까?", "설정 초기화", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Config.Set("row", "0");
                Config.Set("column", "0");
                Config.Set("seat_num", "0");
                Config.Set("seat_available", "false");
                Config.Set("seat_selected", "false");
                Config.Set("mode", "0");
                Config.Set("login", "none");
                Process.Start(Process.GetCurrentProcess().MainModule.FileName);
                Application.Current.Shutdown();
            }
        }
    }
}
