using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private CharacterController characterController;
    private PlayerInteractor interactor;
    [SerializeField] private GameObject playerCamera;

    [SerializeField] private float mouseSensitivity = 2f;

    private float pitch = 0f;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        interactor = GetComponent<PlayerInteractor>();
    }

    public override void OnNetworkSpawn()
    {
        Debug.Log($"Spawn: Local={NetworkManager.Singleton.LocalClientId}, Owner={OwnerClientId}");

        characterController.enabled = IsOwner;

        if (playerCamera != null)
            playerCamera.SetActive(IsOwner);

        var audioListener = GetComponent<AudioListener>();
        if (audioListener != null)
            audioListener.enabled = IsOwner;
    }

    void Update()
    {
        if (!IsOwner) return;
        HandleMovement();
        HandleCameraMovement();
    }

    void HandleMovement()
    {
        var keyboard = UnityEngine.InputSystem.Keyboard.current;
        if (keyboard == null) return;

        float h = 0f;
        float v = 0f;

        if (keyboard.dKey.isPressed) h = 1f;
        if (keyboard.aKey.isPressed) h = -1f;
        if (keyboard.wKey.isPressed) v = 1f;
        if (keyboard.sKey.isPressed) v = -1f;

        // Get camera directions
        Vector3 forward = playerCamera.transform.forward;
        Vector3 right = playerCamera.transform.right;

        // Flatten so we don't move up/down when looking
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        // Convert input into camera-relative movement
        Vector3 moveDirection = forward * v + right * h;

        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
    }

    void HandleCameraMovement()
    {
        if (!interactor.isInteracting)
        {
            var mouse = UnityEngine.InputSystem.Mouse.current;
            if (mouse == null) return;

            Vector2 delta = mouse.delta.ReadValue() * mouseSensitivity;

            transform.Rotate(Vector3.up * delta.x);

            pitch -= delta.y;
            pitch = Mathf.Clamp(pitch, -80f, 80f);

            playerCamera.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        }
    }
}