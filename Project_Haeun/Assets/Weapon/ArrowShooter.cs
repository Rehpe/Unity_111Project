using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShooter : Weapon
{
    public GameObject arrowPrefab;

    [SerializeField]
    private float shootInterval = 3f;
    [SerializeField]
    private float arrowSpeed = 200f;
    [SerializeField]
    private float arrowDamage = 50f;
    
    private float shootTimer;

    // Start is called before the first frame update
    void Start()
    {
        shootTimer = shootInterval;
    }

    // Update is called once per frame
    void Update()
    {
        if(isActivate)
        {
            ShootArrow();
        }
    }

    private void ShootArrow()
    {
        shootTimer -= Time.deltaTime;

        if(shootTimer <= 0)
        {
            GameObject newArrow = Instantiate(arrowPrefab,transform.position, Quaternion.identity);
            Arrow arrow = newArrow.GetComponent<Arrow>();
            if(arrow)
            {
                arrow.SettingArrow(arrowSpeed, arrowDamage);
            }
            shootTimer = shootInterval;
        }
    }
    
}
