using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject promptPanel;
    public TMP_Text promptText;

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

    public void ShowPrompt(GameObject panelPrefab, string text)
    {
        if (currentPromptPanel != null)
            Destroy(currentPromptPanel);

        if (panelPrefab == null)
        {
            currentPromptPanel = null;
            Debug.Log("panelPrefab was null, aborting");
            return;
        }

        currentPromptPanel = Instantiate(panelPrefab, uiRoot);
        currentPromptPanel.SetActive(true);
        currentPromptPanel.GetComponent<PromptPanelUI>()?.SetText(text);
    }

    public void HidePrompt()
    {
        if (currentPromptPanel != null)
            Destroy(currentPromptPanel);
        currentPromptPanel = null;
    }

    public void OpenInteractable(GameObject panelPrefab, Action onSuccess, Action onFail = null)
    {
        currentInteractablePanel = Instantiate(panelPrefab, uiRoot);
        currentInteractablePanel.GetComponent<InteractableController>().Setup(onSuccess, onFail);
        currentInteractablePanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void CloseInteractable()
    {
        if (currentInteractablePanel != null) Destroy(currentInteractablePanel);
        currentInteractablePanel = null;
        Cursor.lockState = CursorLockMode.Locked;
    }
}