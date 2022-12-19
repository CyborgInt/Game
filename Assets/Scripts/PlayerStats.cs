using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{

    private PlayerHUD hud;
    private UIManager ui;

    private void Start()
    {
        GetReferences();
        InitVariables();
    }

    private void GetReferences()
    {
        hud = GetComponent<PlayerHUD>();
        ui = GetComponent<UIManager>();
    }

    public override void CheckHealth()
    {
        /// лернд опнбепйх гднпнбэъ
        
        base.CheckHealth();
        hud.UpdateHealth(health, maxHealth);
    }

    public override void Die()
    {
        base.Die();
        ui.SetActiveHud(false);
    }

    private void Update()
    {
        /// реярнбши лернд дкъ онксвемхъ спнмю

        if(Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(10);
        }
    }
}
