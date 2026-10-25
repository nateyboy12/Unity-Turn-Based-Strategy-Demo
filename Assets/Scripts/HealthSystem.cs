using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDead;
    public event EventHandler OnDamaged;

    [SerializeField] private int health = 100;
    private int healthMax;

    private void Awake()
    {
        healthMax = health;
    }

    public void Damage(int amount)
    {
        health -= amount;

        OnDamaged?.Invoke(this, EventArgs.Empty); // You could pass the damage amount as an arg

        if (health <= 0)
        {
            health = 0;
            Die();
        }

        // Debug.Log(health);
    }

    private void Die()
    {
        OnDead.Invoke(this, EventArgs.Empty);
    }

    public float getHealthNormalized()
    {
        return (float)health / healthMax;
    }
}

