using Loxodon.Framework.Views;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LinkGame
{
    public class GameWindow : Window
    {
        private GameViewModel gameViewModel;
        protected override void OnCreate(IBundle bundle)
        {
            gameViewModel = new GameViewModel();
        }
    }

}
