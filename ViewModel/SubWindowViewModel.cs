using Dragablz;
using Stylet;
using YueDroidBox.Util;

namespace YueDroidBox.ViewModel
{
    public class SubWindowViewModel : Screen
    {
        private readonly IViewManager _viewManager;
        public static IInterTabClient InterTabClient { get; set; }

        public SubWindowViewModel(IViewManager viewManager)
        {
            _viewManager = viewManager;
            //InterTabClient = new CustomTabClient(_viewManager);
            InterTabClient = new CustomTabClient();
        }
    }
}