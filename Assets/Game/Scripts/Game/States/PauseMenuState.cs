using Game.Scripts.Game;
using Game.Scripts.Patterns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuState : IState<GameManager>
{
    public void OnStateEnter(GameManager state)
    {
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
        GameManager.Instance.pauseMenu.SetActive(false);
    }
}
