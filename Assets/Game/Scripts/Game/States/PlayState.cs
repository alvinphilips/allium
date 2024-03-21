using Game.Scripts.Patterns;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Scripts.Game.States
{
    public class PlayState : IState<GameManager>
    {
        public void OnStateEnter(GameManager state)
        {
            EventBus<GameStates>.Publish(GameStates.Running);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    
        public void OnUpdate(GameManager state) 
        {
            // if(Input.GetKeyDown(KeyCode.Escape))
            // {
            //     GameManager.Instance.ChangeState(new PauseMenuState());
            // }
        }
    
        public void OnStateExit(GameManager state)
        {
            EventBus<GameStates>.Publish(GameStates.Paused);
        }
    }
}
