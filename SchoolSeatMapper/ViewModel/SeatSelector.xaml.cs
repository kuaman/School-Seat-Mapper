using System.Windows;
using System.Windows.Controls;

namespace SchoolSeatMapper
{
    public partial class SeatSelector : UserControl
    {
        public List<Seat> Seats1 { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }

        #region General
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
            DataContext = this;
        }

        private void SeatButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Seat seat = (Seat)button.DataContext;
            seat.Selected = !seat.Selected;
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
                Seats1.Add(new Seat { Number = int.Parse(numbers[i]), Available = bool.Parse(availableValues[i]), Selected = bool.Parse(selectedValues[i]) });
            }
        }
        #endregion

        private void save_btn_Click(object sender, RoutedEventArgs e) // 엑셀로 바꿀 예정
        {
            MessageBox.Show("Day+2 업데이트 예정 입니다.");
            /*            string num = String.Join(",", Seats1.Select(m => m.Number).ToArray());
                        string available = String.Join(",", Seats1.Select(m => m.Available).ToArray());
                        string selected = String.Join(",", Seats1.Select(m => m.Selected).ToArray());
                        Config.Set("seat_num", num);
                        Config.Set("seat_available", available);
                        Config.Set("seat_selected", selected);*/
        }

        private void mix_btn_Click(object sender, RoutedEventArgs e)
        {
            var query = from Seat seat in Seats1    // Available == false => Number = null
                        where seat.Available == false
                        select seat;
            foreach (Seat seat1 in query)
            {
                seat1.Number = null;
            }

            switch (Config.Get("select_mode"))
            {
                case "0":
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
                    break;

                case "1":   // Auto Select Mode 1 : Loose Separate / Diagonal 대각선
                    Loose_Separate();
                    break;

                case "2":   // Auto Select Mode 2 : Strict Separate / 3 by 3
                    Strict_Separate();
                    break;
            }
        }

        private void Loose_Separate()
        {
            List<int?> nums = new List<int?>();
            List<int?> separate_num = new List<int?>();
            var query3 = from Seat seat in Seats1
                         select seat;
            foreach (Seat seat2 in query3)
            {
                nums.Add(seat2.Number);
            }

            int rows = nums.Count / Column;

            List<int?> diagonalElements = new List<int?>();
            for (int i = 0; i < rows; i++)
            {
                diagonalElements.Add(nums[i * Column + i]);
            }
            int k = 0;
            // 선택된 대각선 요소 출력
            foreach (var element in diagonalElements)
            {
                separate_num.Add(element);
            }
            var query4 = from Seat seat in Seats1
                         where separate_num.Contains(seat.Number)
                         select seat;
            foreach (Seat seat3 in query4)
            {
                seat3.Selected = true;
            }

            Err_chk(separate_num.Count());

            var query5 = from Seat seat in Seats1
                         where seat.Available == true || seat.Selected == true
                         select seat;
            /*            Err_chk();*/
        }

        private void Strict_Separate()
        {

        }

        private void Err_chk(int chk_num)  // 개수 < 분리
        {
            /*            if (chk_num > Seperate num) 
                        { 

                        }*/
        }

        private void Select_Query()     // select_mode 1,2
        {

        }

        private void Rand_Mapping()
        {

        }

        private void separate_btn_Click(object sender, RoutedEventArgs e) // 분리 버튼
        {
            MessageBox.Show("Day+2 업데이트 예정 입니다.");
        }
    }
}
