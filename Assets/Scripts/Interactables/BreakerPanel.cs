using UnityEngine;

public class BreakerPanel : MonoBehaviour, IInteractable
{
    public GameObject breakerPanelPrefab;
    public GameObject promptPanelPrefab;
    [SerializeField] private string promptText;

    public void Interact(PlayerInteractor playerInteractor, IInteractable hitInteractable, GameObject interactableGameObject)
    {
        // Open UI (single shared panel system)
        UIManager.Instance.OpenInteractable(breakerPanelPrefab);

        // Set interaction state
        playerInteractor.isInteracting = true;

        // Hide prompt immediately
        UIManager.Instance.HidePrompt();
    }

    public GameObject GetPromptPanel()
    {
        return promptPanelPrefab;
    }

    public string GetPromptText()
    {
        return promptText;
    }
}