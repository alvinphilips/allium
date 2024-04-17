using System.Collections.Generic;
using Game.Scripts.Patterns;
using UnityEngine;

namespace Game.Scripts.UI
{
    public class MenuManager : Singleton<MenuManager>
    {
        public LobbyListMenu lobbyListMenu;
        public InLobbyMenu inLobbyMenu;

        [SerializeField] private GameObject dummy;
        
        public readonly Dictionary<string, Menu> RegisteredMenus = new();

        public void RegisterMenu(Menu menu)
        {
            RegisteredMenus.Add(menu.name, menu);
        }

        public Menu GetMenu(string menuName)
        {
            if (!RegisteredMenus.TryGetValue(menuName, out var menu))
            {
                Debug.LogError($"Could not find Menu {menuName}");
            }

            return menu;
        }

        public void HideAllMenus()
        {
            foreach (var menu in RegisteredMenus.Values)
            {
                menu.Hide();
            }
        }
        
        public void ShowMenu(Menu menu)
        {
            ShowDummy();
            HideAllMenus();
            
            menu.Show();
            menu.RefreshUI = true;
            HideDummy();
        }
        
        public void ShowMenu(string menuName)
        {
            var menu = GetMenu(menuName);

            ShowMenu(menu);
        }

        public void ShowDummy()
        {
            dummy.SetActive(true);   
        }
        
        public void HideDummy()
        {
            dummy.SetActive(false);   
        }
    }
}
