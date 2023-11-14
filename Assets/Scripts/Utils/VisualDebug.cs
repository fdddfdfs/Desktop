using TMPro;
using UnityEngine;

public class VisualDebug : MonoBehaviour
{
    public static VisualDebug Instance;

    [SerializeField] private TMP_Text _debugText;
    
    public void ShowDebug(string message)
    {
        _debugText.text = message;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}