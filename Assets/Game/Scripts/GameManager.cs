using System;
using Game.Scripts.Patterns;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Scripts
{
    public class GameManager : Singleton<GameManager>
    {
        private DateTime _sessionStartTime;
        private DateTime _sessionEndTime;

        private void Start()
        {
            _sessionStartTime = DateTime.Now;
            Debug.Log($"Game session started at {_sessionStartTime}.");
        }

        private void OnApplicationQuit()
        {
            _sessionEndTime = DateTime.Now;

            var sessionLength = _sessionEndTime.Subtract(_sessionStartTime);
            
            Debug.Log($"Game session ended at {_sessionEndTime}.");
            Debug.Log($"Game session lasted {sessionLength}.");
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Next Scene"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }
}
