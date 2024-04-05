using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Game.Scripts.Patterns;
using Fusion;
using Fusion.Sockets;
using UnityEngine.Events;

namespace Game.Scripts.Fusion
{
    public class FusionManager : Singleton<FusionManager>, INetworkRunnerCallbacks
    {
        public NetworkRunner Runner { get; private set;  }

        [SerializeField] private NetworkPrefabRef playerPrefab;
        private readonly Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

        public UnityEvent onSessionListUpdatedCallbacks;
        
        public List<SessionInfo> SessionList { get; private set; } = new();

        private new void Awake()
        {
            base.Awake();
            
            if (TryGetComponent<NetworkRunner>(out var runner))
            {
                runner.Shutdown();
                Destroy(runner);
            }

            Runner = gameObject.AddComponent<NetworkRunner>();
        }

        private void Start()
        {
            if (Runner == null) Runner = gameObject.AddComponent<NetworkRunner>();
            JoinLobby();
        }

        private async void JoinLobby()
        {
            var result = await Runner.JoinSessionLobby(SessionLobby.ClientServer);

            if (!result.Ok)
            {
                Debug.LogError($"Could not join session lobby {result.ShutdownReason}");
            }
            else
            {
                Debug.Log("Joined session lobby :)");
            }
        }
        
        public async Task CreateSession(string sessionName)
        {
            var result = await Runner.StartGame(new StartGameArgs
            {
                GameMode = GameMode.AutoHostOrClient,
                SessionName = sessionName,
                PlayerCount = 2
            });

            if (!result.Ok)
            {
                Debug.LogError($"Failed to Start Lobby: {result.ShutdownReason}");
                return;
            }
            
            SessionList.Add(Runner.SessionInfo);
        }
        
        public async void StartGame(GameMode mode)
        {
            // Destroy the previous NetworkRunner, if one exists
            if (TryGetComponent<NetworkRunner>(out var runner))
            {
                Destroy(runner);
            }

            Runner = gameObject.AddComponent<NetworkRunner>();
            Runner.ProvideInput = true;

            // Create the NetworkSceneInfo from the current scene
            var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
            //var sceneInfo = new NetworkSceneInfo();
            //if (scene.IsValid)
            //{
            //    sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
            //}

            // Start or join (depends on gamemode) a session with a specific name
            await Runner.StartGame(new StartGameArgs()
            {
                GameMode = mode,
                //Scene = scene,
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
            });

        }
        
        private void HandleShutdown()
        {
            Runner.Shutdown();
            _spawnedCharacters.Clear();
            Destroy(Runner);
            // SceneManager.LoadScene(0);
            // GameManager.Instance.ChangeState(new MainMenuState());
        }


        #region Fusion Callbacks Interface Implementation
        // ReSharper disable once Unity.IncorrectMethodSignature
        public void OnConnectedToServer(NetworkRunner runner)
        {
            Debug.Log($"{runner.name} connected to server!");
        }

        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
        {
            Debug.Log($"OnConnectFailed due to {reason.ToString()}");
        }

        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
        {
            Debug.Log($"{runner.LocalPlayer.PlayerId} Requested Connection");
        }

        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
        {
            Debug.Log($"");
        }

        public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
        {
            Debug.Log($"{runner.name} disconnected from server!");
        }

        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {
            Debug.Log($"");
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            // Debug.Log($"OnInput");
        }

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
        {
            Debug.Log($"{player.PlayerId} OnInputMissing");
        }

        public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }

        public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            Debug.Log($"{player.PlayerId} joined!");

            if (!Runner.IsServer) return;
            return;
            // Create a unique position for the player
            var spawnPosition = new Vector3((player.RawEncoded % runner.Config.Simulation.PlayerCount) * 3, 1, 0);
            var networkPlayerObject = Runner.Spawn(playerPrefab, spawnPosition, Quaternion.identity, player);
            // Keep track of the player avatars for easy access
            _spawnedCharacters.Add(player, networkPlayerObject);
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            Debug.Log($"{player.PlayerId} left!");

            if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
            {
                runner.Despawn(networkObject);
                _spawnedCharacters.Remove(player);
            }
        }

        public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }

        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }

        public void OnSceneLoadDone(NetworkRunner runner)
        {
            Debug.Log($"OnSceneLoadDone");
        }

        public void OnSceneLoadStart(NetworkRunner runner)
        {
            Debug.Log($"OnSceneLoadStart");
        }

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
            Debug.LogError("UWUW!");
            SessionList.Clear();
            SessionList = sessionList;
            onSessionListUpdatedCallbacks?.Invoke();
            Debug.Log($"Total session count: {sessionList.Count}");
        }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
            Debug.Log($"{shutdownReason.ToString()}");
            HandleShutdown();
        }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
        #endregion  
    }
}
