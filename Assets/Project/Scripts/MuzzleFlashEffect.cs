
using System.Collections;
using UnityEngine;

public class MuzzleFlashEffect : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera playerCamera;

    [Header("Flash Settings")]
    [SerializeField] private float flashDuration = 0.09f;
    [SerializeField] private float flashSize = 0.35f;
    [SerializeField]
    private Color flashColor =
        new Color(1f, 0.35f, 0.03f, 1f);

    private GameObject flashObject;
    private Material flashMaterial;
    private Coroutine flashRoutine;

    private void Awake()
    {
        // Create a visible flash without a collider.
        flashObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
        flashObject.name = "MuzzleFlashVisual";
        flashObject.transform.SetParent(transform, false);

        // Place the flash slightly beyond the muzzle point.
        flashObject.transform.localPosition = new Vector3(0f, 0f, 0.15f);
        flashObject.transform.localScale =
            new Vector3(flashSize, flashSize, 1f);

        Collider col = flashObject.GetComponent<Collider>();
        if (col != null)
            Destroy(col);

        // Use an unlit shader so the flash remains bright.
        Shader shader = Shader.Find("Universal Render Pipeline/Unlit");

        if (shader == null)
            shader = Shader.Find("Unlit/Color");

        if (shader != null)
        {
            flashMaterial = new Material(shader);
            flashMaterial.color = flashColor;

            Renderer flashRenderer =
                flashObject.GetComponent<Renderer>();

            flashRenderer.material = flashMaterial;
            flashRenderer.shadowCastingMode =
                UnityEngine.Rendering.ShadowCastingMode.Off;
            flashRenderer.receiveShadows = false;
        }

        flashObject.SetActive(false);
    }

    public void PlayFlash()
    {
        if (flashObject == null)
            return;

        if (playerCamera == null)
        {
            Debug.LogWarning(
                "MuzzleFlashEffect: Player Camera is not assigned."
            );
        }

        if (flashRoutine != null)
            StopCoroutine(flashRoutine);

        flashRoutine = StartCoroutine(ShowFlash());
    }

    private IEnumerator ShowFlash()
    {
        flashObject.SetActive(true);

        float elapsed = 0f;

        while (elapsed < flashDuration)
        {
            if (playerCamera != null)
            {
                // Face the camera so the flash is visible to the player.
                flashObject.transform.rotation =
                    playerCamera.transform.rotation;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        flashObject.SetActive(false);
        flashRoutine = null;
    }

    private void OnDestroy()
    {
        if (flashMaterial != null)
            Destroy(flashMaterial);
    }
}
