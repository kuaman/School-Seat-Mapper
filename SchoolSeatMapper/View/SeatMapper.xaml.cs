using SchoolSeatMapper.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
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

namespace SchoolSeatMapper
{
    /// <summary>
    /// SeatMapper.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SeatMapper : Page
    {
        private int seat_mode = 0; 
        public SeatMapper()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
        }

        private async void gen_seat_btn_Click(object sender, RoutedEventArgs e)
        {
            RowColumn rowColumn = new RowColumn();
            Wpf.Ui.Controls.MessageBox messageBox = new Wpf.Ui.Controls.MessageBox();
            messageBox.Content = rowColumn;
            messageBox.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            messageBox.Title = "자리 생성";
            messageBox.PrimaryButtonText = "생성";
            messageBox.CloseButtonText = "취소";
            if (await messageBox.ShowDialogAsync() == Wpf.Ui.Controls.MessageBoxResult.Primary)
            {
                row_label.Content = Config.Get("row"); ;
                column_label.Content = Config.Get("column");
                SeatManager seatManager = new SeatManager(ToInt(Config.Get("row")), ToInt(Config.Get("column")));
                control.Content = seatManager;
            }
            else
            {

            }
        }

        private void mix_seat_btn_Click(object sender, RoutedEventArgs e)
        {
            switch (seat_mode)
            {
                case 0:
                    SeatSelector seatSelector = new SeatSelector(ToInt(Config.Get("row")), ToInt(Config.Get("column")));
                    control.Content = seatSelector;
                    mix_seat_btn.Content = "자리 ";
                    seat_mode = 1;
                    break;

                case 1:
                    SeatManager seatManager = new SeatManager(ToInt(Config.Get("row")), ToInt(Config.Get("column")));
                    control.Content = seatManager;
                    mix_seat_btn.Content = "자리 섞기";
                    seat_mode = 0;
                    break;

                default:
                    break;
            }
        }

        private int ToInt(string original)
        {
            int returnVal;
            bool bl = int.TryParse(original, out returnVal);
            if (bl)
            {
                int result = int.Parse(original);
                return result;
            }
            else
            {
                return 0;
            }
        }
    }
}
