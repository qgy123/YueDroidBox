using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using MaterialDesignThemes.Wpf;
using SharpAdbClient;
using Stylet;
using YueDroidBox.Core;

namespace YueDroidBox.ViewModel
{
    public class DeviceViewModel : Screen
    {
        private readonly IViewManager _viewManager;
        private readonly WaitingDialogViewModel _waitingDialogViewModel;
        private readonly List<DeviceData> _selectedDevices;

        public bool IsDialogOpen { get; set; }

        public ObservableCollection<SelectableDeviceViewModel> Items { get; set; }
        public bool CurVisibility { get; set; } = true;
        public int ConfirmHeight { get; set; } = 35;
        public int CurRow { get; set; } = 1;

        private Task<bool> _deviceTask;

        public DeviceViewModel(IViewManager viewManager, WaitingDialogViewModel waitingDialogViewModel)
        {
            _viewManager = viewManager;
            _waitingDialogViewModel = waitingDialogViewModel;

            this.DisplayName = "select devices";
        }

        //public DeviceViewModel(ref List<DeviceData> selectedDevices, bool singleSelectionMode = false)
        //{
        //    _selectedDevices = selectedDevices;

        //    this.DisplayName = singleSelectionMode == false ? "select devices" : "select device";

        //    if (singleSelectionMode)
        //    {
        //        ToggleSingleSelectionMode();
        //    }
        //}

        public void ToggleSingleSelectionMode()
        {
            CurVisibility = false;
            ConfirmHeight = 0;
            CurRow = 0;
        }

        public static void SelectAll(bool select, IEnumerable<SelectableDeviceViewModel> models)
        {
            if (models == null) return;

            foreach (var model in models)
            {
                model.IsSelected = select;
            }
        }

        public void OnSelectAll()
        {
            SelectAll(true, Items);
        }

        public List<DeviceData> GetDevices()
        {
            return Engine.Instance().GetDevices();

            //await Execute.PostToUIThreadAsync(() =>
            //{

            //    Items = new ObservableCollection<SelectableDeviceViewModel>();

            //    foreach (var device in devices)
            //    {
            //        Items.Add(new SelectableDeviceViewModel { Model = device.Model, Serial = device.Serial });
            //        Console.WriteLine($@"{device.Model}({device.Serial})");
            //    }
            //});
        }

        public void OnClick(string argument)
        {
            Console.WriteLine($@"Argument is {argument}");
        }

        public override Task<bool> CanCloseAsync()
        {
            return _deviceTask ?? Task.FromResult(true);
        }

        public void OnRefresh()
        {
            var view = _viewManager.CreateViewForModel(_waitingDialogViewModel);

            DialogHost.Show(view, "RefreshDeviceDialog",
                (object sender, DialogOpenedEventArgs args) =>
                {
                    _deviceTask = Task.Run(() =>
                    {
                        var devices = GetDevices();

                        Thread.Sleep(500);

                        Execute.PostToUIThreadAsync(() =>
                        {
                            Items = new ObservableCollection<SelectableDeviceViewModel>();

                            foreach (var device in devices)
                            {
                                Items.Add(new SelectableDeviceViewModel { Model = device.Model, Serial = device.Serial });
                                Console.WriteLine($@"{device.Model}({device.Serial})");
                            }

                            args.Session.Close(false);
                        });

                        return true;
                    });

                });

        }

        public void OnConfirm()
        {

        }

    }
}