using System.Collections;
using Game.Scripts.Utils;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Scripts.UI
{
    public class OptionsMenu: Menu
    {
        [SerializeField] private StyleSheet style;
        
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
        }
    }
}