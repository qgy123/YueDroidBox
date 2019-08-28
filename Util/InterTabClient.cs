using System.Windows;
using Dragablz;
using Stylet;
using YueDroidBox.Core;
using YueDroidBox.View;
using YueDroidBox.ViewModel;

namespace YueDroidBox.Util
{
    public class InterTabClient : IInterTabClient
    {
        public INewTabHost<Window> GetNewHost(IInterTabClient interTabClient, object partition, TabablzControl source)
        {
            var shell = new ShellView();
            shell.InitializeComponent(); // Only required if you don't have ShellView.xaml.cs
            var vm = IoC.Get<ShellViewModel>();
            var viewModelBinder = IoC.Get<ViewManager>();
            viewModelBinder.BindViewToModel(shell, vm);
            return new NewTabHost<Window>(shell, shell.MainTab);
        }

        public TabEmptiedResponse TabEmptiedHandler(TabablzControl tabControl, Window window)
        {
            return TabEmptiedResponse.CloseWindowOrLayoutBranch;
        }
    }
}