using UnityEngine;

public abstract class MenuWithView<TView>  where TView: MonoBehaviour
{
    protected readonly TView _view;

    protected MenuWithView(Canvas canvas, string menuViewResourceName)
    {
        _view = ResourcesLoader.InstantiateLoadComponent<TView>(menuViewResourceName);
        _view.transform.SetParent(canvas.transform, false);
    }
}