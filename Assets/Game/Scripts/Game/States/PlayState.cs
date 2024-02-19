using Game.Scripts.Patterns;
using UnityEngine;

namespace Game.Scripts.Game.States
{
    public class PlayState : IState<GameManager>
    {
        public void OnStateEnter(GameManager state)
        {
            EventBus<GameStates>.Publish(GameStates.Running);
        }
    
        public void OnUpdate(GameManager state) 
        {
            Debug.Log("PlayStateUpdating");
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                GameManager.Instance.ChangeState(new PauseMenuState());
            }
        }
    
        public void OnStateExit(GameManager state)
        {
            EventBus<GameStates>.Publish(GameStates.Paused);
        }
    }
}
