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

            var optionsPanel = container.Create("w-full");
            optionsPanel.Create<Label>("text-4xl", "text-white").text = "in progress";
            
            var leaveButton = container.Create<Button>("text-2xl", "text-white", "bg-emerald-900", "p-4");
            if (IsMobile)
            {
                leaveButton.AddToClassList("pb-8");
            }
            leaveButton.text = "Leave";
            leaveButton.RegisterCallback<ClickEvent>(evt => MenuManager.Instance.RestorePreviousState());

        }
    }
}