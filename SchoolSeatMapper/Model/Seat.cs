using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SchoolSeatMapper
{
    // A class that represents a seat with properties and events
    public class Seat : INotifyPropertyChanged
    {
        private int? number; // The seat number
        private bool available; // The seat availability
        private bool selected; // The seat selection status

        public int? Number
        {
            get { return number; }
            set { number = value; OnPropertyChanged(); }
        }

        public bool Available
        {
            get { return available; }
            set { available = value; OnPropertyChanged(); }
        }

        public bool Selected
        {
            get { return selected; }
            set { selected = value; OnPropertyChanged(); }
        }

        // A method that raises the PropertyChanged event
        public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // An event that notifies when a property changes
        public event PropertyChangedEventHandler PropertyChanged;
    }
}