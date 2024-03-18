using Game.Scripts.Patterns;
using UnityEngine;

namespace Game.Scripts.Game.States
{
    public class OptionsMenuState : IState<GameManager>
    {
        public void OnStateEnter(GameManager state)
        {
            GameManager.Instance.optionsMenu.SetActive(true);
            EventBus<Settings.OptionsMenuState>.Publish(Settings.OptionsMenuState.Show);
        }
        
        public void OnStateExit(GameManager state)
        {
            EventBus<Settings.OptionsMenuState>.Publish(Settings.OptionsMenuState.Hidden);
            GameManager.Instance.optionsMenu.SetActive(false);
        }
    }
}
