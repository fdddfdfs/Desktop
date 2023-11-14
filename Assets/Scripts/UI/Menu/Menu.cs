using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI
{
    public class Menu : MenuWithView<MenuView>
    {
        private const string MenuViewResourceName = "UI/MenuView";
        
        private readonly List<IMenu> _menus;
        private readonly Model _model;
        private readonly EntryText _entryText;
        
        private bool _isActivatingWindow;
        private SettingsMenu _settingsMenu;

        public GraphicRaycaster LanguageDropdownRaycaster => _settingsMenu.LanguageDropdownRaycaster;

        public Menu(
            InputActionMap input,
            Canvas canvas,
            Model model,
            List<ModelData> modelDatas,
            List<AnimationData> animationDatas,
            List<AnimationMenuData> animationMenuDatas,
            List<CustomizationData> customizationDatas,
            List<CustomizationMenuData> customizationMenuDatas,
            GameConfig gameConfig,
            EntryText entryText) : base(canvas, MenuViewResourceName)
        {
            Transform viewTransform = _view.transform;
            ModelMenu modelMenu = new (model, modelDatas, canvas, viewTransform, gameConfig);
            AnimationMenu animationMenu = new (
                model,
                canvas,
                animationMenuDatas,
                animationDatas,
                viewTransform,
                gameConfig);
            CustomizationMenu customizationMenu = new (
                model,
                canvas,
                customizationMenuDatas,
                customizationDatas,
                viewTransform,
                gameConfig);
            _settingsMenu = new SettingsMenu(canvas, viewTransform);

            List<IMenu> menus = new () { modelMenu, animationMenu, customizationMenu, _settingsMenu };
            
            input["RightMouse"].started += ChangeMenuActive;
            InputAction activatingWindow = input["ActiveWindow"];
            activatingWindow.started += (_) =>
            {
                _isActivatingWindow = true;
            };
            activatingWindow.canceled += (_) =>
            {
                _isActivatingWindow = false;
            };

            _menus = menus;
            _model = model;
            _entryText = entryText;
            
            _view.Init(menus);
        }

        public void ChangeMenuActive(InputAction.CallbackContext context)
        {
            if (!_isActivatingWindow || !_model.IsMouseOnModel()) return;
            
            _entryText.HideText();

            if (_view.IsActive)
            {
                _view.HideMenu();

                foreach (IMenu menu in _menus)
                {
                    menu.HideMenu();
                }
            }
            else
            {
                Vector2 mousePosition = MouseUtils.GetMousePosition();
                Vector2 menuPosition = new (Screen.width / 4f * -Mathf.Sign(mousePosition.x), 0);

                _view.ShowMenu(menuPosition);
            }
        }
    }
}
