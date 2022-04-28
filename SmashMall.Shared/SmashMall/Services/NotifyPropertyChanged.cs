using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SmashMall.Services
{
    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        private string _property;
        private object _sender;
        public event PropertyChangedEventHandler PropertyChanged;

        public NotifyPropertyChanged()
        {

        }
        public NotifyPropertyChanged(object sender)
        {
            _sender = sender;
        }
        public string Property
        {
            get => _property;
            set
            {
                _property = value;
                OnPropertyChanged("Property");
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(_sender, new PropertyChangedEventArgs(propertyName));
        }
    }

}
