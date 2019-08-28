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

        private readonly IViewModelFactory _viewModelFactory;
        private readonly DeviceViewModel _deviceViewModel;
        //private readonly PortForwardingViewModel _portForwardingViewModel;

        public ShellViewModel(IWindowManager windowManager, IViewManager viewManager,
            IViewModelFactory viewModelFactory)
        {
            this.DisplayName = "YueDroidBox";
            _windowManager = windowManager;
            _viewManager = viewManager;
            //_deviceViewModel = deviceViewModel;
            _viewModelFactory = viewModelFactory;
            //_portForwardingViewModel = portForwardingViewModel;
            _deviceViewModel = viewModelFactory.CreateDeviceViewModel();
            //InterTabClient = new CustomTabClient();
            InterTabClient = new InterTabClient();

            MenuItems = new[]
            {
                new MenuItemViewModel("Port Forwarding", _viewModelFactory.CreatePortForwardingViewModelViewModel),
            };
        }

        public void OpenTab(Func<Screen> content)
        {
            // Todo: content can be factory or viewmodel
            //var s = menuItemViewModel as Screen;
            //var s = _viewModelFactory.CreatePortForwardingViewModelViewModel(); // Todo: for testing
            var s = content.Invoke();
            var view = _viewManager.CreateViewForModel(s);
            _viewManager.BindViewToModel(view, s);
            TabContents.Add(new TabContent(s.DisplayName, view));
        }

        public void SelectDevice()
        {
            //var deviceList = new List<DeviceData>();
            //_deviceViewModel.ToggleSingleSelectionMode();
            _windowManager.ShowDialog(_deviceViewModel);
            Console.WriteLine(@"Selected devices:");

            foreach (var data in _deviceViewModel.GetSelectedDevice())
            {
                Console.WriteLine($"{data.Model}");
            }
        }

        public void OnLoaded()
        {
            Engine.Instance().StartServer();
        }

        void OnDeviceConnected(object sender, DeviceDataEventArgs e)
        {
            Console.WriteLine($@"The device {e.Device.Name} has connected to this PC");
        }

        public interface IViewModelFactory
        {
            DeviceViewModel CreateDeviceViewModel();
            PortForwardingViewModel CreatePortForwardingViewModelViewModel();
        }
    }
}
