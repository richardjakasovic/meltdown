using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public Transform uiRoot;

    private GameObject currentInteractablePanel;
    private GameObject currentPromptPanel;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ShowPrompt(GameObject prefab, string text)
    {
        if (currentPromptPanel != null)
            Destroy(currentPromptPanel);

        currentPromptPanel = Instantiate(prefab, uiRoot);
        currentPromptPanel.GetComponent<PromptPanelUI>()?.SetText(text);
    }

    public void HidePrompt()
    {
        if (currentPromptPanel != null)
            Destroy(currentPromptPanel);

        currentPromptPanel = null;
    }

    public void OpenInteractable(GameObject prefab)
    {
        if (currentInteractablePanel == null)
            currentInteractablePanel = Instantiate(prefab, uiRoot);

        currentInteractablePanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void CloseInteractable()
    {
        if (currentInteractablePanel != null)
            currentInteractablePanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
    }
}