using UnityEngine;

public class EntryText : MenuWithView<EntryTextView>
{
    private const float EntryTextTime = 5f;
    private const string EntryTextResourceName = "UI/EntryText";

    private bool _isHidden;
    
    public EntryText(Canvas canvas) : base(canvas, EntryTextResourceName)
    {
        ShowEntryText();
    }

    public void HideText()
    {
        if (_isHidden) return;
        
        _view.HideText();
        _isHidden = true;
    }
    
    private void ShowEntryText()
    {
        _view.ShowTextForTime(EntryTextTime);
    }
}