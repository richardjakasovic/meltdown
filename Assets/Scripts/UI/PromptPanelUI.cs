using TMPro;
using UnityEngine;

public class PromptPanelUI : MonoBehaviour
{
    public TMP_Text promptText;

    public void SetText(string text)
    {
        if (promptText != null) promptText.text = text;
    }
}