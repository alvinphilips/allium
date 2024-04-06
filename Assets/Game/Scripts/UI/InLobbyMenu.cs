using System.Collections;
using System.Linq;
using Fusion;
using Game.Scripts.Fusion;
using Game.Scripts.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Game.Scripts.UI
{
    public class InLobbyMenu : Menu
    {
        [SerializeField] private StyleSheet style;
        [SerializeField] private int gameSceneIndex;
        
        public string LobbyName { get; set; }

        private enum LastHadFocus
        {
            Default,
            LobbyName
        }

        private LastHadFocus _lastHadFocus = LastHadFocus.Default;
        
        private new void Start()
        {
            base.Start();
            FusionManager.Instance.onPlayerCountChanged.AddListener(() => RefreshUI = true);
            FusionManager.Instance.onPlayerLeft.AddListener(() => MenuManager.Instance.ShowMenu(MenuManager.Instance.lobbyListMenu));
        }
        
        protected override IEnumerator Generate()
        {
            yield return null;
            
            var root = Document.rootVisualElement;
            root.Clear();
            root.styleSheets.Add(style);

            var container = root.Create("w-full", "h-full", "justify-between", "bg-emerald-600");

            var playerList = container.Create<ScrollView>("w-full");
            
            var topBar = playerList.Create("bg-emerald-900", "p-4", "text-white", "flex-row", "items-center");
            topBar.Create<Label>().text = "Lobby Name";
            var lobbyName = topBar.Create<TextField>("text-emerald-900", "px-4", "w-60");
            lobbyName.value = LobbyName;
            lobbyName.selectAllOnFocus = false;
            if (_lastHadFocus == LastHadFocus.LobbyName)
            {
                lobbyName.Focus();
                lobbyName.cursorIndex = lobbyName.value.Length;
            }

            lobbyName.selectIndex = lobbyName.text.Length;
            
            lobbyName.RegisterCallback<ChangeEvent<string>>(evt =>
            {
                LobbyName = evt.newValue;
                _lastHadFocus = LastHadFocus.LobbyName;
                RefreshUI = true;
            });
            
            if (!Application.isPlaying) yield break;

            var players = FusionManager.Instance.Runner.ActivePlayers.ToList();
            foreach (var player in players)
            {
                CreatePlayerListItem(playerList, player);
            }

            var bottomBar = container.Create("w-full", "justify-between", "bg-white", "flex-row", "items-center");

            var playerCount = FusionManager.Instance.Runner.SessionInfo.PlayerCount;
            
            var foundLobbiesText = bottomBar.Create<Label>("px-4");
            foundLobbiesText.text = $"{playerCount} Players";
            var startGame = bottomBar.Create<Button>("bg-emerald-900", "text-white", "p-4");
            startGame.text = "Start Game";
            startGame.SetEnabled(FusionManager.Instance.IsHost && playerCount > 1);
            startGame.RegisterCallback<ClickEvent>(evt =>
            {
                FusionManager.Instance.ChangeScene(gameSceneIndex, LoadSceneMode.Single);
                Hide();
            });
        }

        private VisualElement CreatePlayerListItem(VisualElement parent, PlayerRef player)
        {
            var nickname = $"Player {player.PlayerId}";
            if (FusionManager.Instance.Runner.TryGetPlayerObject(player, out var playerObject))
            {
                if (playerObject.TryGetComponent<Player>(out var playerData))
                {
                    nickname = playerData.Nickname;
                }
            }
            
            var item = parent.Create("w-full", "justify-between", "items-center", "border-b-2", "text-white", "flex-row", "border-emerald-900", "bg-emerald-700");
            var label = item.Create<Label>("flex-grow", "justify-center", "p-4", "text-lg", "font-bold");
            label.text = nickname;

            if (!FusionManager.Instance.IsHost) return item;
            
            var kickButton = item.Create<Button>( "p-4", "bg-emerald-800", "text-white", "joinButton", "h-full");
            kickButton.text = "Kick Player";
            
            // We know that the Player is the host here
            var enableKickButton = player.PlayerId != FusionManager.Instance.PlayerId;
            
            kickButton.SetEnabled(enableKickButton);
            if (!enableKickButton)
            {
                kickButton.RemoveFromClassList("joinButton");
            }
            kickButton.RegisterCallback<ClickEvent>(_ =>
            {
                FusionManager.Instance.Runner.Disconnect(player);
                RefreshUI = true;
            });
            
            return item;
        }
    }
}