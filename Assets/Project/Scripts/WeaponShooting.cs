
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponShooting : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform muzzlePoint;
    [SerializeField] private MuzzleFlashEffect muzzleFlash;

    [Header("Shooting Settings")]
    [SerializeField] private float damage = 25f;
    [SerializeField] private float range = 100f;
    [SerializeField] private float fireRate = 6f;

    [Header("Ammo Settings")]
    [SerializeField] private int magazineSize = 30;
    [SerializeField] private int startingReserveAmmo = 90;
    [SerializeField] private float reloadDuration = 1.5f;

    [Header("Impact Effect")]
    [SerializeField] private BulletImpactEffect impactEffect;

    private int currentAmmo;
    private int reserveAmmo;
    private float nextFireTime;
    private bool isReloading;

    public int CurrentAmmo => currentAmmo;
    public int ReserveAmmo => reserveAmmo;
    public int MagazineSize => magazineSize;
    public bool IsReloading => isReloading;

    public event Action OnTargetHit;

    private void Awake()
    {
        currentAmmo = magazineSize;
        reserveAmmo = startingReserveAmmo;
    }

    private void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.rKey.wasPressedThisFrame)
        {
            TryReload();
        }

        if (isReloading || Mouse.current == null)
            return;

        if (Mouse.current.leftButton.isPressed &&
            Time.time >= nextFireTime &&
            currentAmmo > 0)
        {
            currentAmmo--;
            nextFireTime = Time.time +
                1f / Mathf.Max(fireRate, 0.01f);

            Shoot();
            Debug.Log($"Ammo: {currentAmmo}/{reserveAmmo}");
        }
    }

    private void TryReload()
    {
        if (isReloading ||
            currentAmmo >= magazineSize ||
            reserveAmmo <= 0)
            return;

        StartCoroutine(Reload());
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        yield return new WaitForSeconds(reloadDuration);

        int ammoNeeded = magazineSize - currentAmmo;
        int ammoToLoad = Mathf.Min(ammoNeeded, reserveAmmo);

        currentAmmo += ammoToLoad;
        reserveAmmo -= ammoToLoad;
        isReloading = false;

        Debug.Log($"Reloaded: {currentAmmo}/{reserveAmmo}");
    }

    private void Shoot()
    {
        // Show muzzle flash every time a round is fired.
        if (muzzleFlash != null)
            muzzleFlash.PlayFlash();

        if (playerCamera == null)
        {
            Debug.LogError("Player Camera is not assigned!");
            return;
        }

        Ray ray = playerCamera.ViewportPointToRay(
            new Vector3(0.5f, 0.5f, 0f)
        );

        if (Physics.Raycast(ray, out RaycastHit hit, range))
        {
            if (impactEffect != null)
                impactEffect.PlayEffect(hit.point, hit.normal);

            if (muzzlePoint != null)
            {
                Debug.DrawLine(
                    muzzlePoint.position,
                    hit.point,
                    Color.red,
                    1f
                );
            }

            TargetHealth target =
                hit.collider.GetComponentInParent<TargetHealth>();

            if (target != null)
            {
                OnTargetHit?.Invoke();
                target.TakeDamage(damage);
            }
            else
            {
                Debug.Log($"Hit: {hit.collider.name}");
            }
        }
        else if (muzzlePoint != null)
        {
            Debug.DrawRay(
                ray.origin,
                ray.direction * range,
                Color.yellow,
                1f
            );
        }
    }
}
