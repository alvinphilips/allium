using Game.Scripts.Game;
using Game.Scripts.Patterns;
using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Game.States;
using UnityEngine;

public class PauseMenuState : IState<GameManager>
{
    public void OnStateEnter(GameManager state)
    {
        EventBus<GameStates>.Publish(GameStates.Paused);
        
        GameManager.Instance.pauseMenu.SetActive(true);
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
        
        GameManager.Instance.pauseMenu.SetActive(false);
    }
}
