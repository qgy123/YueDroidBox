using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Interactivity;
using MahApps.Metro.Controls;
using MaterialDesignThemes.Wpf;
using SharpAdbClient;
using Stylet;
using YueDroidBox.Core;
using YueDroidBox.Core.Factory;

namespace YueDroidBox.ViewModel
{
    public class PortForwardingViewModel : Screen
    {
        public DeviceData CurrentDeviceData { get; set; }
        public ObservableCollection<ForwardData> ForwardItem { get; set; }

        public object DialogContent { get; set; }

        public bool IsDialogOpen { get; set; }

        private readonly IWindowManager _windowManager;
        private readonly IViewManager _viewManager;
        private readonly IDialogModelFactory _dialogModelFactory;
        private readonly DeviceViewModel _deviceViewModel;

        public PortForwardingViewModel(IWindowManager windowManager, 
            IViewManager viewManager, 
            IViewModelFactory viewModelFactory,
            IDialogModelFactory dialogModelFactory)
        {
            _windowManager = windowManager;
            _viewManager = viewManager;
            _dialogModelFactory = dialogModelFactory;
            _deviceViewModel = viewModelFactory.CreateDeviceViewModel();
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

        public async void OnCreatePF()
        {
            if (CurrentDeviceData == null) return;
            var model = _dialogModelFactory.CreateAddPfDialogViewModel();
            //DialogContent = _viewManager.CreateAndBindViewForModelIfNecessary(model);
            //IsDialogOpen = true;

            var view = _viewManager.CreateAndBindViewForModelIfNecessary(model);
            //_windowManager.ShowDialog(view);
            //DialogHost.Show(view, "RootDialog", ExtendedOpenedEventHandler, ExtendedClosingEventHandler);
            //AdbClient.Instance.CreateForward();

            //var window = (Window)View.GetSelfAndAncestors().FirstOrDefault(a => a is Window);
            var window = Window.GetWindow(View);
            var result = await DialogHostEx.ShowDialog(window, view, ExtendedOpenedEventHandler, ExtendedClosingEventHandler);

            if ((bool)result)
            {
                Console.WriteLine(model.Local);
                Console.WriteLine(model.Remote);
            }

        }
        private void ExtendedOpenedEventHandler(object sender, DialogOpenedEventArgs eventargs)
        {
            Console.WriteLine("Opened...");
        }

        private void ExtendedClosingEventHandler(object sender, DialogClosingEventArgs eventargs)
        {
            Console.WriteLine("Closing...");
        }

        public void OnCloseDialog()
        {
            IsDialogOpen = false;
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