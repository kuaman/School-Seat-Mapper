using ClosedXML.Excel;
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
            // 엑셀 파일 생성
            using (var workbook = new XLWorkbook())
            {
                List<string?> nums = new List<string?>();
                var query3 = from Seat seat in Seats1
                             select seat;
                foreach (Seat seat2 in query3)
                {
                    nums.Add(seat2.Name);
                }
                // 워크시트 생성
                var worksheet = workbook.Worksheets.Add("Seats");
                int count = 0;
                // 데이터 추가
                for (int row = 1; row <= Row; row++)
                {
                    for (int column = 1; column <= Column; column++, count++)
                    {
                        string seatName = nums[count];
                        worksheet.Cell(row, column).Value = seatName;
                    }
                }

                // 파일 저장
                var saveFileDialog = new Microsoft.Win32.SaveFileDialog
                {
                    FileName = "Seats.xlsx",
                    DefaultExt = ".xlsx",
                    Filter = "Excel Files (.xlsx)|*.xlsx"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    workbook.SaveAs(saveFileDialog.FileName);
                    MessageBox.Show("엑셀 파일이 저장되었습니다.", "저장");
                }
            }
        }

        private void mix_btn_Click(object sender, RoutedEventArgs e)
        {
            switch (Config.Get("mode"))
            {
                case "0":
                    var query = from Seat seat in Seats1    // Available == false => Name = null
                                where seat.Available == false
                                select seat;
                    foreach (Seat seat1 in query)
                    {
                        seat1.Name = null;
                    }
                    var query1 = from Seat seat in Seats1
                                 where seat.Available == true
                                 select seat;
                    List<string?> Names = new List<string?>();
                    int i = 0;
                    string result;
                    for (int k = 1; k <= query1.Count(); k++)
                    {
                        do
                        {
                            result = new Random().Next(1, query1.Count() + 1).ToString();
                        }
                        while (Names.Contains(result));

                        Names.Add(result);
                    }

                    foreach (Seat seat1 in query1)
                    {
                        seat1.Name = Names[i];
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
            List<string?> separate_num = new List<string?>();
            int count = 1;
            for (int i = 0; i < Row; i++)   // 알고리즘
            {
                for (int j = 0; j < Column; j++)
                {
                    if ((i + j) % 2 != 0)
                    {
                        separate_num.Add(count.ToString());
                    }
                    count++;
                }
            }
            var query3 = from Seat seat in ChangeSeat
                         where separate_num.Contains(seat.Name)
                         select seat;
            foreach (Seat seat3 in query3)
            {
                if (seat3.Available == true)
                {
                    seat3.Selected = true;
                }
                else
                {
                    separate_num.Remove(seat3.Name);
                }
            }

            if (Err_chk(separate_num.Count()))
            {
                var query4 = from Seat seat in ChangeSeat
                             where seat.Available == true && seat.Selected == false
                             select seat;
                if (Err_chk(query4.Count()))     // 최종 자리 개수 / 분리 명단 개수 확인
                {
                    Seats1.Clear();
                    foreach (Seat seat in ChangeSeat)
                    {
                        Seats1.Add(new Seat
                        {
                            Name = seat.Name,
                            Available = seat.Available,
                            Selected = seat.Selected
                        });
                    }
                    Rand_Seat(separate_num);
                    itemcontrol.ItemsSource = null;
                    itemcontrol.ItemsSource = Seats1;
                    string num = String.Join(",", Seats1.Select(m => m.Name).ToArray());
                    string available = String.Join(",", Seats1.Select(m => m.Available).ToArray());
                    string selected = String.Join(",", Seats1.Select(m => m.Selected).ToArray());
                    Config.Set("seat_num", num);
                    Config.Set("seat_available", available);
                    Config.Set("seat_selected", selected);
                }
            }
        }

        private void Strict_Separate()
        {
            List<string?> separate_num = new List<string?>();
            int count = 1;
            for (int i = 0; i < Row; i++)   // 알고리즘
            {
                for (int j = 0; j < Column; j++)
                {
                    if (i % 2 == 0 && j % 2 == 0)
                    {
                        separate_num.Add(count.ToString());
                    }
                    count++;
                }
            }
            var query5 = from Seat seat in ChangeSeat
                         where separate_num.Contains(seat.Name)
                         select seat;
            foreach (Seat seat5 in query5)
            {
                if (seat5.Available == true)
                {
                    seat5.Selected = true;
                }
                else
                {
                    separate_num.Remove(seat5.Name);
                }
            }

            if (Err_chk(separate_num.Count()))
            {
                var query6 = from Seat seat in ChangeSeat
                             where seat.Available == true && seat.Selected == false
                             select seat;

                if (Err_chk(query6.Count()))     // 최종 자리 개수 / 분리 명단 개수 확인
                {
                    Seats1.Clear();
                    foreach (Seat seat in ChangeSeat)
                    {
                        Seats1.Add(new Seat
                        {
                            Name = seat.Name,
                            Available = seat.Available,
                            Selected = seat.Selected
                        });
                    }
                    Rand_Seat(separate_num);
                    itemcontrol.ItemsSource = null;
                    itemcontrol.ItemsSource = Seats1;
                    string num = String.Join(",", Seats1.Select(m => m.Name).ToArray());
                    string available = String.Join(",", Seats1.Select(m => m.Available).ToArray());
                    string selected = String.Join(",", Seats1.Select(m => m.Selected).ToArray());
                    Config.Set("seat_num", num);
                    Config.Set("seat_available", available);
                    Config.Set("seat_selected", selected);
                }
            }
        }

        private bool Err_chk(int chk_num)
        {
            try
            {
                if (chk_num >= trimmedStrings.Length)
                {
                    return true;
                }
                else
                {
                    MessageBox.Show($"분리 리스트 개수가 가능한 자리보다 많습니다.\n 분리 가능한 자리 수 : {chk_num}");
                    return false;
                }
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show("분리 리스트를 추가하지 않았습니다.");
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void Rand_Seat(List<string?> separateList)
        {
            var query7 = from Seat seat in Seats1    // Available == false => Name = null
                        where seat.Available == false
                        select seat;
            foreach (Seat seat7 in query7)
            {
                seat7.Name = null;
            }

            int count = separateList.Count();
            List<string?> Counts = new List<string?>();
            List<string?> Selected = new List<string?>();
            string result;
            for (int k = 1; k <= count; k++)
            {
                do
                {
                    result = new Random().Next(0, count).ToString();
                }
                while (Counts.Contains(result));

                Counts.Add(result);
            }
            var query8 = from Seat seat in Seats1
                         where seat.Selected == true
                         select seat;
            int j = 0;
            foreach (Seat seat1 in query8)
            {
                try
                {
                    seat1.Name = trimmedStrings[Convert.ToInt32(Counts[j])];
                    Selected.Add(trimmedStrings[Convert.ToInt32(Counts[j])]);
                }
                catch
                {
                    seat1.Selected = false;
                }
                j++;
            }

            var query9 = from Seat seat in Seats1
                          where seat.Available == true
                          select seat;
            List<string?> Counts1 = Enumerable.Range(1, query9.Count()).Select(i => i.ToString()).ToList();
            List<string?> Counts2 = new List<string?>();
            Counts2 = Counts1.Except(Selected).ToList();

            var query10 = from Seat seat in Seats1
                         where seat.Available == true && seat.Selected == false
                         select seat;
            Counts2 = Counts2.OrderBy(x => Guid.NewGuid()).ToList();
            foreach (Seat seat1 in query10)
            {
                if (Counts2.Count > 0)
                {
                    seat1.Name = Counts2[0];  // Counts2 리스트의 첫 번째 요소를 할당
                    Counts2.RemoveAt(0);     // 할당한 요소를 리스트에서 제거
                }
                else
                {
                    break;
                }
            }
        }

        private void separate_btn_Click(object sender, RoutedEventArgs e) // 분리 버튼
        {
            SelectWindow selectWindow = new SelectWindow(Row * Column);
            selectWindow.DataAdded += SelectWindow_DataAdded;
            selectWindow.ShowDialog();
        }

        private void SelectWindow_DataAdded(object sender, string list)
        {
            string[] strings = list.Split(',');
            if (strings.Length > 0) // 배열의 마지막 요소 제거
            {
                trimmedStrings = new string[strings.Length - 1];
                Array.Copy(strings, trimmedStrings, strings.Length - 1);
            }
        }
    }
}
