using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class Menu
    {
        private const string MenuViewResourceName = "UI/MenuView";
        
        private readonly InputActionMap _input;
        private readonly MenuView _menuView;

        public Menu(InputActionMap input, Canvas canvas)
        {
            _input = input;
            _input["RightMouse"].started += ChangeMenuActive;

            _menuView = ResourcesLoader.InstantiateLoadComponent<MenuView>(MenuViewResourceName);
            _menuView.transform.SetParent(canvas.transform, false);
        }

        private void ChangeMenuActive(InputAction.CallbackContext context)
        {
            if (_menuView.IsActive)
            {
                _menuView.HideMenu();
            }
            else
            {
                Vector2 mousePosition = MouseUtils.GetMousePosition();
                
                _menuView.ShowMenu(mousePosition);
            }
        }
    }
}
