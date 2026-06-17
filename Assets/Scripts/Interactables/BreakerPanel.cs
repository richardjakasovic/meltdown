using UnityEngine;

public class BreakerPanel : MonoBehaviour, IInteractable
{
    public GameObject breakerPanelPrefab;
    public GameObject promptPanelPrefab;

    public void Interact()
    {
        UIManager.Instance.OpenInteractable(breakerPanelPrefab,
            onSuccess: () => Debug.Log("Breaker panel fixed!"),
            onFail: () => Debug.Log("BreakerPanel not fixed!")
        );
    }

    public GameObject GetPromptPanel() => promptPanelPrefab;
    public string GetPromptText() => "Press E to open panel";
}