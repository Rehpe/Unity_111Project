using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealBuff : Buff
{
    [SerializeField]
    private float healAmount = 150f;
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
            player.Heal(healAmount);
        }
    }
}
