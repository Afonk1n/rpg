using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroStats : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public Image imgHealth;

    public int level;
    public float exp;
    public float curExp;
    public Image imgExp;

    public void Start()
    {
        exp = 100 * level;
        InterfaceUpdate();
    }

    public void AddHealth(int add)
    {
        health += add;

        health = Mathf.Clamp(health, 0, maxHealth);
        InterfaceUpdate();
    }

    public void AddExp(int add)
    {
        curExp += add;

        if(curExp == exp)
        {
            level++;
            exp = 100 * level;
            curExp = 0;
        }
        else if(curExp >= exp)
        {
            curExp -= exp;
            level++;
            exp = 100 * level;
        }
        InterfaceUpdate();
    }

    
    public void InterfaceUpdate()
    {
        float timeHealth = health / maxHealth * 100;
        imgHealth.fillAmount = timeHealth / 100;

        float timeExp = curExp / exp * 100;
        imgExp.fillAmount = timeExp / 100;
    }
    
}
