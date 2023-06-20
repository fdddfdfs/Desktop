using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class Menu : MenuWithView<MenuView>
    {
        private const string MenuViewResourceName = "UI/MenuView";
        
        private readonly List<IMenu> _menus;
        private readonly Model _model;

        public Menu(
            InputActionMap input,
            Canvas canvas,
            Model model,
            List<ModelData> modelDatas,
            List<AnimationData> animationDatas,
            List<CustomizationData> customizationDatas,
            List<CustomizationMenuData> customizationMenuDatas) : base(canvas, MenuViewResourceName)
        {
            Transform viewTransform = _view.transform;
            ModelMenu modelMenu = new (model, modelDatas, canvas, viewTransform);
            AnimationMenu animationMenu = new (model, canvas, animationDatas, viewTransform);
            CustomizationMenu customizationMenu = new (
                model,
                canvas,
                customizationMenuDatas,
                customizationDatas,
                viewTransform);
            SettingsMenu settingsMenu = new SettingsMenu(canvas, viewTransform);

            List<IMenu> menus = new () { modelMenu, animationMenu, customizationMenu, settingsMenu };
            
            input["RightMouse"].started += ChangeMenuActive;

            _menus = menus;
            _model = model;
            
            _view.Init(menus);
        }

        private void ChangeMenuActive(InputAction.CallbackContext context)
        {
            if (_model.IsMouseOnModel()) return;
            
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
                
                _view.ShowMenu(mousePosition);
            }
        }
    }
}
