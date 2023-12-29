using System.Windows;
using System.Windows.Controls;

namespace SchoolSeatMapper
{
    public partial class SeatSelector : UserControl
    {
        public List<Seat> Seats1 { get; set; }

        private List<Seat> ChangeSeat { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }

        public string[] trimmedStrings;

        #region General
        public SeatSelector(int row, int column)
        {
            this.Row = row;
            this.Column = column;
            SeatsFromString(Config.Get("seat_num"), Config.Get("seat_available"), Config.Get("seat_selected"));
            InitializeComponent();
            DataContext = this;
        }

        private void SeatButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Seat seat = (Seat)button.DataContext;
            seat.Selected = !seat.Selected;
        }

        private void SeatsFromString(string NameString, string availableString, string selectedString)
        {
            string[] Names = NameString.Split(',');
            string[] availableValues = availableString.Split(',');
            string[] selectedValues = selectedString.Split(',');

            Seats1 = new List<Seat> { };
            ChangeSeat = new List<Seat> { };

            for (int i = 0; i < Names.Length; i++)
            {
                Seats1.Add(new Seat { Name = Names[i], Available = bool.Parse(availableValues[i]), Selected = bool.Parse(selectedValues[i]) });
                ChangeSeat.Add(new Seat { Name = Names[i], Available = bool.Parse(availableValues[i]), Selected = bool.Parse(selectedValues[i]) });
            }
        }
        #endregion

        private void save_btn_Click(object sender, RoutedEventArgs e) // 엑셀로 바꿀 예정
        {
            MessageBox.Show("Day+2 업데이트 예정 입니다.");
            /*            string num = String.Join(",", Seats1.Select(m => m.Name).ToArray());
                        string available = String.Join(",", Seats1.Select(m => m.Available).ToArray());
                        string selected = String.Join(",", Seats1.Select(m => m.Selected).ToArray());
                        Config.Set("seat_num", num);
                        Config.Set("seat_available", available);
                        Config.Set("seat_selected", selected);*/
        }

        private void mix_btn_Click(object sender, RoutedEventArgs e)
        {
            var query = from Seat seat in Seats1    // Available == false => Name = null
                        where seat.Available == false
                        select seat;
            foreach (Seat seat1 in query)
            {
                seat1.Name = null;
            }
            Config.Set("Error", "1");
            switch (Config.Get("select_mode"))
            {
                case "0":
                    var query1 = from Seat seat in Seats1
                                 where seat.Available == true
                                 select seat;
                    List<string?> examples = new List<string?>();
                    int i = 0;
                    int ex_result;
                    for (int k = 1; k <= query1.Count(); k++)
                    {
                        do
                        {
                            ex_result = new Random().Next(1, query1.Count() + 1);
                        }
                        while (examples.Contains(ex_result.ToString()));

                        examples.Add(ex_result.ToString());
                    }

                    foreach (Seat seat1 in query1)
                    {
                        seat1.Name = examples[i];
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
            List<string?> nums = new List<string?>();
            List<string?> separate_num = new List<string?>();
            var query3 = from Seat seat in Seats1
                         select seat;
            foreach (Seat seat2 in query3)
            {
                nums.Add(seat2.Name);
            }
            int count = 1;
            List<string?> diagonalElements = new List<string?>();
            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Column; j++)
                {
                    if ((i + j) % 2 != 0)
                    {
                        diagonalElements.Add(count.ToString());
                    }
                    else
                    {

                    }
                    count++;
                }
            }
            foreach (var element in diagonalElements)
            {
                separate_num.Add(element);
            }
            var query4 = from Seat seat in ChangeSeat
                         where separate_num.Contains(seat.Name)
                         select seat;
            foreach (Seat seat3 in query4)
            {
                seat3.Selected = true;
            }

            Err_chk(separate_num.Count(), 0);

            var query5 = from Seat seat in ChangeSeat
                         where seat.Available == true || seat.Selected == false
                         select seat;
            Err_chk(query5.Count(), 1);
        }

        private void Strict_Separate()
        {
            List<string?> nums = new List<string?>();
            List<string?> separate_num = new List<string?>();
            var query3 = from Seat seat in Seats1
                         select seat;
            foreach (Seat seat2 in query3)
            {
                nums.Add(seat2.Name);
            }
            int count = 1;
            List<string?> diagonalElements = new List<string?>();
            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Column; j++)
                {
                    if (i % 2 == 0 || j % 2 == 0)
                    {
                        diagonalElements.Add(count.ToString());
                    }
                    else
                    {

                    }
                    count++;
                }
            }
            foreach (var element in diagonalElements)
            {
                separate_num.Add(element);
            }
            var query4 = from Seat seat in ChangeSeat
                         where separate_num.Contains(seat.Name)
                         select seat;
            foreach (Seat seat3 in query4)
            {
                seat3.Selected = true;
            }

            Err_chk(separate_num.Count(), 0);

            var query5 = from Seat seat in ChangeSeat
                         where seat.Available == true || seat.Selected == false
                         select seat;
            Err_chk(query5.Count(), 1);
        }

        private void Err_chk(int chk_num, int mode)  // 개수 < 분리
        {
            if (chk_num > trimmedStrings.Length)
            {
                if (mode == 1)
                {
                    Seats1 = ChangeSeat;
                }
                else
                {

                }
            }
            else
            {
                MessageBox.Show("분리 리스트를 다시 확인해주세요.");
            }
        }

        private void separate_btn_Click(object sender, RoutedEventArgs e) // 분리 버튼
        {
            SelectWindow selectWindow = new SelectWindow();
            selectWindow.DataAdded += SelectWindow_DataAdded;
            selectWindow.ShowDialog();
        }

        private void SelectWindow_DataAdded(object sender, string list)
        {
            string[] strings = list.Split(',');
            // 배열의 마지막 요소 제거
            if (strings.Length > 0)
            {
                trimmedStrings = new string[strings.Length - 1];
                Array.Copy(strings, trimmedStrings, strings.Length - 1);
            }
        }
    }
}
