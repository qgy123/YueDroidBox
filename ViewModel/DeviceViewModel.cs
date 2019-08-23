using System.Collections.Generic;
using System.Collections.ObjectModel;
using Stylet;

namespace YueDroidBox.ViewModel
{
    public class DeviceViewModel : Screen
    {
        public ObservableCollection<SelectableDeviceViewModel> Items { get; set; }

        public DeviceViewModel(ObservableCollection<SelectableDeviceViewModel> items)
        {
            this.DisplayName = "devices";
            Items = items;
        }

        public void OnSelectAll()
        {
            SelectAll(true, Items);
        }

        public void OnConfirm()
        {

        }

        public static void SelectAll(bool select, IEnumerable<SelectableDeviceViewModel> models)
        {
            foreach (var model in models)
            {
                model.IsSelected = select;
            }
        }
    }
}