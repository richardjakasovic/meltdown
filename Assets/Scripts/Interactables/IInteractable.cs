using System;
using UnityEngine;

public interface IInteractable
{
    void Interact(PlayerInteractor playerInteractor);
    GameObject GetPromptPanel();
    string GetPromptText();

    bool isOpen { get; set; }
}
