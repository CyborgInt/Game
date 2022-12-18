using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShooting : MonoBehaviour
{
    private float lastShootTime = 0;

    [SerializeField] private bool canShoot = true;
    public bool canReload = true;

    [SerializeField] private int primaryCurrentAmmo;
    [SerializeField] private int primaryCurrentAmmoStorage;

    [SerializeField] private int secondaryCurrentAmmo;
    [SerializeField] private int secondaryCurrentAmmoStorage;

    [SerializeField] private bool primaryMagazineIsEmpty = false;
    [SerializeField] private bool secondaryMagazineIsEmpty = false;

    [SerializeField] private GameObject bloodPS = null;

    private Camera cam;
    private Inventory inventory;
    private EquipmentManager manager;
    private Animator anim;
    private PlayerHUD hud;

    private void Start()
    {
        GetReferences();
        canShoot = true;
        canReload = true;
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.Mouse0))    // ¬€—“–≈À
        {
            Shoot();    
        }

        if(Input.GetKeyDown(KeyCode.R))     // œ≈–≈«¿–ﬂƒ ¿
        {
            Reload(manager.currentlyEquippedWeapon);
        }
    }

    private void RaycastShoot(Weapon currentWeapon)
    {
        /// ‘”Õ ÷»ﬂ –≈¿À»«”ﬁŸ¿ﬂ À”◊-“–¿≈ “Œ–»ﬁ   “Œ◊ ≈, 
        /// œŒ  Œ“Œ–Œ… ¡”ƒ≈“ œ≈–≈Ã≈Ÿ¿“‹—ﬂ œ”Àﬂ.
        
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        float currentWeaponRange = currentWeapon.range;

        if (Physics.Raycast(ray, out hit, currentWeaponRange))
        {
            Debug.Log(hit.transform.name);
            if(hit.transform.tag == "Enemy")
            {
                CharacterStats enemyStats = hit.transform.GetComponent<CharacterStats>();
                enemyStats.TakeDamage(currentWeapon.damage);
                //Spawn particles
                SpawnBloodParticles(hit.point, hit.normal);
            }
        }
        if(inventory.GetItem(manager.currentlyEquippedWeapon).weaponType != WeaponType.Melee)
            Instantiate(currentWeapon.muzzleFlashParticles, manager.currentWeaponBarrel);
    }

    private void Shoot()
    {   /// Ã≈“Œƒ ¬€—“–≈À¿, œ–Œ¬≈–ﬂ≈“ ¬Œ«ÃŒ∆ÕŒ—“‹ ¬€—“–≈À¿ » ¬Œ«ÃŒ∆ÕŒ—“‹ œ≈–≈«¿–ﬂƒ »
        /// œŒ—À≈ œŒÀ”◊¿≈“ Œ–”∆»≈, «¿œ»—€¬¿≈“ ¬–≈Ãﬂ ¬€—“–≈À¿ ¬ ÀŒ√», ¬€«€¬¿≈“ Ã≈“Œƒ€
        /// √≈Õ≈–¿÷»» “–¿≈ “Œ–»» » Œ–”∆»ﬂ
        
        CheckCanShoot(manager.currentlyEquippedWeapon);

        if (canShoot && canReload)
        {
            Weapon currentWeapon = inventory.GetItem(manager.currentlyEquippedWeapon);

            if (Time.time > lastShootTime + currentWeapon.fireRate)
            {
                Debug.Log("Shoot");
                lastShootTime = Time.time;

                RaycastShoot(currentWeapon);
                UseAmmo((int)currentWeapon.weaponStyle, 1, 0);
                PlayShootAnimations();
            }
        }
        else
            Debug.Log("not enough ammo in magazine");
    }

    private void UseAmmo(int slot, int currentAmmoUsed, int currentStoredAmmoUsed)
    {
        /// Ã≈“Œƒ ƒÀﬂ ¬€¡Œ–¿ Œ–”∆»ﬂ
        
        //primary
        if (slot == 0)
        {
            if (primaryCurrentAmmo <= 0)
            {
                primaryMagazineIsEmpty = true;
                CheckCanShoot(manager.currentlyEquippedWeapon);
            }
            else
            {
                primaryCurrentAmmo -= currentAmmoUsed;
                primaryCurrentAmmoStorage -= currentStoredAmmoUsed;
                hud.UpdateWeaponAmmoUI(primaryCurrentAmmo, primaryCurrentAmmoStorage); 
            }
        }

        //secondary
        if (slot == 1)
        {
            if (secondaryCurrentAmmo <= 0)
            {
                secondaryMagazineIsEmpty = true;
                CheckCanShoot(manager.currentlyEquippedWeapon);
            }
            else
            {
                secondaryCurrentAmmo -= currentAmmoUsed;
                secondaryCurrentAmmoStorage -= currentStoredAmmoUsed;
                hud.UpdateWeaponAmmoUI(secondaryCurrentAmmo, secondaryCurrentAmmoStorage);
            }
        }
    }

    private void AddAmmo(int slot, int currentAmmoAdded, int currentStoredAmmoAdded)
    {
        /// Ã≈“Œƒ ƒŒ¡¿¬À≈Õ»ﬂ Œ–”∆»ﬂ ¬ ¿ “»¬Õ€… —ÀŒ“ "»Õ¬≈Õ“¿–ﬂ"
        /// œ≈–≈ƒ¿®“ ƒ¿ÕÕ€≈ ¬ "’”ƒ"
        
        //primary
        if (slot == 0)
        {
            primaryCurrentAmmo += currentAmmoAdded;
            primaryCurrentAmmoStorage += currentStoredAmmoAdded;
            hud.UpdateWeaponAmmoUI(primaryCurrentAmmo, primaryCurrentAmmoStorage);
        }

        //secondary
        if (slot == 1)
        {
            secondaryCurrentAmmo += currentAmmoAdded;
            secondaryCurrentAmmoStorage += currentStoredAmmoAdded;
            hud.UpdateWeaponAmmoUI(secondaryCurrentAmmo, secondaryCurrentAmmoStorage);
        }
    }

    private void Reload(int slot)
    {
        /// Ã≈“Œƒ ƒÀﬂ œ≈–≈«¿–ﬂƒ », œ–Œ¬≈–ﬂﬁŸ»… ≈® ¬Œ«ÃŒ∆ÕŒ—“‹
        /// » »—œŒÀÕﬂ≈“ œ≈–≈«¿–ﬂƒ ” ƒÀﬂ  ŒÕ –≈“ÕŒ√Œ Œ–”∆»ﬂ
        
        if (canReload)
        {
            //primary
            if (slot == 0)
            {
                int ammoToReload = inventory.GetItem(0).magazineSize - primaryCurrentAmmo;

                // ÂÒÎË Û Ì‡Ò ·Û‰ÂÚ ‰ÓÒÚ‡ÚÓ˜ÌÓ Ô‡ÚÓÌÓ‚, ˜ÚÓ·˚ ÔÂÂÁ‡ˇ‰ËÚ¸ Ì‡¯ Ï‡„‡ÁËÌ
                if (primaryCurrentAmmoStorage >= ammoToReload)
                {
                    // ÂÒÎË ÚÂÍÛ˘ËÈ Ï‡„‡ÁËÌ ÔÓÎÓÌ
                    if (primaryCurrentAmmo == inventory.GetItem(0).magazineSize)
                    {
                        Debug.Log("Magazine is already full.");
                        return;
                    }

                    AddAmmo(slot, ammoToReload, 0);
                    UseAmmo(slot, 0, ammoToReload);

                    primaryMagazineIsEmpty = false;
                    CheckCanShoot(slot);
                }
                else
                    Debug.Log("Not enough ammo to reload");
            }

            //secondary
            if (slot == 1)
            {
                int ammoToReload = inventory.GetItem(1).magazineSize - secondaryCurrentAmmo;

                // ÂÒÎË Û Ì‡Ò ·Û‰ÂÚ ‰ÓÒÚ‡ÚÓ˜ÌÓ Ô‡ÚÓÌÓ‚, ˜ÚÓ·˚ ÔÂÂÁ‡ˇ‰ËÚ¸ Ì‡¯ Ï‡„‡ÁËÌ
                if (secondaryCurrentAmmoStorage >= ammoToReload)
                {
                    // ÂÒÎË ÚÂÍÛ˘ËÈ Ï‡„‡ÁËÌ ÔÓÎÓÌ
                    if (secondaryCurrentAmmo == inventory.GetItem(1).magazineSize)
                    {
                        Debug.Log("Magazine is already full.");
                        return;
                    }

                    AddAmmo(slot, ammoToReload, 0);
                    UseAmmo(slot, 0, ammoToReload);

                    secondaryMagazineIsEmpty = false;
                    CheckCanShoot(slot);
                }
                else
                    Debug.Log("Not enough ammo to reload");
            }

            anim.SetTrigger("reload");
            manager.currentWeaponAnim.SetTrigger("reload");
        }
        else
            Debug.Log("Cant reload at the moment");
    }

    private void CheckCanShoot(int slot)
    {
        /// Ã≈“Œƒ ƒÀﬂ œ–Œ¬≈– » ¬Œ«ÃŒ∆ÕŒ—“» ¬€—“–≈À¿
        /// œ–Œ¬≈–ﬂ≈“ ¬€¡–¿ÕÕŒ≈ Œ–”∆»≈ » ¬≈À»◊»Õ” ≈√Œ œ¿“–ŒÕŒ¬
        
        //primary
        if(slot == 0)
        {
            if (primaryMagazineIsEmpty)
                canShoot = false;
            else
                canShoot = true;
        }

        //secondary
        if(slot == 1)
        {
            if (secondaryMagazineIsEmpty)
                canShoot = false;
            else
                canShoot = true;
        }
    }

    public void InitAmmo(int slot, Weapon weapon)
    {
        /// Ã≈“Œƒ »Õ»÷»¿À»«¿÷»» Œ–”∆»ﬂ » œ¿–¿Ã≈“–Œ¬

        //primary
        if (slot == 0)
        {
            primaryCurrentAmmo = weapon.magazineSize;
            primaryCurrentAmmoStorage = weapon.storedAmmo;
        }

        //secondary
        if (slot == 1)
        {
            secondaryCurrentAmmo = weapon.magazineSize;
            secondaryCurrentAmmoStorage = weapon.storedAmmo;
        }
    }

    private void PlayShootAnimations()
    {
        anim.SetTrigger("shoot");
        manager.currentWeaponAnim.SetTrigger("shoot");
    }

    private void SpawnBloodParticles(Vector3 position, Vector3 normal)
    {
        Instantiate(bloodPS, position, Quaternion.FromToRotation(Vector3.up, normal));
    }

    private void GetReferences()
    {
        cam = GetComponentInChildren<Camera>();
        inventory = GetComponent<Inventory>();
        manager = GetComponent<EquipmentManager>();
        anim = GetComponentInChildren<Animator>();
        hud = GetComponent<PlayerHUD>();
    }
}
