using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using Dragablz;
using SharpAdbClient;
using Stylet;
using YueDroidBox.Core;
using YueDroidBox.Util;

namespace YueDroidBox.ViewModel
{
    public class ShellViewModel : Screen
    {
        public MenuItemViewModel[] MenuItems { get; }
        public ObservableCollection<TabContent> TabContents { get; set; } = new ObservableCollection<TabContent>();
        public static IInterTabClient InterTabClient { get; set; }

        private readonly IWindowManager _windowManager;
        private readonly IViewManager _viewManager;
        private readonly DeviceViewModel _deviceViewModel;
        private readonly PortForwardingViewModel _portForwardingViewModel;

        public ShellViewModel(IWindowManager windowManager, IViewManager viewManager, DeviceViewModel deviceViewModel,
            PortForwardingViewModel portForwardingViewModel)
        {
            this.DisplayName = "YueDroidBox";
            _windowManager = windowManager;
            _viewManager = viewManager;
            _deviceViewModel = deviceViewModel;
            _portForwardingViewModel = portForwardingViewModel;

            InterTabClient = new CustomTabClient();

            MenuItems = new[]
            {
                new MenuItemViewModel("Port Forwarding", _portForwardingViewModel),
            };
        }

        public void OpenTab(object menuItemViewModel)
        {
            var s = menuItemViewModel as Screen;
            var view = _viewManager.CreateViewForModel(s);
            TabContents.Add(new TabContent(s.DisplayName, view));
        }

        public void SelectDevice()
        {
            //var deviceList = new List<DeviceData>();
            _windowManager.ShowDialog(_deviceViewModel);
        }

        public void OnLoaded()
        {
            Engine.Instance().StartServer();
            return;
        }

        void OnDeviceConnected(object sender, DeviceDataEventArgs e)
        {
            Console.WriteLine($@"The device {e.Device.Name} has connected to this PC");
        }
    }
}
