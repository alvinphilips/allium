using Game.Scripts.Patterns;
using UnityEngine;

namespace Game.Scripts.Game.States
{
    public class MainMenuState : IState<GameManager>
    {
        public void OnStateEnter(GameManager state)
        {
            GameManager.Instance.mainMenu.SetActive(true);
        }
        
        public void OnStateExit(GameManager state)
        {
            GameManager.Instance.mainMenu.SetActive(false);
        }
        
        public void OnStateResume(GameManager state)
        {
            throw new System.NotImplementedException();
        }
    }
}
