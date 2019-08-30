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
using YueDroidBox.Core.Factory;
using YueDroidBox.Util;
using YueDroidBox.ViewModel.Dialog;

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

        public ShellViewModel(IWindowManager windowManager, IViewManager viewManager,
            IViewModelFactory viewModelFactory)
        {
            this.DisplayName = "YueDroidBox";
            _windowManager = windowManager;
            _viewManager = viewManager;
            _viewModelFactory = viewModelFactory;
            _deviceViewModel = viewModelFactory.CreateDeviceViewModel();
            //InterTabClient = new CustomTabClient();
            InterTabClient = new InterTabClient();

            MenuItems = new[]
            {
                new MenuItemViewModel("Port Forwarding", _viewModelFactory.CreatePortForwardingViewModel),
                new MenuItemViewModel("Cmd Window", _viewModelFactory.CreateCmdControlViewModel),
            };
        }

        public void OpenTab(Func<Screen> content)
        {
            var s = content.Invoke();
            var view = _viewManager.CreateViewForModel(s);
            _viewManager.BindViewToModel(view, s);
            TabContents.Add(new TabContent(s.DisplayName, view));
        }

        public void SelectDevice()
        {
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


    }
}
