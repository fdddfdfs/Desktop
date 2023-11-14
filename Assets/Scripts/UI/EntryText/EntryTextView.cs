using System.Threading;
using TMPro;
using UnityEngine;

public class EntryTextView : MonoBehaviour
{
    [SerializeField] private TMP_Text _entryText;

    private CancellationTokenSource _cancellationTokenSource;
    
    public async void ShowTextForTime(float time)
    {
        _entryText.text = $"{AllTexts.EntryTextButtons} {Localization.Instance[AllTexts.EntryTextMessage]}";
        _entryText.gameObject.SetActive(true);
        _entryText.transform.SetAsLastSibling();
        
        _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(
            AsyncUtils.Instance.GetCancellationToken());

        await AsyncUtils.Instance.Wait(time, _cancellationTokenSource.Token).ContinueWith(AsyncUtils.Instance.EmptyTask);

        if (!AsyncUtils.Instance.GetCancellationToken().IsCancellationRequested)
        {
            _entryText.gameObject.SetActive(false);
        }
    }

    public void HideText()
    {
        _cancellationTokenSource.Cancel();
    }
}