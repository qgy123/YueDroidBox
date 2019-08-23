using System.Windows;
using Dragablz;
using Stylet;
using YueDroidBox.View;
using YueDroidBox.ViewModel;

namespace YueDroidBox.Util
{
    public class CustomTabClient : DefaultInterTabClient
    {
        //private readonly IViewManager _viewManager;

        //public CustomTabClient(IViewManager viewManager)
        //{
        //    _viewManager = viewManager;
        //}

        public override INewTabHost<Window> GetNewHost(IInterTabClient interTabClient, object partition, TabablzControl source)
        {
            var window = new NewSubWindow();
            window.TabablzControl.Items.Clear();
            return new NewTabHost<Window>(window, window.TabablzControl);


            //var vm = new SubWindowViewModel(_viewManager);
            //var view = _viewManager.CreateViewForModel(vm);

            //var window = view as SubWindowView;
            //return new NewTabHost<Window>(window, window.InitialTabablzControl);

        }

        public override TabEmptiedResponse TabEmptiedHandler(TabablzControl tabControl, Window window)
        {
            if (window is NewSubWindow)
                return TabEmptiedResponse.CloseWindowOrLayoutBranch;
            return TabEmptiedResponse.DoNothing;
        }
    }
}