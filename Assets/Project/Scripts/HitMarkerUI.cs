
using System.Collections;
using UnityEngine;
using TMPro;

public class HitMarkerUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WeaponShooting weaponShooting;
    [SerializeField] private TMP_Text hitMarkerText;

    [Header("Settings")]
    [SerializeField] private float visibleDuration = 0.15f;

    private Coroutine hideCoroutine;

    private void Awake()
    {
        if (hitMarkerText == null)
            hitMarkerText = GetComponent<TMP_Text>();

        if (hitMarkerText != null)
            hitMarkerText.enabled = false;
    }

    private void OnEnable()
    {
        if (weaponShooting != null)
            weaponShooting.OnTargetHit += ShowHitMarker;
    }

    private void OnDisable()
    {
        if (weaponShooting != null)
            weaponShooting.OnTargetHit -= ShowHitMarker;
    }

    private void ShowHitMarker()
    {
        if (hitMarkerText == null)
            return;

        hitMarkerText.enabled = true;

        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        hideCoroutine = StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSecondsRealtime(visibleDuration);

        if (hitMarkerText != null)
            hitMarkerText.enabled = false;

        hideCoroutine = null;
    }
}
