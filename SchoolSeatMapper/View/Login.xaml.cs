using System.Windows;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace SchoolSeatMapper
{
    /// <summary>
    /// login.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private async void request_btn_Click(object sender, RoutedEventArgs e)
        {
            string url = "http://login.ssmshinil.kro.kr/"; // 대상 URL

            // HttpClient 인스턴스 생성
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    // URL에서 HTML 문서 가져오기
                    string htmlContent = await httpClient.GetStringAsync(url);

                    string titlePattern = @"<title>(.*?)<\/title>";
                    Match match = Regex.Match(htmlContent, titlePattern);

                    if (match.Success)
                    {
                        string titleContent = match.Groups[1].Value.Trim();
                        if (pw.Text == titleContent)
                        {
                            Config.Set("login", id.Text);
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("PW 오류!");
                        this.Close();
                    }
                }
                catch (HttpRequestException ex)
                {
                    MessageBox.Show($"HTTP 요청 오류: {ex.Message}");
                    this.Close();
                }
            }
        }
    }
}
