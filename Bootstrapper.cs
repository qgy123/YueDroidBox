using System;
using Stylet;
using StyletIoC;
using YueDroidBox.Core;
using YueDroidBox.ViewModel;

namespace YueDroidBox
{
    public class Bootstrapper : Bootstrapper<ShellViewModel>
    {
        protected override void OnStart()
        {
            Stylet.Logging.LogManager.Enabled = true;
        }

        protected override void ConfigureIoC(IStyletIoCBuilder builder)
        {
            // Configure the IoC container in here
            base.ConfigureIoC(builder);
        }

        protected override void Configure()
        {
            // Perform any other configuration before the application starts
            IoC.GetInstance = this.Container.Get;
            IoC.GetAllInstances = this.Container.GetAll;
            IoC.BuildUp = this.Container.BuildUp;
        }
    }
}
