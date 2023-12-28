using System.ComponentModel;

namespace SchoolSeatMapper.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _row;
        private string _col;

        public string Row
        {
            get
            { return _row; }
            set
            {
                _row = value;
                OnPropertyChanged("Row");
            }
        }
        public string Column
        {
            get
            { return _col; }
            set
            {
                _col = value;
                OnPropertyChanged("Column");
            }
        }

        public MainViewModel()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
