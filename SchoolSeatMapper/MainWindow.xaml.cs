﻿using AutoUpdaterDotNET;
using System.Diagnostics;
using System.IO;
using System.Windows;

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
            AutoUpdater.Start("https://raw.githubusercontent.com/kuaman/School-Seat-Mapper/master/version.xml");
        }

        public readonly string Cfpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\SchoolSeatMapper";

        private void InitialConfig()
        {
            AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", Cfpath + @"\app.config");
            AppConfig.Change(Cfpath + @"\app.config");

            if (!File.Exists(Cfpath + @"\app.config"))
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
            else
            {
                Config.Set("row", "0");
                Config.Set("column", "0");
                Config.Set("seat_num", "0");
                Config.Set("seat_available", "false");
                Config.Set("seat_selected", "false");
                Config.Set("mode", "0");
                Config.Set("login", "none");
            }

            DirectoryInfo di = new(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\SchoolSeatMapper");

            if (!di.Exists)
            {
                di.Create();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Navigation.Navigate("자리 배정");
            Navigation.IsPaneOpen = false;
            InitialConfig();
        }

        private void login_btn_Click(object sender, RoutedEventArgs e)
        {
            if (Config.Get("login") == "none")
            {
                Login login = new Login();
                login.Owner = this;
                login.DataAdded += Login_DataAdded;
                login.ShowDialog();
            }
            else
            {
                box_loginid.Text = Config.Get("login");
                box_loginid.Visibility = Visibility.Visible;
                msgbox_login.Visibility = Visibility.Collapsed;
                login_btn.Visibility = Visibility.Collapsed;
            }
        }

        private void Login_DataAdded(object sender, string id)
        {
            box_loginid.Text = id;
            box_loginid.Visibility = Visibility.Visible;
            msgbox_login.Visibility = Visibility.Collapsed;
            login_btn.Visibility = Visibility.Collapsed;
        }
    }
}