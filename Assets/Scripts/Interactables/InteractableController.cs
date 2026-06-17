using System;
using UnityEngine;

public class InteractableController : MonoBehaviour
{
    private Action onSuccess;
    private Action onFail;

    public void Setup(Action successCallback, Action failCallback = null)
    {
        onSuccess = successCallback;
        onFail = failCallback;
    }

    void OnEnable()
    {
        // TODO: reset your minigame's state here each time the panel opens
    }

    // Call this from wherever your minigame logic decides the player won
    public void Win()
    {
        onSuccess?.Invoke();
        UIManager.Instance.CloseInteractable();
    }

    // Call this from wherever your minigame logic decides the player lost
    public void Fail()
    {
        onFail?.Invoke();
        UIManager.Instance.CloseInteractable();
    }
}