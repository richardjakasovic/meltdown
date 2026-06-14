using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private CharacterController characterController;
    [SerializeField] private GameObject playerCamera;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    public override void OnNetworkSpawn()
    {
        Debug.Log($"OnNetworkSpawn — IsOwner: {IsOwner}, IsHost: {IsHost}, OwnerClientId: {OwnerClientId}, LocalClientId: {NetworkManager.Singleton.LocalClientId}");

        // Get the audio listener on this player
        AudioListener audioListener = GetComponent<AudioListener>();

        if (IsOwner)
        {
            playerCamera.SetActive(true);
            if (audioListener != null) audioListener.enabled = true;
        }
        else
        {
            characterController.enabled = false;
            if (audioListener != null) audioListener.enabled = false;
        }
    }

    void Update()
    {
        if (!IsOwner) return;
        HandleMovement();
    }

    void HandleMovement()
    {
        var keyboard = UnityEngine.InputSystem.Keyboard.current;
        if (keyboard == null) return;

        float h = 0f;
        float v = 0f;

        if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed) h = 1f;
        if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed) h = -1f;
        if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed) v = 1f;
        if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed) v = -1f;

        Vector3 move = new Vector3(h, 0, v) * moveSpeed * Time.deltaTime;
        characterController.Move(move);
    }
}