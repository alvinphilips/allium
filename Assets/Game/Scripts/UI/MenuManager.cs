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
        public readonly Stack<Menu> PreviousMenus = new(5);
        private Menu _currentMenu;

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
            _currentMenu.Hide();
        }
        
        public void ShowMenu(Menu menu, bool rememberPreviousMenu = false)
        {
            ShowDummy();
            HideAllMenus();

            if (rememberPreviousMenu && _currentMenu != null)
            {
                PreviousMenus.Push(_currentMenu);
            }

            _currentMenu = menu;
            menu.Show();
            menu.RefreshUI = true;
            HideDummy();
        }

        public void SetCurrentMenu(Menu menu)
        {
            _currentMenu = menu;
        }
        
        public void ShowMenu(string menuName, bool rememberPreviousMenu = false)
        {
            var menu = GetMenu(menuName);

            ShowMenu(menu, rememberPreviousMenu);
        }

        public void ShowDummy()
        {
            dummy.SetActive(true);   
        }
        
        public void HideDummy()
        {
            dummy.SetActive(false);   
        }

        public void RestorePreviousState()
        {
            ShowDummy();
            if (PreviousMenus.TryPop(out var previousMenu))
            {
                _currentMenu = previousMenu;
            }
            
            HideAllMenus();
            _currentMenu.Show();
            _currentMenu.RefreshUI = true;
            HideDummy();
        }
    }
}
