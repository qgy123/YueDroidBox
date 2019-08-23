using System.ComponentModel;

namespace YueDroidBox.ViewModel
{
    public class SelectableDeviceViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsSelected { get; set; }
        public string Model { get; set; }
        public string Serial { get; set; }
    }
}