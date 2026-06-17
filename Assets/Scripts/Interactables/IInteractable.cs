using UnityEngine;

public interface IInteractable
{
    void Interact();
    GameObject GetPromptPanel();
    string GetPromptText();
}
