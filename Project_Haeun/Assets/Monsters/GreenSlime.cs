using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenSlime : Slime
{
    // Start is called before the first frame update
    void Start()
    {
        maxHp = 100f;
        attackDamage = 50f;
        attackSpeed = 0.5f;

        currentHp = maxHp;
        attackTimer = attackSpeed;
        hpBar.SetMaxHp(maxHp);
        hpBar.SetHp(currentHp);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}