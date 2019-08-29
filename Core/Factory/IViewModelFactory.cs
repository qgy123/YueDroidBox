using YueDroidBox.ViewModel;

namespace YueDroidBox.Core.Factory
{
    public interface IViewModelFactory
    {
        DeviceViewModel CreateDeviceViewModel();
        PortForwardingViewModel CreatePortForwardingViewModel();
    }
}