using System.Collections;
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
        }
    }
}