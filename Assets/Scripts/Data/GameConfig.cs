using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Data/GameConfig")]
public class GameConfig : ScriptableObject
{
    [SerializeField] private bool _isDemo;

    public bool IsDemo => _isDemo;
}