using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Collections;
using System.Linq;
using System.Security.Permissions;

namespace SchoolSeatMapper
{
    // A class that represents the main window of the application
    public partial class SeatSelector : UserControl
    {
        public List<Seat> Seats1 { get; set; } // A list of seats to display
        public int Row { get; set; }
        public int Column { get; set; }

        public SeatSelector(int row, int column)
        {
            this.Row = row;
            this.Column = column;
            if (Config.Get("seat_num") != "0")
            {
                SeatsFromString(Config.Get("seat_num"), Config.Get("seat_available"), Config.Get("seat_selected"));
            }
            else
            {
                MessageBox.Show("올바르게 저장했는지 확인하세요", "에러 발생!");
            }
            InitializeComponent();
            // Set the data context of the window to this instance
            DataContext = this;
        }

        // A method that handles the click event of the seat buttons
        private void SeatButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the button object from the sender
            Button button = (Button)sender;

            // Get the seat object from the button's data context
            Seat seat = (Seat)button.DataContext;

            seat.Selected = !seat.Selected;
        }

        private void save_btn_Click(object sender, RoutedEventArgs e) // 엑셀로 바꿀 예정
        {
/*            string num = String.Join(",", Seats1.Select(m => m.Number).ToArray());
            string available = String.Join(",", Seats1.Select(m => m.Available).ToArray());
            string selected = String.Join(",", Seats1.Select(m => m.Selected).ToArray());
            Config.Set("seat_num", num);
            Config.Set("seat_available", available);
            Config.Set("seat_selected", selected);*/
        }

        private void SeatsFromString(string numberString, string availableString, string selectedString)
        {
            // 각 문자열을 ','를 기준으로 분할하여 배열로 변환
            string[] numbers = numberString.Split(',');
            string[] availableValues = availableString.Split(',');
            string[] selectedValues = selectedString.Split(',');

            // 분할된 값들을 사용하여 새로운 Seat 객체 리스트 생성
            Seats1 = new List<Seat> { };

            for (int i = 0; i < numbers.Length; i++)
            {
                Seats1.Add(new Seat { Number = int.Parse(numbers[i]), Available = bool.Parse(availableValues[i]), Selected = bool.Parse(selectedValues[i])  });
            }
        }

        private void mix_btn_Click(object sender, RoutedEventArgs e)
        {
            #region Seat Auto Select
            switch (Config.Get("auto_select"))
            {
                case "0":
                    break;

                case "1":   // Auto Select Mode 1 : Loose Separate / Diagonal 대각선
                    Loose_Separate();
                    break;

                case "2":   // Auto Select Mode 2 : Strict Separate / 3 by 3
                    Strict_Separate();
                    break;
            }
            #endregion
            #region Number_Rename
            var query = from Seat seat in Seats1
                        where seat.Available == false
                        select seat;
            foreach (Seat seat1 in query)
            {
                seat1.Number = null;
            }

            var query1 = from Seat seat in Seats1
                         where seat.Available == true
                         select seat;
            List<int> examples = new List<int>();
            int i = 0;
            int ex_result;
            for (int k = 1; k <= query1.Count(); k++)
            {
                do
                {
                    ex_result = new Random().Next(1, query1.Count() + 1);
                }
                while (examples.Contains(ex_result));

                examples.Add(ex_result);
            }

            foreach (Seat seat1 in query1)
            {
                seat1.Number = examples[i];
                i++;
            }
            #endregion
        }

        private void Loose_Separate()
        {
            List <int?> nums = new List <int?> ();
            List<int?> separate_num = new List<int?>();
            var query = from Seat seat in Seats1
                        select seat;
            foreach (Seat seat2 in query)
            {
                nums.Add(seat2.Number);
            }

            // 대각선 요소 선택
            List<int?> diagonalElements = SelectDiagonalElements(nums, Column);

            // 선택된 대각선 요소 출력
            foreach (var element in diagonalElements)
            {
                separate_num.Add(element);
            }

            // 누가, 몇명이 떨어져야 하는지
        }
        private void Strict_Separate()
        {
            MessageBox.Show("업데이트 예정 입니다.");
        }
        static List<int?> SelectDiagonalElements(List<int?> dataList, int columns)
        {
            int rows = dataList.Count / columns;

            // 대각선 요소 선택
            List<int?> diagonalElements = new List<int?>();
            for (int i = 0; i < rows; i++)
            {
                diagonalElements.Add(dataList[i * columns + i]);
            }

            return diagonalElements;
        }

        private void separate_btn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
