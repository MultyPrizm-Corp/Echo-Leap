using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    public void Damage(int damage, string element, bool ignoreBlock = false);

    public void InstantlyKill();

    public int GetHealth();
}

public class PlayerHealthSlider : MonoBehaviour, IHealth
{
    [SerializeField] private int health;
    [SerializeField] private int maxHealth;
    [SerializeField] private bool block;
    [SerializeField] private bool dash;

    public void Damage(int damage, string element, bool ignoreBlock = false)
    {

    }

    public void InstantlyKill()
    {

    }

    public int GetHealth()
    {
        return health;
    }
}
