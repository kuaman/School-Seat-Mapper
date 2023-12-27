using System;
using System.Collections.Generic;
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
