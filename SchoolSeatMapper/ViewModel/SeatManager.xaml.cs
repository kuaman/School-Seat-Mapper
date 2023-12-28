using System.Windows;
using System.Windows.Controls;

namespace SchoolSeatMapper
{
    // A class that represents the main window of the application
    public partial class SeatManager : UserControl
    {
        public List<Seat> Seats { get; set; } // A list of seats to display
        public int Row { get; set; }
        public int Column { get; set; }

        public SeatManager(int row, int column)
        {
            this.Row = row;
            this.Column = column;
            if (Config.Get("seat_num") == "0")
            {
                Generate_Seat(row, column);
            }
            else
            {
                SeatsFromString(Config.Get("seat_num"), Config.Get("seat_available"), Config.Get("seat_selected"));
            }
            InitializeComponent();
            // Set the data context of the window to this instance
            DataContext = this;
        }

        private void Generate_Seat(int row, int column)
        {
            Seats = new List<Seat> { };
            for (int i = 1; i <= row * column; i++)
            {
                Seats.Add(new Seat { Number = i, Available = true, Selected = false });
            }
        }

        // A method that handles the click event of the seat buttons
        private void SeatButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the button object from the sender
            Button button = (Button)sender;

            // Get the seat object from the button's data context
            Seat seat = (Seat)button.DataContext;
            seat.Selected = !seat.Selected; // Available 속성 => Selected로 표현
        }

        private void save_btn_Click(object sender, RoutedEventArgs e)
        {
            string num = String.Join(",", Seats.Select(m => m.Number).ToArray());
            string available = String.Join(",", Seats.Select(m => !m.Selected).ToArray());
            string selected = String.Join(",", Seats.Select(m => !m.Available).ToArray());
            Config.Set("seat_num", num);
            Config.Set("seat_available", available);
            Config.Set("seat_selected", selected);
        }

        private void SeatsFromString(string numberString, string availableString, string selectedString)
        {
            // 각 문자열을 ','를 기준으로 분할하여 배열로 변환
            string[] numbers = numberString.Split(',');
            /*            string[] availableValues = availableString.Split(',');*/
            string[] selectedValues = selectedString.Split(',');

            // 분할된 값들을 사용하여 새로운 Seat 객체 리스트 생성
            Seats = new List<Seat> { };

            for (int i = 0; i < numbers.Length; i++)
            {
                Seats.Add(new Seat { Number = int.Parse(numbers[i]), Available = true, Selected = bool.Parse(selectedValues[i]) }); // SeatSelector에서 다시 SeatManager로 값을 불러올 때는 Available 모두 true
            }
        }
    }
}
