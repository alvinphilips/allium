using Game.Scripts.Patterns;
using Game.Scripts.UI;
using UnityEngine;

namespace Game.Scripts.Game.States
{
    public class OptionsMenuState : IState<GameManager>
    {
        public void OnStateEnter(GameManager state)
        {
            MenuManager.Instance.ShowMenu(MenuManager.Instance.optionsMenu);
            EventBus<Settings.OptionsMenuState>.Publish(Settings.OptionsMenuState.Show);
        }
        
        public void OnStateExit(GameManager state)
        {
            EventBus<Settings.OptionsMenuState>.Publish(Settings.OptionsMenuState.Hidden);
        }
    }
}
