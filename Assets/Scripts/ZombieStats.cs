using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieStats : CharacterStats
{
    [SerializeField] private int damage;
    public float attackSpeed;

    [SerializeField] private bool canAttack;

    private void Start()
    {
        InitVariables();
    }

    public void DealDamage(CharacterStats statsToDamage)
    {
        // опхлемемхе спнмю
        statsToDamage.TakeDamage(damage);
    }

    public override void Die()
    {
        // ялепрэ
        base.Die();
        Destroy(gameObject);
    }

    public override void InitVariables()
    {
        // оепелеммше
        maxHealth = 100;
        SetHealthTo(maxHealth);
        isDead = false;

        damage = 10;
        attackSpeed = 1.5f;
        canAttack = true;
    }
}
