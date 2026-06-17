using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BreakerPanel : MonoBehaviour, IInteractable
{
    public GameObject breakerPanelPrefab;
    public GameObject promptPanelPrefab;
    [SerializeField] private string promptText;
    private bool _isOpen;

    public bool isOpen { get => _isOpen; set => _isOpen = value; }

    public void Interact(PlayerInteractor playerInteractor)
    {
        if (!_isOpen)
        {
            UIManager.Instance.OpenInteractable(breakerPanelPrefab,
            onSuccess: () => Debug.Log("Breaker panel fixed!"),
            onFail: () => Debug.Log("BreakerPanel not fixed!")
        );
            isOpen = true;
            playerInteractor.isInteracting = true;
        }
        else
        {
            UIManager.Instance.CloseInteractable();
            isOpen = false;
            playerInteractor.isInteracting = false;
        }
    }

    public GameObject GetPromptPanel() => promptPanelPrefab;
    public string GetPromptText() => promptText;
}