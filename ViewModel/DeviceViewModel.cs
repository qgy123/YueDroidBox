using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SharpAdbClient;
using Stylet;
using YueDroidBox.Core;

namespace YueDroidBox.ViewModel
{
    public class DeviceViewModel : Screen
    {
        private readonly List<DeviceData> _selectedDevices;
        public ObservableCollection<SelectableDeviceViewModel> Items { get; set; }
        public bool CurVisibility { get; set; } = true;
        public int ConfirmHeight { get; set; } = 35;
        public int CurRow { get; set; } = 1;

        public DeviceViewModel(ref List<DeviceData> selectedDevices, bool singleSelectionMode = false)
        {
            _selectedDevices = selectedDevices;

            this.DisplayName = singleSelectionMode == false ? "select devices" : "select device";

            if (singleSelectionMode)
            {
                ToggleSingleSelectionMode();
            }
        }

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

        public void RefreshDevices()
        {
            var devices = Engine.Instance().GetDevices();

            Items = new ObservableCollection<SelectableDeviceViewModel>();

            foreach (var device in devices)
            {
                Items.Add(new SelectableDeviceViewModel { Model = device.Model, Serial = device.Serial });
                Console.WriteLine($@"{device.Model}({device.Serial})");
            }
        }

        public void OnClick(string argument)
        {
            Console.WriteLine($@"Argument is {argument}");
        }

        public void OnRefresh()
        {
            RefreshDevices();
        }
        
        public void OnConfirm()
        {

        }

    }
}