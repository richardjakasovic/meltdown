using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class FuelValveInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string promptText;
    public GameObject promptPanelPrefab;
    public float rotationSpeed = 0.2f;
    private bool isBeingTurned;
    [SerializeField] private float anglePerStep = 45f;
    private int step = 0;
    private const int MaxSteps = 3;

    public void Interact(PlayerInteractor playerInteractor,
                         IInteractable hitInteractable,
                         GameObject fuelValvePrefab)
    {
        //OnStartTurn();
        isBeingTurned = true;
    }

    public GameObject GetPromptPanel() => promptPanelPrefab;

    public string GetPromptText() => promptText;


    public void Update()
    {
        if (!isBeingTurned)
            return;

        if (Keyboard.current.eKey.wasPressedThisFrame && step < MaxSteps)
        {
            step++;
        }

        if (Keyboard.current.qKey.wasPressedThisFrame && step > 0)
        {
            step--;
        }

        transform.localRotation =
            Quaternion.Euler(step * anglePerStep, 0f, 0f);
    }
}