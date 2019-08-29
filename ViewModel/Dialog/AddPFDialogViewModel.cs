using System;
using System.Collections.Generic;
using System.Windows.Navigation;
using FluentValidation;
using MaterialDesignThemes.Wpf;
using Stylet;
using YueDroidBox.Util;

namespace YueDroidBox.ViewModel.Dialog
{
    public class AddPFDialogViewModel : Screen
    {
        private readonly IWindowManager _windowManager;
        //public int? Local { get; set; }
        //public int? Remote { get; set; }
        public Stringable<int> Local { get; set; }
        public Stringable<int> Remote { get; set; }


        public AddPFDialogViewModel(IWindowManager windowManager, IModelValidator<AddPFDialogViewModel> validator) : base(validator)
        {
            _windowManager = windowManager;
            this.Validate();
            this.DisplayName = "Create PortForwarding Dialog";
        }

        protected override void OnValidationStateChanged(IEnumerable<string> changedProperties)
        {
            base.OnValidationStateChanged(changedProperties);
            // Fody can't weave other assemblies, so we have to manually raise this
            this.NotifyOfPropertyChange(() => this.CanCloseDialogCommand);
        }

        public void Accept() { _ = DialogHost.CloseDialogCommand; }

        public bool CanCloseDialogCommand
        {
            get { return !this.HasErrors; }
        }

        public class AddPFDialogViewModelValidator : AbstractValidator<AddPFDialogViewModel>
        {
            public AddPFDialogViewModelValidator()
            {
                RuleFor(x => x.Local).Must(x => x.IsValid).WithMessage("{PropertyName} must be a valid number");
                When(x => x.Local.IsValid, () =>
                {
                    RuleFor(x => x.Local.Value).GreaterThan(0).WithName("Port").LessThan(65536).WithName("Port");
                });
                
                RuleFor(x => x.Remote).Must(x => x.IsValid).WithMessage("{PropertyName} must be a valid number");
                When(x => x.Local.IsValid, () =>
                {
                    RuleFor(x => x.Remote.Value).GreaterThan(0).WithName("Port").LessThan(65536).WithName("Port");
                });

            }
        }

    }

}