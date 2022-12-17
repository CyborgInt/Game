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
        if(Input.GetKey(KeyCode.Mouse0))    // ÂÛÑÒĞÅË
        {
            Shoot();    
        }

        if(Input.GetKeyDown(KeyCode.R))     // ÏÅĞÅÇÀĞßÄÊÀ
        {
            Reload(manager.currentlyEquippedWeapon);
        }
    }

    private void RaycastShoot(Weapon currentWeapon)
    {
        /// ÔÓÍÊÖÈß ĞÅÀËÈÇÓŞÙÀß ËÓ×-ÒĞÀÅÊÒÎĞÈŞ Ê ÒÎ×ÊÅ, 
        /// ÏÎ ÊÎÒÎĞÎÉ ÁÓÄÅÒ ÏÅĞÅÌÅÙÀÒÜÑß ÏÓËß.
        
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        float currentWeaponRange = currentWeapon.range;

        if (Physics.Raycast(ray, out hit, currentWeaponRange))
        {
            Debug.Log(hit.transform.name);
        }

        Instantiate(currentWeapon.muzzleFlashParticles, manager.currentWeaponBarrel);
    }

    private void Shoot()
    {   /// ÌÅÒÎÄ ÂÛÑÒĞÅËÀ, ÏĞÎÂÅĞßÅÒ ÂÎÇÌÎÆÍÎÑÒÜ ÂÛÑÒĞÅËÀ È ÂÎÇÌÎÆÍÎÑÒÜ ÏÅĞÅÇÀĞßÄÊÈ
        /// ÏÎÑËÅ ÏÎËÓ×ÀÅÒ ÎĞÓÆÈÅ, ÇÀÏÈÑÛÂÀÅÒ ÂĞÅÌß ÂÛÑÒĞÅËÀ Â ËÎÃÈ, ÂÛÇÛÂÀÅÒ ÌÅÒÎÄÛ
        /// ÃÅÍÅĞÀÖÈÈ ÒĞÀÅÊÒÎĞÈÈ È ÎĞÓÆÈß
        
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
            }
        }
        else
            Debug.Log("not enough ammo in magazine");
    }

    private void UseAmmo(int slot, int currentAmmoUsed, int currentStoredAmmoUsed)
    {
        /// ÌÅÒÎÄ ÄËß ÂÛÁÎĞÀ ÎĞÓÆÈß
        
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
        /// ÌÅÒÎÄ ÄÎÁÀÂËÅÍÈß ÎĞÓÆÈß Â ÀÊÒÈÂÍÛÉ ÑËÎÒ "ÈÍÂÅÍÒÀĞß"
        /// ÏÅĞÅÄÀ¨Ò ÄÀÍÍÛÅ Â "ÕÓÄ"
        
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
        /// ÌÅÒÎÄ ÄËß ÏÅĞÅÇÀĞßÄÊÈ, ÏĞÎÂÅĞßŞÙÈÉ Å¨ ÂÎÇÌÎÆÍÎÑÒÜ
        /// È ÈÑÏÎËÍßÅÒ ÏÅĞÅÇÀĞßÄÊÓ ÄËß ÊÎÍÊĞÅÒÍÎÃÎ ÎĞÓÆÈß
        
        if (canReload)
        {
            //primary
            if (slot == 0)
            {
                int ammoToReload = inventory.GetItem(0).magazineSize - primaryCurrentAmmo;

                // åñëè ó íàñ áóäåò äîñòàòî÷íî ïàòğîíîâ, ÷òîáû ïåğåçàğÿäèòü íàø ìàãàçèí
                if (primaryCurrentAmmoStorage >= ammoToReload)
                {
                    // åñëè òåêóùèé ìàãàçèí ïîëîí
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

                // åñëè ó íàñ áóäåò äîñòàòî÷íî ïàòğîíîâ, ÷òîáû ïåğåçàğÿäèòü íàø ìàãàçèí
                if (secondaryCurrentAmmoStorage >= ammoToReload)
                {
                    // åñëè òåêóùèé ìàãàçèí ïîëîí
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
        /// ÌÅÒÎÄ ÄËß ÏĞÎÂÅĞÊÈ ÂÎÇÌÎÆÍÎÑÒÈ ÂÛÑÒĞÅËÀ
        /// ÏĞÎÂÅĞßÅÒ ÂÛÁĞÀÍÍÎÅ ÎĞÓÆÈÅ È ÂÅËÈ×ÈÍÓ ÅÃÎ ÏÀÒĞÎÍÎÂ
        
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
        /// ÌÅÒÎÄ ÈÍÈÖÈÀËÈÇÀÖÈÈ ÎĞÓÆÈß È ÏÀĞÀÌÅÒĞÎÂ

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

    private void GetReferences()
    {
        cam = GetComponentInChildren<Camera>();
        inventory = GetComponent<Inventory>();
        manager = GetComponent<EquipmentManager>();
        anim = GetComponentInChildren<Animator>();
        hud = GetComponent<PlayerHUD>();
    }
}
