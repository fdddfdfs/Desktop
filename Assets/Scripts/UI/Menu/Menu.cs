using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class Menu
    {
        private const string MenuViewResourceName = "UI/MenuView";
        
        private readonly InputActionMap _input;
        private readonly MenuView _menuView;
        private readonly List<IMenu> _menus;

        public Menu(InputActionMap input, Canvas canvas, List<IMenu> menus)
        {
            _input = input;
            _input["RightMouse"].started += ChangeMenuActive;

            _menus = menus;

            _menuView = ResourcesLoader.InstantiateLoadComponent<MenuView>(MenuViewResourceName);
            _menuView.transform.SetParent(canvas.transform, false);

            _menuView.Init(menus);
        }

        private void ChangeMenuActive(InputAction.CallbackContext context)
        {
            if (_menuView.IsActive)
            {
                _menuView.HideMenu();

                foreach (IMenu menu in _menus)
                {
                    menu.HideMenu();
                }
            }
            else
            {
                Vector2 mousePosition = MouseUtils.GetMousePosition();
                
                _menuView.ShowMenu(mousePosition);
            }
        }
    }
}
