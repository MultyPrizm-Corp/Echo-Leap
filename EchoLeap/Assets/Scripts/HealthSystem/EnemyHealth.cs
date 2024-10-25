using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IHealth
{
    [Header("General")]
    [SerializeField] private int health = 10;
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private bool block = false;
    [SerializeField] private bool dash = false;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private string hitAnimation = null;
    [SerializeField] private string deathAnimation = null;

    [Header("Special")]
    [SerializeField] private HealthSlider healthSlider;

    private void Start()
    {
        if (healthSlider != null)
        {
            healthSlider.SetMaxHealth(maxHealth);
            healthSlider.SetHealth(health);   
        }
    }

    public void Damage(int damage, string element, bool ignoreBlock = false)
    {
        if(!block && !ignoreBlock)
        {
            if(health - damage < 0)
            {
                health = 0;
            }
            else
            {
                health -= damage;
            }

            if(hitAnimation != null && animator != null)
            {
                animator.SetBool(hitAnimation, true);
            }

            if(healthSlider != null)
            {
                healthSlider.SetHealth(health);
            }
        }

        if(health <= 0)
        {
            if(deathAnimation != null && animator != null)
            {
                animator.SetBool(deathAnimation, true);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public void InstantlyKill()
    {
        if (deathAnimation != null)
        {
            animator.SetBool(deathAnimation, true);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int GetHealth()
    {
        return health;
    }
}
