using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : NetworkBehaviour
{
    public Transform head;
    public float interactRange = 3f;
    public float sphereRadius = 0.15f;
    public LayerMask interactableLayer;

    private IInteractable current;
    private RaycastHit lastHit;
    private bool isHitting;

    public bool isInteracting;

    private GameObject currentTargetObject;

    void Update()
    {
        if (!IsOwner) return;

        var keyboard = Keyboard.current;

        if (isInteracting)
        {
            HandleInteractionMode(keyboard);
            return;
        }

        HandleLookMode(keyboard);
    }

    private void HandleInteractionMode(Keyboard keyboard)
    {
        // ESC closes UI
        if (keyboard.escapeKey.wasPressedThisFrame)
        {
            UIManager.Instance.CloseInteractable();
            isInteracting = false;
            currentTargetObject = null;
            return;
        }

        if (currentTargetObject != null)
        {
            float distance = Vector3.Distance(
                head.position,
                currentTargetObject.transform.position
            );

            if (distance > interactRange)
            {
                Debug.Log("OUT OF RANGE → closing UI");

                UIManager.Instance.CloseInteractable();
                isInteracting = false;
                currentTargetObject = null;
            }
        }
    }

    private void HandleLookMode(Keyboard keyboard)
    {
        isHitting = Physics.SphereCast(
            head.position,
            sphereRadius,
            head.forward,
            out lastHit,
            interactRange,
            interactableLayer
        );

        if (isHitting)
        {
            GameObject interactableGameObject = lastHit.collider.gameObject;
            IInteractable hitInteractable =
                interactableGameObject.GetComponent<IInteractable>();

            // Update current hover target (prompt system)
            if (hitInteractable != current)
            {
                current = hitInteractable;

                if (current != null)
                    UIManager.Instance.ShowPrompt(
                        current.GetPromptPanel(),
                        current.GetPromptText()
                    );
                else
                    UIManager.Instance.HidePrompt();
            }

            // Interact
            if (current != null && keyboard.eKey.wasPressedThisFrame)
            {
                current.Interact(this, hitInteractable, interactableGameObject);

                currentTargetObject = lastHit.collider.transform.root.gameObject;
                isInteracting = true;

                UIManager.Instance.HidePrompt();
            }
        }
        else
        {
            current = null;
            UIManager.Instance.HidePrompt();
        }
    }

    void OnDrawGizmos()
    {
        if (head == null) return;

        Vector3 start = head.position;
        Vector3 end = start + head.forward * interactRange;

        Gizmos.color = isHitting ? Color.green : Color.red;

        Gizmos.DrawWireSphere(start, sphereRadius);
        Gizmos.DrawWireSphere(end, sphereRadius);

        Vector3 up = Vector3.up * sphereRadius;
        Vector3 right = head.right * sphereRadius;

        Gizmos.DrawLine(start + up, end + up);
        Gizmos.DrawLine(start - up, end - up);
        Gizmos.DrawLine(start + right, end + right);
        Gizmos.DrawLine(start - right, end - right);

        if (isHitting)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(lastHit.point, 0.05f);
        }
    }
}