using Loxodon.Framework.Commands;
using Loxodon.Framework.Contexts;
using Loxodon.Framework.Interactivity;
using Loxodon.Framework.Localizations;
using Loxodon.Framework.Messaging;
using Loxodon.Framework.Observables;
using Loxodon.Framework.ViewModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LinkGame
{
    public class StartViewModel : ViewModelBase
    {
        private SimpleCommand startCommand;//响应开始游戏按钮
        private SimpleCommand cancelCommand;//响应取消游戏按钮
        private Localization localization;
        private ObservableDictionary<string, string> localizationDictionary = new ObservableDictionary<string, string>();

        private InteractionRequest startInteractionRequest;

        public ICommand StartCommand
        {
            get { return this.startCommand; }
        }

        public ICommand CancelCommand
        {
            get { return this.cancelCommand; }
        }

        public ObservableDictionary<string,string> LocalizationDictionary
        {
            get
            {
                return this.localizationDictionary;
            }
        }

        public InteractionRequest StartInteractionRequest
        {
            get
            {
                return this.startInteractionRequest;
            }
        }

        public StartViewModel() : this(null)
        {

        }

        public StartViewModel(IMessenger messenger) : base(messenger)
        {
            ApplicationContext context = Context.GetApplicationContext();
            this.localization = context.GetService<Localization>();
            startInteractionRequest = new InteractionRequest(this);
            this.startCommand = new SimpleCommand(() =>
            {
                this.startCommand.Enabled = false;
                startInteractionRequest.Raise();
                this.startCommand.Enabled = true;
            });

            this.cancelCommand = new SimpleCommand(() =>
            {
                this.cancelCommand.Enabled = false;
            });
        }



    }

}
