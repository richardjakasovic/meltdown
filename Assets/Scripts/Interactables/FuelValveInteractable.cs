using UnityEngine;
using UnityEngine.InputSystem;

public class FuelValveInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string promptText;
    public GameObject promptPanelPrefab;
    public float rotationSpeed = 0.2f;
    private bool isBeingTurned;
    private Vector2 lastMouseAngle;

    public void Interact(PlayerInteractor playerInteractor,
                         IInteractable hitInteractable,
                         GameObject fuelValvePrefab)
    {
        //OnStartTurn();
        isBeingTurned = true;
    }

    public void OnStartTurn()
    {
        lastMouseAngle = Mouse.current.position.ReadValue();
    }

    public GameObject GetPromptPanel() => promptPanelPrefab;

    public string GetPromptText() => promptText;


    public void Update()
    {
        if (isBeingTurned)
        {
            var keyboard = Keyboard.current;

            if (keyboard.eKey.wasPressedThisFrame)
            {
                transform.Rotate(0, 45, 0);
            }
        }
    }
}