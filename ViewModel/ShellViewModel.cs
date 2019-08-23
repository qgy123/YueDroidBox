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
        private readonly PortForwardingViewModel _portForwardingViewModel;

        public ShellViewModel(IWindowManager windowManager, IViewManager viewManager,
            PortForwardingViewModel portForwardingViewModel)
        {
            this.DisplayName = "YueDroidBox";
            _windowManager = windowManager;
            _viewManager = viewManager;
            _portForwardingViewModel = portForwardingViewModel;

            InterTabClient = new CustomTabClient();

            MenuItems = new[]
            {
                new MenuItemViewModel("Port Forwarding", _portForwardingViewModel),
            };
        }

        public void OpenTab(object sender, MouseButtonEventArgs e)
        {

            if (e.OriginalSource is TextBlock t)
            {
                var context = t.DataContext;

                if (context != null && context is MenuItemViewModel m)
                {
                    var s = m.Content as Screen;
                    var view = _viewManager.CreateViewForModel(s);
                    TabContents.Add(new TabContent(s.DisplayName, view));
                }
            }

        }

        public void SelectDevice()
        {
            var deviceList = new List<DeviceData>();
            _windowManager.ShowDialog(new DeviceViewModel(ref deviceList, false));
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
