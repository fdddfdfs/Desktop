using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SubmenuView<TData> : AnimatedMenuView where TData: ScriptableObject, IIcon
{
    [SerializeField] protected List<Button> _buttons;
    [SerializeField] protected List<Image> _lockers;
    [SerializeField] private List<Image> _buttonsImages;

    protected GameConfig _gameConfig;
    
    private Submenu<TData> _submenu;
    private List<TData> _data;

    public Vector2 Position => _menu.transform.localPosition;

    public int Count => _buttons.Count;

    public virtual void Init(Submenu<TData> submenu, GameConfig gameConfig)
    {
        HideMenu(null);
        _submenu = submenu;
        _gameConfig = gameConfig;
        
        _data = new List<TData>();
        for (var i = 0; i < _buttons.Count; i++)
        {
            _data.Add(null);
        }
        
        for (var i = 0; i < _buttons.Count; i++)
        {
            int index = i;
            _buttons[i].onClick.AddListener(() => _submenu.ButtonClicked(index, _data[index]));
        }

        _menu.transform.localScale = Vector3.zero;
        _menu.transform.localRotation = Quaternion.identity;
    }
    
    public void ChangeButton(int index, TData data)
    {
        _buttonsImages[index].DOColor(new Color(0, 0, 0, 0), Duration).OnComplete(() =>
        {
            _buttonsImages[index].sprite = data.Icon;
            _buttonsImages[index].DOColor(Color.white, Duration);
        });

        _data[index] = data;
    }
}