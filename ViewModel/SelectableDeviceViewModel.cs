using System.ComponentModel;
using SharpAdbClient;

namespace YueDroidBox.ViewModel
{
    public class SelectableDeviceViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public bool IsSelected { get; set; }

        public DeviceData DeviceData { get; set; }
        //public string Model { get; set; }
        //public string Serial { get; set; }
    }
}