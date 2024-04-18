using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Fusion;
using Game.Scripts.Audio;
using Game.Scripts.Fusion;
using Game.Scripts.Game.States;
using Game.Scripts.Patterns;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.XR.ARFoundation;

namespace Game.Scripts.Game
{
    [Serializable]
    public class SaveData
    {
        public DateTime StartTime;
        public DateTime EndTime;
        public TimeSpan Duration;

        public SaveData(DateTime sTime, DateTime eTime, TimeSpan dur)
        {
            StartTime = sTime;
            EndTime = eTime;
            Duration = dur;
        }
    }

    [Serializable]
    public class SaveDataList
    {
        public List<SaveData> gameSessions = new List<SaveData>();
    }

    public class GameManager : NetworkSingleton<GameManager>
    {
        private DateTime _sessionStartTime;
        private DateTime _sessionEndTime;

        private SaveDataList _dataList;
        private string _savePath;

        private readonly Stack<IState<GameManager>> _stateHistory = new Stack<IState<GameManager>>();
        private IState<GameManager> _currentState;

        [SerializeField] private Camera nonARCamera;
        [SerializeField] private AssetReference backgroundMusic;
        [SerializeField] private ARSession arSession;
        [SerializeField] public ARRaycastManager arRaycastManager;
        [SerializeField] public ARPlaneManager arPlaneManager;
        [SerializeField] private List<NetworkObject> units = new();

        private List<NetworkObject> spawnedUnits = new();

        public List<NetworkObject> Units => units;

        public NetworkObject CurrentUnit => units[_currentUnitIndex];
        public int SelectedUnitIndex => _currentUnitIndex;

        private int _currentUnitIndex = 0;

        private AudioObject _bgm;

        private bool _muteAudio;

        public bool IsAREnabled { get; private set; }

        private IEnumerator SetupAR()
        {
            arSession.gameObject.SetActive(false);
            if (Application.platform != RuntimePlatform.IPhonePlayer)
            {
                yield break;
            }
            yield return ARSession.CheckAvailability();

            if (ARSession.state == ARSessionState.Ready)
            {
                IsAREnabled = true;
                arSession.gameObject.SetActive(true);
                nonARCamera.gameObject.SetActive(false);
            }
        }

        private void Start()
        {
            _sessionStartTime = DateTime.Now;
            Debug.Log($"Game session started at {_sessionStartTime}.");

            _savePath = Path.Combine(Application.persistentDataPath, "SaveData.json");
            LoadData();

            StartCoroutine(SetupAR());
            
            var handle = Addressables.LoadAssetAsync<AudioObject>(backgroundMusic.AssetGUID);
            
            handle.Completed += (op) =>
            {
                if (op.Status == AsyncOperationStatus.Succeeded)
                {
                    Debug.Log("Playing music");
                    AudioManager.Instance.LoadMusic(op.Result);
                    AudioManager.Instance.PlayMusic(op.Result.Id);

                    _bgm = op.Result;
                }
                else
                {
                    Debug.LogError("Failed to load main menu music");
                }
            };

            ChangeState(new MainMenuState());
        }

        private void OnApplicationQuit()
        {
            _sessionEndTime = DateTime.Now;

            var sessionLength = _sessionEndTime.Subtract(_sessionStartTime);
            
            Debug.Log($"Game session ended at {_sessionEndTime}.");
            Debug.Log($"Game session lasted {sessionLength}.");

            _dataList.gameSessions.Add(new SaveData(_sessionStartTime, _sessionEndTime, sessionLength));

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
            if (Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.P))
            {
                ChangeState(new OptionsMenuState(), true);
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                _muteAudio = !_muteAudio;
                AudioManager.Instance.MuteAll(_muteAudio);
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

        private void LoadData()
        {
            if (File.Exists(_savePath))
            {
                string json = File.ReadAllText(_savePath);
                _dataList = JsonUtility.FromJson<SaveDataList>(json);
            }
        }

        public void SpawnItem(Vector3 position, Quaternion rotation)
        {
            if (FusionManager.Instance.IsHost)
            {
                FusionManager.Instance.Runner.Spawn(CurrentUnit, position, rotation);
            }
            else
            {
                RpcSpawnItem(CurrentUnit, position, rotation);
            }
        }
        
        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void RpcSpawnItem(NetworkObject no, Vector3 position, Quaternion rotation)
        {
            Runner.Spawn(no, position, rotation);
        }

        public void SetSelectedUnitIndex(int index)
        {
            _currentUnitIndex = index;
        }

        private void SerializeData()
        {
            string json = JsonUtility.ToJson(_dataList);
            File.WriteAllText(_savePath, json);
        }
    }
}
