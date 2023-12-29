using ClosedXML.Excel;
using Microsoft.Win32;
using SchoolSeatMapper.ViewModel;
using System.Text;
using System.Windows;
using System.Windows.Input;


namespace SchoolSeatMapper
{
    /// <summary>
    /// SelectWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SelectWindow : Window
    {
        public SelectWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
            Height = ((MainWindow)Application.Current.MainWindow).Height;
            Left = ((MainWindow)Application.Current.MainWindow).Left + 786;
            Top = ((MainWindow)Application.Current.MainWindow).Top;
        }

        private void TextBox1_Add()
        {
            listBox1.Items.Add(textbox1.Text);
            textbox1.Text = string.Empty;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (textbox1.Text == "")
            {
                try
                {
                    // 엑셀 파일 열기 대화상자 표시
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Filter = "Excel 파일 (*.xlsx;*.xls)|*.xlsx;*.xls";
                    if (openFileDialog.ShowDialog() == true)
                    {
                        // 엑셀 파일 읽기
                        using (var workbook = new XLWorkbook(openFileDialog.FileName))
                        {
                            var worksheet = workbook.Worksheet(1); // 첫 번째 시트 선택

                            // 데이터 읽기
                            var cells = worksheet.CellsUsed();
                            foreach (var cell in cells)
                            {
                                listBox1.Items.Add(cell.Value);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"오류: {ex.Message}");
                }
            }
            else
            {
                TextBox1_Add();
            }
        }

        private void Del_Click(object sender, RoutedEventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
                return;
            listBox1.Items.RemoveAt(listBox1.SelectedIndex);
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (textbox1.Text != "")
                    TextBox1_Add();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closed(object sender, EventArgs e)
        {

        }

        public event EventHandler<string> DataAdded;

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder combinedString = new StringBuilder();
            foreach (var item in listBox1.Items)
            {
                combinedString.Append(item.ToString());
                combinedString.Append(",");
            }

            // 이벤트를 통해 결합된 문자열을 전송
            DataAdded?.Invoke(this, combinedString.ToString().Trim());
            this.Close();
        }
    }
}
