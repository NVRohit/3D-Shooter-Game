using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonCamera : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerBody;

    [Header("Mouse Look")]
    [SerializeField] private float sensitivity = 0.1f;
    [SerializeField] private float smoothTime = 0.03f;

    [Header("Pitch Limits")]
    [SerializeField] private float minPitch = -80f;
    [SerializeField] private float maxPitch = 80f;

    private float pitch;

    private Vector2 currentLook;
    private Vector2 smoothLookVelocity;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleMouseLook();
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

        float mouseX = currentLook.x * sensitivity;
        float mouseY = currentLook.y * sensitivity;

        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        transform.localRotation =
            Quaternion.Euler(pitch, 0f, 0f);

        playerBody.Rotate(
            Vector3.up * mouseX
        );
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