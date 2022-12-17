using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] private ProgressBar healthBar;
    [SerializeField] private WeaponUI weaponUI;

    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        // намнбкемхе бекхвхмш гднпнбэъ
        healthBar.SetValue(currentHealth, maxHealth);
    }

    public void UpdateWeaponUI(Weapon newWeapon)
    {
        // намнбкемхе лндекэйх нпсфхъ
        weaponUI.UpdateInfo(newWeapon.icon, newWeapon.magazineSize, newWeapon.storedAmmo);
    }

    public void UpdateWeaponAmmoUI(int currentAmmo, int storedAmmo)
    {
        // намнбкемхе хйнмйх нпсфхъ б "усд" оюмекх
        weaponUI.UpdateAmmoUI(currentAmmo, storedAmmo);
    }
}
