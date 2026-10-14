
using TMPro;
using UnityEngine;

public class AmmoUI : MonoBehaviour
{
    [SerializeField] private WeaponShooting weapon;
    [SerializeField] private TMP_Text ammoText;

    private void Update()
    {
        if (weapon == null || ammoText == null)
            return;

        if (weapon.IsReloading)
        {
            ammoText.text = "RELOADING...";
        }
        else
        {
            ammoText.text =
                $"{weapon.CurrentAmmo} / {weapon.ReserveAmmo}";
        }
    }
}
