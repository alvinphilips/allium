using Game.Scripts.Game;
using Game.Scripts.Patterns;
using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Game.States;
using UnityEditor;
using UnityEngine;

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
}
