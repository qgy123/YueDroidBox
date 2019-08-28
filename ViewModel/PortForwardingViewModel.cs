using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SharpAdbClient;
using Stylet;
using YueDroidBox.Core;

namespace YueDroidBox.ViewModel
{
    public class PortForwardingViewModel : Screen
    {
        public DeviceData CurrentDeviceData { get; set; }
        public ObservableCollection<ForwardData> ForwardItem { get; set; }

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
            GetPortForwardingInfo();
        }

        private void GetDefaultDevice()
        {
            CurrentDeviceData = Engine.Instance().GetDevices().First();
        }

        public void GetPortForwardingInfo()
        {
            if (CurrentDeviceData == null) return;

            IEnumerable<ForwardData> forwardList = AdbClient.Instance.ListForward(CurrentDeviceData);
            ForwardItem = new ObservableCollection<ForwardData>(forwardList);

        }

        public void OnRefresh() => GetPortForwardingInfo();

        public void OnCreatePF()
        {
            if (CurrentDeviceData == null) return;

            //AdbClient.Instance.CreateForward();
        }

        public void SelectDevice()
        {

            _deviceViewModel.ToggleSingleSelectionMode();
            var view = _viewManager.CreateViewForModel(_deviceViewModel);
            _viewManager.BindViewToModel(view, _deviceViewModel);
            _windowManager.ShowDialog(_deviceViewModel);

            var device = _deviceViewModel.GetSelectedDevice();

            CurrentDeviceData = device.Count == 0 ? null : _deviceViewModel.GetSelectedDevice()[0];
            GetPortForwardingInfo();
        }
    }
}