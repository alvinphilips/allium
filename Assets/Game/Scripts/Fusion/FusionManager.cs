using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

public class FusionManager : MonoBehaviour, INetworkRunnerCallbacks
{
    public NetworkRunner _runner;

    //Bind this to UI Buttons
    public void CreateGame()
    {
        StartGame(GameMode.Host);
    }

    public void JoinGame()
    {
        StartGame(GameMode.Client);
    }

    public void LeaveGame()
    {
        if(_runner != null)
        {
            _runner.Shutdown(false);
            Destroy(_runner);
        }
    }

    private void OnGUI()
    {
        if (_runner == null)
        {
            if (GUI.Button(new Rect(0, 0, 200, 40), "Host"))
            {
                CreateGame();
            }
            if (GUI.Button(new Rect(0, 40, 200, 40), "Join"))
            {
                JoinGame();
            }
        }
        else
        {
            if (GUI.Button(new Rect(0, 0, 200, 40), "Leave"))
            {
                LeaveGame();
            }
        }
    }

    async void StartGame(GameMode mode)
    {
        // Create the Fusion runner and let it know that we will be providing user input
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        // Create the NetworkSceneInfo from the current scene
        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        //var sceneInfo = new NetworkSceneInfo();
        //if (scene.IsValid)
        //{
        //    sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        //}

        // Start or join (depends on gamemode) a session with a specific name
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "TestRoom",
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
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
        Debug.Log($"OnInput");
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
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log($"{player.PlayerId} left!");
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
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        Debug.Log($"");
    }
    #endregion  
}
