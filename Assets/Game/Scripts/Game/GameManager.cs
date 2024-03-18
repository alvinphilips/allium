using System;
using System.Collections.Generic;
using System.IO;
using Fusion;
using Game.Scripts.Fusion;
using Game.Scripts.Game.States;
using Game.Scripts.Patterns;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Scripts.Game
{
    [System.Serializable]
    public class SaveData
    {
        public DateTime startTime;
        public DateTime endTime;
        public TimeSpan duration;

        public SaveData(DateTime sTime, DateTime eTime, TimeSpan dur)
        {
            startTime = sTime;
            endTime = eTime;
            duration = dur;
        }
    }

    [System.Serializable]
    public class SaveDataList
    {
        public List<SaveData> gameSessions = new List<SaveData>();
    }

    public class GameManager : Singleton<GameManager>
    {
        private DateTime _sessionStartTime;
        private DateTime _sessionEndTime;

        SaveDataList dataList;
        private string savePath;

        private readonly Stack<IState<GameManager>> _stateHistory = new Stack<IState<GameManager>>();
        private IState<GameManager> _currentState;

        public GameObject mainMenu;
        public GameObject pauseMenu;
        public GameObject optionsMenu;

        private void Start()
        {
            _sessionStartTime = DateTime.Now;
            Debug.Log($"Game session started at {_sessionStartTime}.");

            savePath = Path.Combine(Application.persistentDataPath, "SaveData.json");
            LoadData();

            ChangeState(new MainMenuState());
        }

        private void OnApplicationQuit()
        {
            _sessionEndTime = DateTime.Now;

            var sessionLength = _sessionEndTime.Subtract(_sessionStartTime);
            
            Debug.Log($"Game session ended at {_sessionEndTime}.");
            Debug.Log($"Game session lasted {sessionLength}.");

            dataList.gameSessions.Add(new SaveData(_sessionStartTime, _sessionEndTime, sessionLength));

            SerializeData();
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
            Debug.Log("Changing States!");
            _currentState = newState;
            _currentState.OnStateEnter(this);
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                ChangeState(new OptionsMenuState(), true);
            }
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

        private void LoadData()
        {
            if (File.Exists(savePath))
            {
                string json = File.ReadAllText(savePath);
                dataList = JsonUtility.FromJson<SaveDataList>(json);
            }
        }

        private void SerializeData()
        {
            string json = JsonUtility.ToJson(dataList);
            File.WriteAllText(savePath, json);
        }

        #region Menu Functions

        //Bind this to UI Buttons
        public void CreateGame()
        {
            FusionManager.Instance.StartGame(GameMode.Host);
            ChangeState(new PlayState());
        }

        public void JoinGame()
        {
            FusionManager.Instance.StartGame(GameMode.Client);
            ChangeState(new PlayState());
        }

        public void LeaveGame()
        {
            if (FusionManager.Instance.runner != null)
            {
                FusionManager.Instance.runner.Shutdown();
            }
        }

        #endregion
    }
}
