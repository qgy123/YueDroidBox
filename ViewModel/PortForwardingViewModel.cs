using System;
using System.Collections.Generic;
using System.Linq;
using SharpAdbClient;
using Stylet;
using YueDroidBox.Core;

namespace YueDroidBox.ViewModel
{
    public class PortForwardingViewModel : Screen
    {
        public DeviceData CurrentDeviceData { get; set; }

        private readonly IWindowManager _windowManager;
        private readonly IViewManager _viewManager;
        private readonly DeviceViewModel _deviceViewModel;

        public PortForwardingViewModel(IWindowManager windowManager, IViewManager viewManager, DeviceViewModel deviceViewModel)
        {
            _windowManager = windowManager;
            _viewManager = viewManager;
            _deviceViewModel = deviceViewModel;
            this.DisplayName = "Port Forwarding";
        }

        protected override void OnViewLoaded()
        {
            Init();
        }

        public void Init()
        {
            GetDefaultDevice();
        }

        private void GetDefaultDevice()
        {
            CurrentDeviceData = Engine.Instance().GetDevices().First();
        }

        public void SelectDevice()
        {

            _deviceViewModel.ToggleSingleSelectionMode();
            var view = _viewManager.CreateViewForModel(_deviceViewModel);
            _viewManager.BindViewToModel(view, _deviceViewModel);
            _windowManager.ShowDialog(_deviceViewModel);
            CurrentDeviceData = _deviceViewModel.GetSelectedDevice()[0];
        }
    }
}