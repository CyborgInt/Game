using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    /// яйпхор нрбевючыхи гю янрнъмхъ х ярюрш хцпнйю


    [SerializeField] protected int health;
    [SerializeField] protected int maxHealth;

    [SerializeField] protected bool isDead;

    public void Start()
    {
        InitVariables();
    }

    public virtual void CheckHealth()
    {
        /// лернд опнбепъчыхи гднпнбэе, 
        /// х бшгшбючыхи онанвмше лерндш

        if(health <= 0)
        {
            health = 0;
            Die();
        }
        if(health >= maxHealth)
        {
            health = maxHealth;
        }
    }

    public virtual void Die()
    {
        isDead = true;
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void SetHealthTo(int healthToSetTo)
    {
        /// лернд бшгшбючыхи опнбепйс гднпнбэъ

        health = healthToSetTo;
        CheckHealth();
    }

    public void TakeDamage(int damage)
    {
        /// лернд онксвемхъ спнмю

        int healthAfterDamage = health - damage;
        SetHealthTo(healthAfterDamage);
    }

    public void Heal(int heal)
    {
        /// лернд бняярюмнбкемхъ гднпнбэъ

        int healthAfterHeal = health + heal;
        SetHealthTo(healthAfterHeal);
    }

    public virtual void InitVariables()
    {
        maxHealth = 100;
        SetHealthTo(maxHealth);
        isDead = false;
    }
}
