using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpeedBuff : Buff
{
    [SerializeField]
    private float increaseAttackSpeed = 0.2f;
    
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
            player.BuffAttackSpeed(increaseAttackSpeed);
        }
    }
}
