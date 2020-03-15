using Loxodon.Framework.Binding;
using Loxodon.Framework.Binding.Builder;
using Loxodon.Framework.Contexts;
using Loxodon.Framework.Interactivity;
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

        private IUIViewLocator viewLocator;

        protected override void OnCreate(IBundle bundle)
        {
            this.viewLocator = Context.GetApplicationContext().GetService<IUIViewLocator>();
            this.startViewModel = new StartViewModel();

            BindingSet<StartWindow, StartViewModel> bindingSet = this.CreateBindingSet(startViewModel);
            bindingSet.Bind(this.startButton).For(v => v.onClick).To(vm => vm.StartCommand).OneWay();
            bindingSet.Bind(this.quitButton).For(v => v.onClick).To(vm => vm.CancelCommand).OneWay();
            bindingSet.Bind().For(v => v.StartGame).To(vm => vm.StartInteractionRequest);

            bindingSet.Build();
        }

        protected void StartGame(object sender, InteractionEventArgs args)
        {
            //打开GameWindow
            GameWindow gameWindow = viewLocator.LoadWindow<GameWindow>(this.WindowManager, "UI/GameWindow");
            gameWindow.Create();
            gameWindow.Show();
        }
    }

}
