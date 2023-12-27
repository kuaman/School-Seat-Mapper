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
using AutoUpdaterDotNET;

namespace SchoolSeatMapper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Uri BaseUri = new Uri("https://raw.githubusercontent.com/kuaman/School-Seat-Mapper/master/version.xml");
            AutoUpdater.AppCastURL = BaseUri.AbsoluteUri;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Navigation.IsPaneOpen = false;
            Config.Set("row", "0");
            Config.Set("column", "0");
            Config.Set("seat_num", "0");
            Config.Set("seat_available", "false");
            Config.Set("seat_selected", "false");
            Config.Set("auto_select", "0");
            Config.Set("login", "none");
        }

        private void login_btn_Click(object sender, RoutedEventArgs e)
        {
            if (Config.Get("login") == "none")
            {
                Login login = new Login();
                login.Owner = this;
                login.ShowDialog();
            }
            else
            {
                box_loginid.Text = Config.Get("login");
                msgbox_login.Visibility = Visibility.Collapsed;
                login_btn.Visibility = Visibility.Collapsed;
            }
        }
    }
}