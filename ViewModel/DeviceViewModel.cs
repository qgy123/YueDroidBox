using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        // public event PropertyChangedEventHandler PropertyChanged;
        // Therefore you do not need to do anything special in order to use Fody.PropertyChanged with any subclass of Screen,      ValidatingModelBase, or PropertyChangedBase.

        private readonly IViewManager _viewManager;
        private readonly WaitingDialogViewModel _waitingDialogViewModel;
        private readonly List<DeviceData> _selectedDevices;

        public ObservableCollection<SelectableDeviceViewModel> Items { get; set; }
        private bool _singleSelection = false;

        public bool CurVisibility { get; set; } = true;
        public int ConfirmHeight { get; set; } = 35;
        public int CurRow { get; set; } = 1;

        private Task<bool> _deviceTask;

        private bool _selectAll = false;

        private List<DeviceData> _selectedDevice = new List<DeviceData>();

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
            this.DisplayName = "select device";
            _singleSelection = true;
            CurVisibility = false;
            ConfirmHeight = 0;
            CurRow = 0;
        }

        protected override void OnViewLoaded()
        {
            Execute.PostToUIThreadAsync(OnRefresh);
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
            _selectAll = !_selectAll;
            SelectAll(_selectAll, Items);
        }

        public List<DeviceData> GetDevices()
        {
            return Engine.Instance().GetDevices();

        }

        public void OnClick(DeviceData deviceData)
        {
            if (!_singleSelection) return;

            if (Items == null) return;

            _selectedDevice.Clear();
            _selectedDevice.Add(deviceData);

            this.RequestClose();
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
                                Items.Add(new SelectableDeviceViewModel { DeviceData = device });
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
            if (Items == null) return;

            _selectedDevice.Clear();

            foreach (var model in Items)
            {
                if (model.IsSelected)
                {
                    _selectedDevice.Add(model.DeviceData);
                }
            }

            this.RequestClose();

        }

        public List<DeviceData> GetSelectedDevice()
        {
            return _selectedDevice;
        }
    }
}