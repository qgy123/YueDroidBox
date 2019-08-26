using Stylet;

namespace YueDroidBox.ViewModel
{
    public class SingleSelectDeviceViewModel : DeviceViewModel
    {
        public SingleSelectDeviceViewModel(IViewManager viewManager, WaitingDialogViewModel waitingDialogViewModel) : base(viewManager, waitingDialogViewModel)
        {
            this.DisplayName = "select device";

            ToggleSingleSelectionMode();

        }
    }
}