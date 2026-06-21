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
        OnStartTurn();
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
            Vector2 currentMouse = Mouse.current.position.ReadValue();

            // Get angle of current and last mouse position relative to screen center
            Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);

            float angleNow = Mathf.Atan2(currentMouse.y - screenCenter.y,
                                           currentMouse.x - screenCenter.x) * Mathf.Rad2Deg;
            float angleLast = Mathf.Atan2(lastMouseAngle.y - screenCenter.y,
                                           lastMouseAngle.x - screenCenter.x) * Mathf.Rad2Deg;

            float delta = Mathf.DeltaAngle(angleLast, angleNow);
            transform.Rotate(0, delta * rotationSpeed, 0);

            lastMouseAngle = currentMouse;
        }
    }
}