
using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonCamera : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerBody;
    [SerializeField] private Camera playerCamera;

    [Header("Mouse Look")]
    [SerializeField] private float sensitivity = 0.1f;
    [SerializeField] private float smoothTime = 0.03f;

    [Header("Pitch Limits")]
    [SerializeField] private float minPitch = -80f;
    [SerializeField] private float maxPitch = 80f;

    [Header("Aim Down Sights")]
    [SerializeField] private float normalFOV = 60f;
    [SerializeField] private float aimFOV = 40f;
    [SerializeField] private float aimSensitivityMultiplier = 0.5f;
    [SerializeField] private float aimTransitionSpeed = 10f;

    private float pitch;
    private Vector2 currentLook;
    private Vector2 smoothLookVelocity;
    private bool isAiming;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (playerCamera != null)
        {
            normalFOV = playerCamera.fieldOfView;
        }
    }

    private void Update()
    {
        HandleAim();
        HandleMouseLook();
        HandleFOV();
    }

    private void HandleAim()
    {
        isAiming = Mouse.current != null &&
                   Mouse.current.rightButton.isPressed;
    }

    private void HandleFOV()
    {
        if (playerCamera == null)
            return;

        float targetFOV = isAiming ? aimFOV : normalFOV;

        playerCamera.fieldOfView = Mathf.Lerp(
            playerCamera.fieldOfView,
            targetFOV,
            aimTransitionSpeed * Time.deltaTime
        );
    }

    private void HandleMouseLook()
    {
        if (Mouse.current == null)
            return;

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        currentLook = Vector2.SmoothDamp(
            currentLook,
            mouseDelta,
            ref smoothLookVelocity,
            smoothTime
        );

        float activeSensitivity = sensitivity *
            (isAiming ? aimSensitivityMultiplier : 1f);

        float mouseX = currentLook.x * activeSensitivity;
        float mouseY = currentLook.y * activeSensitivity;

        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);

        if (playerBody != null)
        {
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
