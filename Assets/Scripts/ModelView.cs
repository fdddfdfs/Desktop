using UnityEngine;

public class ModelView : MonoBehaviour
{
    [SerializeField] private GameObject _model;
    [SerializeField] private Animator _animator;
    [SerializeField] private BoxCollider _boxCollider;

    public Vector2 Size => _boxCollider.size * (Vector2)_boxCollider.transform.localScale;

    public void ChangeActive(bool active)
    {
        _model.SetActive(active);
    }
}