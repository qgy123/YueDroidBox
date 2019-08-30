using YueDroidBox.ViewModel;
using YueDroidBox.ViewModel.Control;

namespace YueDroidBox.Core.Factory
{
    public interface IViewModelFactory
    {
        DeviceViewModel CreateDeviceViewModel();
        PortForwardingViewModel CreatePortForwardingViewModel();
        CmdControlViewModel CreateCmdControlViewModel();
    }
}