using Game.Scripts.Patterns;
using UnityEngine;

namespace Game.Scripts.Game.States
{
    public class PauseMenuState : IState<GameManager>
    {
        public void OnStateEnter(GameManager state)
        {
            EventBus<GameStates>.Publish(GameStates.Paused);
        
        }

        public void OnUpdate(GameManager state)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameManager.Instance.ChangeState(new PlayState());
            }
        }

        public void OnStateExit(GameManager state)
        {
            EventBus<GameStates>.Publish(GameStates.Running);
        
        }
    }
}
