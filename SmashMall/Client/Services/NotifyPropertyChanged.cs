using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SmashMall.Client.Services
{
    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        private string property;
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
            get => property;
            set
            {
                property = value;
                OnPropertyChanged("Propery");
            }
        }
        void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
