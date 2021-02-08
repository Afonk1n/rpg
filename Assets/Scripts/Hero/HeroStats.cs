using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroStats : MonoBehaviour
{
    public float health;
    public float maxHealth;

    public void AddHealth(int add)
    {
        health += add;

        health = Mathf.Clamp(health, 0, maxHealth);
    }
    
}
