using System.Windows.Controls;

namespace SchoolSeatMapper.ViewModel
{
    /// <summary>
    /// RowColumn.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class RowColumn : UserControl
    {
        public RowColumn()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Config.Set("row", row_box.Text);
            Config.Set("column", column_box.Text);
        }
    }
}
