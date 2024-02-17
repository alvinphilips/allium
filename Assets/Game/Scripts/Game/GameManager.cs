using System;
using System.Collections.Generic;
using Game.Scripts.Game.States;
using Game.Scripts.Patterns;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Scripts.Game
{
    public class GameManager : Singleton<GameManager>
    {
        private DateTime _sessionStartTime;
        private DateTime _sessionEndTime;

        private readonly Stack<IState<GameManager>> _stateHistory = new Stack<IState<GameManager>>();
        private IState<GameManager> _currentState;
        
        private void Start()
        {
            _sessionStartTime = DateTime.Now;
            Debug.Log($"Game session started at {_sessionStartTime}.");

            ChangeState(new MainMenuState());
        }

        private void OnApplicationQuit()
        {
            _sessionEndTime = DateTime.Now;

            var sessionLength = _sessionEndTime.Subtract(_sessionStartTime);
            
            Debug.Log($"Game session ended at {_sessionEndTime}.");
            Debug.Log($"Game session lasted {sessionLength}.");
        }
        
        /// <summary>
        /// Switch to a new State, and optionally specify if it is meant to be a temporary state.
        /// </summary>
        /// <param name="newState">State to switch to</param>
        /// <param name="isTemporary">is the State meant to be temporary?</param>
        public void ChangeState(IState<GameManager> newState, bool isTemporary = false)
        {
            if (!isTemporary)
            {
                _currentState?.OnStateExit(this);
            }
            else if (_currentState != null)
            {
                _stateHistory.Push(_currentState);
            }

            _currentState = newState;
            _currentState.OnStateEnter(this);
        }

        private void Update()
        {
            _currentState?.OnUpdate(this);
        }

        /// <summary>
        /// Revert to a previous State, if one exists.
        /// </summary>
        public void RollbackState()
        {
            if (_stateHistory.Count == 0) return;
            _currentState?.OnStateExit(this);
            _currentState = _stateHistory.Pop();
            _currentState.OnStateResume(this);
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Next Scene"))
            {
                SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCount);
            }
        }
    }
}
