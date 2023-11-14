using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Model : IUpdatable
{
    private const float Speed = 0.001f;
    private const float MinimumScale = 0.1f;
    private const float RotationSpeed = 0.5f;

    private readonly Camera _camera;
    private readonly LayerMask _raycastMask;
    private readonly Dictionary<ModelData, ModelView> _models;
    private readonly Vector2 _borders;
    private readonly InputAction _scroll;
    private readonly Animator _animator;
    private readonly InputAction _mouseDelta;

    private bool _isMoving;
    private bool _isRotating;
    private ModelView _currentModel;
    private Vector2 _movingOffset;
    private AnimationClip _currentClip;
    private float _currentAnimationSpeed;

    public ModelData CurrentModelData { get; private set; }

    public Model(List<ModelData> modelDatas, InputActionMap inputActionMap, Camera camera)
    {
        _camera = camera;
        _raycastMask = 1 << LayerMask.NameToLayer("Model");

        InputAction leftMouse = inputActionMap["LeftMouse"];
        leftMouse.started += _ => StartMoving();
        leftMouse.canceled += _ => StopMoving();

        InputAction rightMouse = inputActionMap["RightMouse"];
        rightMouse.started += _ => StartRotation();
        rightMouse.canceled += _ => StopRotation();

        _scroll = inputActionMap["Scroll"];
        _mouseDelta = inputActionMap["MouseDelta"];
        
        _models = new Dictionary<ModelData, ModelView>();
        foreach (ModelData modelData in modelDatas)
        {
            var modelView = ResourcesLoader.InstantiateLoadedComponent<ModelView>(modelData.Prefab);
            _models[modelData] = modelView;
            modelView.ChangeActive(false);
            modelView.BoxCollider.enabled = false;
        }

        _currentModel = _models[modelDatas[0]];
        _currentModel.ChangeActive(true);
        _currentModel.BoxCollider.enabled = true;
        CurrentModelData = modelDatas[0];
        Achievements.Instance.GetAchievement(modelDatas[0].AchievementData);
        Rotate();
        
        _borders = new Vector2(Screen.width, Screen.height);
        _borders = _camera.ScreenToWorldPoint(_borders);
    }
    
    public bool IsMouseOnModel()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = _camera.ScreenPointToRay(mousePosition);
        return Physics.Raycast(ray, out RaycastHit _, Mathf.Infinity, _raycastMask);
    }

    public void ChangeModel(ModelData newModel)
    {
        ModelView newModelView = _models[newModel];

        if (newModelView == _currentModel)
        {
            Debug.LogWarning("Trying to change model to same model");
            return;
        }

        newModelView.ChangeActive(true);
        newModelView.transform.position = _currentModel.transform.position;
        newModelView.transform.localScale = _currentModel.transform.localScale;
        newModelView.BoxCollider.enabled = true;

        float animationNormalizedTime = _currentModel.GetCurrentAnimationNormalizedTime();
        newModelView.ChangeAnimation(_currentClip, animationNormalizedTime);
        
        _currentModel.ChangeActive(false);
        _currentModel.BoxCollider.enabled = false;
        _currentModel = newModelView;
        CurrentModelData = newModel;
        Rotate();
        _currentModel.ChangeAnimationSpeed(_currentAnimationSpeed);
    }

    public void ChangeAnimation(AnimationClip animationClip)
    {
        _currentModel.ChangeAnimation(animationClip);
        _currentClip = animationClip;
    }

    public void ChangeCustomization(CustomizationData customizationData)
    {
        if(customizationData is HairData hairData)
        {
            _currentModel.CustomizeHairs(hairData);
            return;
        }
        
        _currentModel.Customize(customizationData.CustomizationType, customizationData.Material);
    }

    public void CustomizeWeight(CustomizationType type, float newWeight)
    {
        _currentModel.CustomizeWeight(type, newWeight);
    }

    public void ChangeAnimationSpeed(float speed)
    {
        _currentModel.ChangeAnimationSpeed(speed);
        _currentAnimationSpeed = speed;
    }

    public void Update()
    {
        if (_isMoving)
        {
            Move();
        }

        if (_isRotating)
        {
            Rotate();
        }

        float scrollDeltaY = _scroll.ReadValue<Vector2>().y;

        if (scrollDeltaY != 0 && IsMouseOnModel())
        {
            ChangeScale(scrollDeltaY);
        }
    }

    private void Move()
    {
        Vector2 newPosition = (Vector2)_camera.ScreenToWorldPoint(Mouse.current.position.ReadValue()) 
                              + _movingOffset; 
        _currentModel.transform.position = GetClampedPosition(newPosition);
    }

    private Vector2 GetClampedPosition(Vector2 position)
    {
        if (Mathf.Abs(position.x) + _currentModel.Size.x / 2 > _borders.x)
        {
            position = new Vector2(
                Mathf.Sign(position.x) * (_borders.x - _currentModel.Size.x / 2),
                position.y);
        }
            
        if (position.y + _currentModel.Size.y > _borders.y)
        {
            position = new Vector2(
                position.x,
                _borders.y - _currentModel.Size.y);
        }
        else if (position.y < -_borders.y)
        {
            position = new Vector2(position.x, -_borders.y);
        }

        return position;
    }

    private void ChangeScale(float delta)
    {
        delta *= Speed;
        
        if (_currentModel.Size.y + delta > _borders.y * 2) return;
        if (_currentModel.Size.y + delta < _borders.y * MinimumScale) return;

        _currentModel.transform.localScale += Vector3.one * delta;
        _currentModel.transform.position = GetClampedPosition(_currentModel.transform.position);
    }

    private void Rotate()
    {
        var mouseDelta = _mouseDelta.ReadValue<Vector2>();
        _currentModel.ModelTransform.transform.rotation *= Quaternion.Euler(0, -mouseDelta.x * RotationSpeed, 0);

        _currentModel.ChangeHairsBoneGravity(_currentModel.ModelTransform.rotation * 
                                             Quaternion.Euler(0,180,0) *
                                             CurrentModelData.HairsBoneGravity);
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

    private void StopMoving()
    {
        _isMoving = false;
    }

    private void StartRotation()
    {
        if (IsMouseOnModel())
        {
            _isRotating = true;
        }
    }

    private void StopRotation()
    {
        _isRotating = false;
    }
}