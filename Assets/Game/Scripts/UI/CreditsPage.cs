using System;
using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Utils;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Scripts.UI
{
    public class CreditsPage: Menu
    {
        [Serializable]
        public struct CreditsEntry
        {
            public string name;
            public string description;
        }
        [SerializeField] private List<CreditsEntry> creditsList = new();
        [SerializeField] private bool displayDescription;
        [SerializeField] private StyleSheet style;
        
        protected override IEnumerator Generate()
        {
            yield return null;
            
            var root = Document.rootVisualElement;
            root.Clear();
            root.styleSheets.Add(style);

            var container = root.Create("w-full", "h-full", "justify-between", "bg-emerald-600");

            var credits = container.Create<ScrollView>("w-full");
            credits.horizontalScrollerVisibility = ScrollerVisibility.Hidden;
            if (IsMobile)
            {
                credits.AddToClassList("pt-12");
            }
            credits.Create<Label>("text-4xl", "text-white", "w-full", "text-center", "p-4").text = "Credits";

            foreach (var entry in creditsList)
            {
                var row = credits.Create("bg-emerald-700", "text-white", "text-center", "p-4", "m-2");
                row.Create<Label>("text-lg").text = entry.name;
                if (displayDescription)
                {
                    row.Create<Label>().text = entry.description;
                }
            }

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