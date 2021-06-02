using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarScript : MonoBehaviour
{
    public float currentHP;
    public float maxHP = 10;
    public Image healthBar;

    private void Start()
    {
        currentHP = maxHP;
        healthBar.fillAmount = currentHP / maxHP;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "attack")
        {
            if (currentHP > 0)
            {
                currentHP -= 2;
                healthBar.fillAmount = currentHP / maxHP;
            }
        }
    }
}
