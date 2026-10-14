
using System.Collections;
using UnityEngine;

public class BulletImpactEffect : MonoBehaviour
{
    [Header("Effect Settings")]
    [SerializeField] private Color impactColor = Color.red;
    [SerializeField] private float effectDuration = 0.12f;
    [SerializeField] private float surfaceOffset = 0.015f;

    public void PlayEffect(Vector3 position, Vector3 normal)
    {
        GameObject flash = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        flash.name = "BulletImpactFlash";

        // Keep the effect just above the surface.
        flash.transform.position = position + normal * surfaceOffset;
        flash.transform.localScale = Vector3.one * 0.12f;

        Collider effectCollider = flash.GetComponent<Collider>();
        if (effectCollider != null)
            Destroy(effectCollider);

        Renderer effectRenderer = flash.GetComponent<Renderer>();
        Material material = new Material(Shader.Find("Universal Render Pipeline/Unlit"));

        if (material.shader == null)
            material.shader = Shader.Find("Unlit/Color");

        material.color = impactColor;
        effectRenderer.material = material;

        Destroy(material, effectDuration + 0.1f);
        Destroy(flash, effectDuration);
    }
}
