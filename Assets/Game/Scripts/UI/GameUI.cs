using System.Collections;
using Fusion;
using Game.Scripts.Game;
using Game.Scripts.Utils;
using UnityEngine.UIElements;

namespace Game.Scripts.UI
{
    public class GameUI: Menu
    {
        protected override IEnumerator Generate()
        {
            yield return null;
            
            var root = Document.rootVisualElement;
            root.Clear();
            root.styleSheets.Add(style);

            var container = root.Create("w-full", "h-full", "justify-between");
            
            var topBar = container.Create("bg-emerald-900", "p-4", "text-white");
            if (IsMobile)
            {
                topBar.AddToClassList("pt-12");
            }

            var bottomBar = container.Create("bg-emerald-700", "w-full", "p-4");
            var units = bottomBar.Create<ScrollView>("w-full", "bg-emerald-800", "flex-col");
            for (var i = 0; i < GameManager.Instance.Units.Count; i++)
            {
                var unitButton = units.Create<Button>("text-center", "text-white");
                if (GameManager.Instance.SelectedUnitIndex == i)
                {
                    unitButton.AddToClassList("bg-emerald-600");
                }
                unitButton.text = GameManager.Instance.Units[i].name;
                var index = i;
                unitButton.RegisterCallback<ClickEvent>(_ =>
                {
                    GameManager.Instance.SetSelectedUnitIndex(index);
                    RefreshUI = true;
                });
            }
            if (IsMobile)
            {
                bottomBar.AddToClassList("pb-8");
            }
        }
    }
}