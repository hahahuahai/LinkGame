using Loxodon.Framework.Commands;
using Loxodon.Framework.Contexts;
using Loxodon.Framework.Localizations;
using Loxodon.Framework.Messaging;
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

        public StartViewModel() : this(null)
        {

        }

        public StartViewModel(IMessenger messenger) : base(messenger)
        {
            ApplicationContext context = Context.GetApplicationContext();
            this.localization = context.GetService<Localization>();

            this.startCommand = new SimpleCommand(() =>
            {
                this.startCommand.Enabled = false;
            });

            this.cancelCommand = new SimpleCommand(() =>
            {
                this.cancelCommand.Enabled = false;
            });
        }

        public ICommand StartCommand
        {
            get { return this.startCommand; }
        }

        public ICommand CancelCommand
        {
            get { return this.cancelCommand; }
        }
    }

}
