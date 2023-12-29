using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SchoolSeatMapper
{
    // A class that represents a seat with properties and events
    public class Seat : INotifyPropertyChanged
    {
        private string? name; // The seat Name
        private bool available; // The seat availability
        private bool selected; // The seat selection status

        public string? Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged(); }
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