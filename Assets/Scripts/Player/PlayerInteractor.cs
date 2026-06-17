using System.Globalization;
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

    void Update()
    {
        if (!IsOwner) return;

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
            var keyboard = UnityEngine.InputSystem.Keyboard.current;
            IInteractable hitInteractable = lastHit.collider.GetComponent<IInteractable>();

            if (hitInteractable != current)
            {
                current = hitInteractable;

                if (current != null)
                    UIManager.Instance.ShowPrompt(current.GetPromptPanel(), current.GetPromptText());
                else
                    UIManager.Instance.HidePrompt();
            }

            if (current != null && keyboard.eKey.isPressed)
                current.Interact();
        }
        else if (current != null)
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

        // Sphere at start and end of the cast
        Gizmos.DrawWireSphere(start, sphereRadius);
        Gizmos.DrawWireSphere(end, sphereRadius);

        // Lines connecting them, offset top/bottom/left/right, to suggest the swept capsule shape
        Vector3 up = Vector3.up * sphereRadius;
        Vector3 right = head.right * sphereRadius;

        Gizmos.DrawLine(start + up, end + up);
        Gizmos.DrawLine(start - up, end - up);
        Gizmos.DrawLine(start + right, end + right);
        Gizmos.DrawLine(start - right, end - right);

        // If it hit something, mark the exact hit point
        if (isHitting)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(lastHit.point, 0.05f);
        }
    }
}