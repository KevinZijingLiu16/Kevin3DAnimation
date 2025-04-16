using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int health;

    public event Action OnTakeDamage;

    public event Action OnDie;

    private bool isInvulnerable = false;

    public bool IsDead => health == 0;



    // Start is called before the first frame update
    private void Start()
    {
        health = maxHealth; 
    }

    public void SetInvulnerable(bool isInvulnerable)
    {
        this.isInvulnerable = isInvulnerable;
    }

    public void DealDamage(int damage)
    {
        
        if (health == 0)
        {
            return;
        }

        if (isInvulnerable)
        {
            return;
        }

        health = Mathf.Max(health - damage, 0); //mathf.max is used to prevent health from going below 0 by return the largest value between 0 and health - damage

        OnTakeDamage?.Invoke();

        if (health == 0)
        {
            OnDie?.Invoke();
        }

        Debug.Log("Health: " + health);
    }

 

}
