using System;
using UnityEngine;

public interface IInteractable
{
    void Interact(PlayerInteractor playerInteractor, IInteractable hitInteractable, GameObject interactableGameObject);
    GameObject GetPromptPanel();
    string GetPromptText();
}
