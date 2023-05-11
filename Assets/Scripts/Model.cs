using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Model : IUpdatable
{
    private const float Speed = 0.001f;
    
    private readonly Camera _camera;
    private readonly LayerMask _raycastMask;
    private readonly Dictionary<ModelData, ModelView> _models;
    private readonly Vector2 _borders;
    private readonly InputAction _scroll;

    private bool _isMoving;
    private ModelView _currentModel;
    private Vector2 _movingOffset;

    public Model(List<ModelData> modelDatas, InputActionMap inputActionMap, Camera camera)
    {
        _camera = camera;
        _raycastMask = 1 << LayerMask.NameToLayer("Model");

        InputAction leftMouse = inputActionMap["LeftMouse"];
        leftMouse.started += (_) => StartMoving();
        leftMouse.canceled += (_) => StopMoving();

        _scroll = inputActionMap["Scroll"];

        _models = new Dictionary<ModelData, ModelView>();
        foreach (ModelData modelData in modelDatas)
        {
            var modelView = ResourcesLoader.InstantiateLoadedComponent<ModelView>(modelData.Prefab);
            _models[modelData] = modelView;
            modelView.ChangeActive(false);
        }

        _currentModel = _models[modelDatas[0]];
        _currentModel.ChangeActive(true);

        _borders = new Vector2(Screen.width, Screen.height);
        _borders = _camera.ScreenToWorldPoint(_borders);
    }

    public void ChangeModel(ModelData newModel)
    {
        ModelView newModelView = _models[newModel];
        newModelView.ChangeActive(true);
        newModelView.transform.position = _currentModel.transform.position;
        newModelView.transform.localScale = _currentModel.transform.localScale;
        
        _currentModel.ChangeActive(false);
        _currentModel = newModelView;
    }

    public void Update()
    {
        if (_isMoving)
        {
            Move();
        }

        float scrollDeltaY = _scroll.ReadValue<Vector2>().y;

        if (scrollDeltaY != 0)
        {
            ChangeScale(scrollDeltaY);
        }
    }

    private void Move()
    {
        Vector2 newPosition = (Vector2)_camera.ScreenToWorldPoint(Mouse.current.position.ReadValue()) 
                              + _movingOffset;
        if (Mathf.Abs(newPosition.x) + _currentModel.Size.x / 2 > _borders.x)
        {
            newPosition = new Vector2(
                Mathf.Sign(newPosition.x) * (_borders.x - _currentModel.Size.x / 2),
                newPosition.y);
        }
            
        if (newPosition.y + _currentModel.Size.y > _borders.y)
        {
            newPosition = new Vector2(
                newPosition.x,
                _borders.y - _currentModel.Size.y);
        }
        else if (newPosition.y < -_borders.y)
        {
            newPosition = new Vector2(newPosition.x, -_borders.y);
        }
            
        _currentModel.transform.position = newPosition;
    }

    private void ChangeScale(float delta)
    {
        _currentModel.transform.localScale += Vector3.one * delta * Speed;
    }

    private void StartMoving()
    {
        if (IsMouseOnModel())
        {
            _isMoving = true;
            _movingOffset = _currentModel.transform.position 
                            - _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        }
    }

    private bool IsMouseOnModel()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = _camera.ScreenPointToRay(mousePosition);
        return Physics.Raycast(ray, out RaycastHit _, Mathf.Infinity, _raycastMask);
    }

    private void StopMoving()
    {
        _isMoving = false;
    }
}