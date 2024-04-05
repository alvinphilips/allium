using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Game.Scripts.Game;
using Game.Scripts.Game.States;
using Game.Scripts.Patterns;
using Fusion;
using Fusion.Sockets;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Game.Scripts.Fusion
{
    public class FusionManager : Singleton<FusionManager>, INetworkRunnerCallbacks
    {
        public NetworkRunner Runner { get; private set;  }

        [SerializeField] private NetworkPrefabRef playerPrefab;
        private readonly Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

        public readonly UnityAction<List<SessionInfo>> OnSessionListChange = default;
        public List<SessionInfo> SessionList { get; private set; } = new();

        private void Start()
        {
            if (TryGetComponent<NetworkRunner>(out var runner))
            {
                Destroy(runner);
            }

            Runner = gameObject.AddComponent<NetworkRunner>();
        }
        
        public async Task HostLobby(string lobbyName)
        {
            var result = await Runner.StartGame(new StartGameArgs
            {
                GameMode = GameMode.Host,
                CustomLobbyName = lobbyName,
                SessionName = lobbyName,
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

        private async Task JoinLobby(NetworkRunner runner, string lobbyId)
        {
            var result = await runner.JoinSessionLobby(SessionLobby.Custom, lobbyId);

            if (!result.Ok)
            {
                Debug.LogError($"Failed to Join Lobby: {result.ShutdownReason}");
            }
        }
        
        private void HandleShutdown()
        {
            Runner.Shutdown();
            _spawnedCharacters.Clear();
            Destroy(Runner);
            SceneManager.LoadScene(0);
            GameManager.Instance.ChangeState(new MainMenuState());
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
            OnSessionListChange.Invoke(sessionList);
            SessionList.Clear();
            SessionList = sessionList;
            Debug.Log($"New Session Created. Total session count: {SessionList.Count}");
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
