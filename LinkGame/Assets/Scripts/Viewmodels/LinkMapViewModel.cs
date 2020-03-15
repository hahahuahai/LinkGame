using Loxodon.Framework.Commands;
using Loxodon.Framework.Interactivity;
using Loxodon.Framework.Messaging;
using Loxodon.Framework.ViewModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LinkGame
{
    public class LinkMapViewModel : ViewModelBase
    {
        private int firstNumber = 0;
        private int secondNumber = 0;
        private readonly SimpleCommand<int> linkButtonClickCommand;
        private InteractionRequest<int> linkButtonClickInteractionRequest;
        
        

        public SimpleCommand<int> LinkButtonClickCommand
        {
            get
            {
                return this.linkButtonClickCommand;
            }
        }

        public InteractionRequest<int> LinkButtonClickInteractionRequest
        {
            get
            {
                return this.linkButtonClickInteractionRequest;
            }
        }

        public LinkMapViewModel(IMessenger messenger) : base(messenger)
        {
            this.linkButtonClickCommand = new SimpleCommand<int>(Click);
            this.linkButtonClickInteractionRequest = new InteractionRequest<int>(this);
        }

        /// <summary>
        /// 点击方块的时间
        /// </summary>
        /// <param name="number"></param>
        private void Click(int number)
        {
            if (firstNumber == 0)
            {
                firstNumber = number;
                Debug.Log("firstNumber:" + firstNumber);
            }
            else if (secondNumber == 0)
            {                
                secondNumber = number;
                Debug.Log("secondNumber:" + secondNumber);
            }
            if (firstNumber == secondNumber && firstNumber != 0 && secondNumber != 0)
            {
                Debug.Log("满足消除条件");
                linkButtonClickInteractionRequest.Raise(secondNumber);
                firstNumber = secondNumber = 0;
            }
            else if (firstNumber != 0 && secondNumber != 0)
            {
                Debug.Log("不满足消除条件");
                firstNumber = secondNumber = 0;
            }
        }
    }

}
