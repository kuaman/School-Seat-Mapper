using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace SchoolSeatMapper
{
    /// <summary>
    /// Info.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Info : UserControl
    {
        public Info()
        {
            InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo { FileName = e.Uri.AbsoluteUri, UseShellExecute = true });
            e.Handled = true; // Page에서 내부 탐색 안되도록
        }

        // Display the underline on only the MouseEnter event.
        private void OnMouseEnter(object sender, EventArgs e)
        {
            (sender as System.Windows.Documents.Hyperlink).TextDecorations = TextDecorations.Underline;
        }

        // Remove the underline on the MouseLeave event.
        private void OnMouseLeave(object sender, EventArgs e)
        {
            (sender as System.Windows.Documents.Hyperlink).TextDecorations = null;
        }
    }
}
