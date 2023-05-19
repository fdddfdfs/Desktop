using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MenuView : MonoBehaviour
    {
        private const float AnimationTime = 0.1f;
        private const float AnimationScaleOffset = 1.5f;
        
        [SerializeField] private GameObject _menu;
        [SerializeField] private Button _pickModel;
        [SerializeField] private Button _pickClothes;
        [SerializeField] private Button _pickAnimation;
        [SerializeField] private Button _settings;

        public bool IsActive => _menu.activeSelf;

        public void Init(List<IMenu> menus)
        {
            foreach (IMenu menu in menus)
            {
                menu.HideMenu();
            }
            
            List<Button> buttons = new (){ _pickModel, _pickClothes, _pickAnimation, _settings };
            for (var i = 0; i < buttons.Count; i++)
            {
                int index = i;

                buttons[index].onClick.AddListener(
                    () =>
                    {
                        foreach (IMenu menu in menus)
                        {
                            menu.HideMenu();
                        }
                        
                        menus[index].ChangeMenuActive(_menu.transform.localPosition, null);
                    });
            }

            _menu.SetActive(false);
        }

        public void ShowMenu(Vector2 menuPosition)
        {
            _menu.transform.localPosition = menuPosition;
            _menu.SetActive(true);
            
            _menu.transform.DOKill();
            _menu.transform
                .DOScale(Vector2.one * AnimationScaleOffset, AnimationTime)
                .OnComplete(() => _menu.transform.DOScale(Vector2.one, AnimationTime));
        }

        public void HideMenu()
        {
            _menu.transform.DOKill();
            _menu.transform
                .DOScale(Vector2.zero, AnimationTime)
                .OnComplete(() => _menu.SetActive(false));
        }
        
        private void Awake()
        {
            _menu.transform.localScale = Vector3.zero;
        }
    }
}
