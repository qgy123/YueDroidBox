using System.Collections.Generic;
using SharpAdbClient;
using Stylet;

namespace YueDroidBox.ViewModel
{
    public class PortForwardingViewModel : Screen
    {
        private readonly IWindowManager _windowManager;

        public PortForwardingViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;
            this.DisplayName = "Port Forwarding";
        }

        public void SelectDevice()
        {
            //var deviceList = new List<DeviceData>();
            //_windowManager.ShowDialog(new DeviceViewModel(ref deviceList, true));
        }
    }
}