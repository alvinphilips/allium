using System.Collections;
using Game.Scripts.Utils;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Scripts.UI
{
    public class MainMenu: Menu
    {
        // TODO: Switch to using MenuManager for this
        [SerializeField] private CreditsPage credits;
        
        protected override IEnumerator Generate()
        {
            yield return null;
            
            var root = Document.rootVisualElement;
            root.Clear();
            root.styleSheets.Add(style);

            var container = root.Create("w-full", "h-full", "bg-emerald-600");

            if (IsMobile)
            {
                container.AddToClassList("pt-12");
            }

            container.Create<Label>("text-white", "text-4xl", "text-center", "pb-8").text = "Main Menu";
            
            var playButton = container.Create<Button>("text-2xl", "m-4",  "text-white", "p-4", "bg-emerald-800");
            playButton.text = "Play";
            playButton.RegisterCallback<ClickEvent>(_ =>
            {
                MenuManager.Instance.SetCurrentMenu(this);
                MenuManager.Instance.ShowMenu(MenuManager.Instance.lobbyListMenu, true);
            });

            var optionsButton = container.Create<Button>("text-2xl", "m-4", "text-white", "p-4", "bg-emerald-800");
            optionsButton.text = "Options";
            optionsButton.RegisterCallback<ClickEvent>(_ =>
            {
                MenuManager.Instance.SetCurrentMenu(this);
                MenuManager.Instance.ShowMenu(MenuManager.Instance.optionsMenu, true);
            });
            
            var creditsButton = container.Create<Button>("text-2xl", "m-4", "text-white", "p-4", "bg-emerald-800");
            creditsButton.text = "Credits";
            creditsButton.RegisterCallback<ClickEvent>(_ =>
            {
                MenuManager.Instance.SetCurrentMenu(this);
                MenuManager.Instance.ShowMenu(credits, true);
            });
        }
    }
}