using Loxodon.Framework.Binding;
using Loxodon.Framework.Binding.Builder;
using Loxodon.Framework.Views;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LinkGame
{
    public class StartWindow : Window
    {
        public Text gameNameText;
        public Button startButton;
        public Button quitButton;

        private StartViewModel startViewModel;

        protected override void OnCreate(IBundle bundle)
        {
            this.startViewModel = new StartViewModel();

            BindingSet<StartWindow, StartViewModel> bindingSet = this.CreateBindingSet(startViewModel);
            bindingSet.Bind(this.startButton).For(v => v.onClick).To(vm => vm.StartCommand).OneWay();
            bindingSet.Bind(this.quitButton).For(v => v.onClick).To(vm => vm.CancelCommand).OneWay();

            bindingSet.Build();
        }
    }

}
