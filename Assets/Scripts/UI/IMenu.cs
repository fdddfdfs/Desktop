using System;
using UnityEngine;

public interface IMenu
{
    public void ChangeMenuActive(Vector2 position, Action onComplete);

    public void HideMenu();
}