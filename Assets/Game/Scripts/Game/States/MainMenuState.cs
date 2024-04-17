using Game.Scripts.Patterns;
using Game.Scripts.UI;
using UnityEngine;

namespace Game.Scripts.Game.States
{
    public class MainMenuState : IState<GameManager>
    {
        public void OnStateEnter(GameManager state)
        {
            MenuManager.Instance.ShowMenu(MenuManager.Instance.mainMenu);
        }
    }
}
