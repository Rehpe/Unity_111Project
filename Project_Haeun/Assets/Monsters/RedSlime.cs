using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedSlime : Slime
{
    // Start is called before the first frame update
    void Start()
    {
        maxHp = 500f;
        attackDamage = 100f;
        attackSpeed = 1f;

        currentHp = maxHp;
        attackTimer = attackSpeed;
        hpBar.SetMaxHp(maxHp);
        hpBar.SetHp(currentHp);
        Debug.Log("HpBar 세팅 완료");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
