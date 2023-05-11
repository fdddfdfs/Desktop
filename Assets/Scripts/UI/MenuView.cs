using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MenuView : MonoBehaviour
    {
        [SerializeField] private GameObject _menu;
        [SerializeField] private Button _pickModel;
        [SerializeField] private Button _pickClothes;
        [SerializeField] private Button _pickAnimation;
        [SerializeField] private Button _settings;

        public bool IsActive => _menu.activeSelf;
        
        public void ShowMenu(Vector2 menuPosition)
        {
            _menu.transform.localPosition = menuPosition;
            _menu.SetActive(true);
        }

        public void HideMenu()
        {
            _menu.SetActive(false);
        }
    }
}
