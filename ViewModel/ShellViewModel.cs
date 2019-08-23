using System;
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

        public ShellViewModel(IWindowManager windowManager, IViewManager viewManager)
        {
            this.DisplayName = "YueDroidBox";
            _windowManager = windowManager;
            _viewManager = viewManager;

            //InterTabClient = new CustomTabClient(_viewManager);
            InterTabClient = new CustomTabClient();

            MenuItems = new[]
            {
                new MenuItemViewModel("Port Forwarding", new PortForwardingViewModel()),
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

        public void OnLoaded()
        {
            return;
            Engine.Instance().StartServer();
            var devices = Engine.Instance().GetDevices();
            var items = new ObservableCollection<SelectableDeviceViewModel>();

            foreach (var device in devices)
            {
                items.Add(new SelectableDeviceViewModel { Model = device.Model, Serial = device.Serial });
                Console.WriteLine($@"{device.Model}({device.Serial})");
            }
            var monitor = Engine.Instance().StartMonitor();

            _windowManager.ShowDialog(new DeviceViewModel(items));
        }

        void OnDeviceConnected(object sender, DeviceDataEventArgs e)
        {
            Console.WriteLine($@"The device {e.Device.Name} has connected to this PC");
        }
    }
}
