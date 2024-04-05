using System.Collections;
using Game.Scripts.Fusion;
using Game.Scripts.Utils;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Scripts.UI
{
    public class LobbyCreateMenu : Menu
    {
        [SerializeField] private StyleSheet style;

        private bool _hideFullGames = true;

        private void OnEnable()
        {
            FusionManager.Instance.onSessionListUpdatedCallbacks.AddListener(() => StartCoroutine(Generate()));
        }
        
        protected override IEnumerator Generate()
        {
            yield return null;
            
            var root = Document.rootVisualElement;
            root.Clear();
            root.styleSheets.Add(style);

            var container = root.Create("w-full", "h-full", "justify-between", "bg-emerald-600");

            var lobbyList = container.Create<ScrollView>("w-full");
            
            var topBar = lobbyList.Create("bg-emerald-800", "p-4", "text-white");
            var hideFullGamesToggle = topBar.Create<Toggle>();
            hideFullGamesToggle.value = _hideFullGames;
            hideFullGamesToggle.RegisterCallback<ChangeEvent<bool>>(evt =>
            {
                _hideFullGames = evt.newValue;
                RefreshUI = true;
            });
            hideFullGamesToggle.text = "Hide Full Games";
            
            var suitableLobbies = 0;
            if (!Application.isPlaying) yield break;
            
            foreach (var lobby in FusionManager.Instance.SessionList) {
                CreateLobbyListItem(lobbyList, lobby.Name, lobby.PlayerCount, lobby.MaxPlayers);
                suitableLobbies++;
            }

            var bottomBar = container.Create("w-full", "justify-between", "bg-white", "flex-row", "items-center");
            var foundLobbiesText = bottomBar.Create<Label>("px-4");
            foundLobbiesText.text =
                $"Found {suitableLobbies}/{FusionManager.Instance.SessionList.Count} lobbies that meet search criteria.";
            var createLobbyButton = bottomBar.Create<Button>("bg-emerald-800", "text-white", "p-4");
            createLobbyButton.text = "Host Lobby";
            createLobbyButton.RegisterCallback<ClickEvent>(async evt =>
            {
                await FusionManager.Instance.CreateSession("uwu-" + Random.Range(1000, 5000));
                RefreshUI = true;
            });
        }

        private static VisualElement CreateLobbyListItem(VisualElement parent, string lobbyName, int playerCount, int maxPlayerCount=2)
        {
            var item = parent.Create("w-full", "justify-between", "items-center", "border-b-2", "text-white", "flex-row", "border-emerald-900", "bg-emerald-700");
            var label = item.Create<Label>("flex-grow", "justify-center", "p-4", "text-lg", "font-bold");
            label.text = lobbyName;
            var count = item.Create<Label>("m-2", "rounded-xl", "py-2", "px-4", "bg-emerald-600");
            count.text = $"{playerCount}/{maxPlayerCount}";
            var joinButton = item.Create<Button>("w-20", "p-4", "bg-emerald-800", "text-white", "joinButton", "h-full");
            joinButton.text = "Join";
            joinButton.SetEnabled(playerCount < maxPlayerCount);
            return item;
        }
    }
}