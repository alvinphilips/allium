using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Game.Scripts.Game;
using Game.Scripts.Game.States;
using Game.Scripts.Patterns;
using Fusion;
using Fusion.Sockets;

namespace Game.Scripts.Fusion
{
    public class FusionManager : Singleton<FusionManager>, INetworkRunnerCallbacks
    {
        public NetworkRunner runner;

        [SerializeField] private NetworkPrefabRef _playerPrefab;
        private readonly Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

        public async void StartGame(GameMode mode)
        {
            // Create the Fusion runner and let it know that we will be providing user input
            if (runner == null && (runner = GetComponent<NetworkRunner>()) == null)
            {
                runner = gameObject.AddComponent<NetworkRunner>();
            }
            runner.ProvideInput = true;

            // Create the NetworkSceneInfo from the current scene
            var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
            //var sceneInfo = new NetworkSceneInfo();
            //if (scene.IsValid)
            //{
            //    sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
            //}

            // Start or join (depends on gamemode) a session with a specific name
            await runner.StartGame(new StartGameArgs()
            {
                GameMode = mode,
                SessionName = "TestRoom",
                //Scene = scene,
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
            });

        }

        private void HandleShutdown()
        {
            SceneManager.LoadScene(0);
            GameManager.Instance.ChangeState(new MainMenuState());
        }


        #region Fusion Callbacks Interface Implementation
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

        public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {
            Debug.Log($"");
        }

        public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {
            Debug.Log($"");
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            Debug.Log($"{player.PlayerId} joined!");

            if (this.runner.IsServer)
            {
                // Create a unique position for the player
                Vector3 spawnPosition = new Vector3((player.RawEncoded % runner.Config.Simulation.PlayerCount) * 3, 1, 0);
                NetworkObject networkPlayerObject = this.runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
                // Keep track of the player avatars for easy access
                _spawnedCharacters.Add(player, networkPlayerObject);
            }
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

        public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
        {
            Debug.Log($"");
        }

        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
        {
            Debug.Log($"");
        }

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
            Debug.Log($"");
        }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
            Debug.Log($"{shutdownReason.ToString()}");
            HandleShutdown();
        }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
        {
            Debug.Log($"");
        }
        #endregion  
    }
}
