using Stylet;

namespace YueDroidBox.ViewModel.Dialog
{
    public class AddPFDialogViewModel : Screen
    {
        public int Local { get; set; }
        public int Remote { get; set; }
        public AddPFDialogViewModel()
        {
            this.DisplayName = "Create PortForwarding Dialog";
        }
    }
}