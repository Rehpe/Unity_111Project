using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDamageBuff : Buff
{
    [SerializeField]
    private float increaseDamage = 50f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Execute()
    {
        Player player = FindObjectOfType<Player>();

        if(player)
        {
            player.BuffAttackDamage(increaseDamage);
            Debug.Log("딜 버프");
        }
    }
}
