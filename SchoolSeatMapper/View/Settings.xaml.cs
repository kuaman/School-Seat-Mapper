using System.Windows.Controls;

namespace SchoolSeatMapper
{
    /// <summary>
    /// Settings.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Settings : Page
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void autosave_combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (autosave_combo.SelectedItem != null)
            {
                switch (autosave_combo.SelectedItem)
                {
                    case 0:
                        Config.Set("select_mode", "0");
                        break;

                    case 1:
                        Config.Set("select_mode", "1");
                        break;

                    case 2:
                        Config.Set("select_mode", "2");
                        break;
                }
            }
        }
    }
}
