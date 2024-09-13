using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBlade : Weapon
{
    [SerializeField]
    private float radius = 30f;
    [SerializeField]
    private float rotateSpeed = 300f;
    [SerializeField]
    protected float attackDamage = 50f;
    private float rotateAngle = 0f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(isActivate)
        {
            Rotate();
        }
    }

    private void Rotate()
    {
        rotateAngle += rotateSpeed * Time.deltaTime;
        if(rotateAngle >= 360f)
            rotateAngle -= 360f;
        
        float x = Mathf.Cos(rotateAngle * Mathf.Deg2Rad) * radius;
        float y = Mathf.Sin(rotateAngle * Mathf.Deg2Rad) * radius;
        
        transform.localPosition = new Vector2(x,y);
    }

     void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Slime"))
        {
            Slime slime = other.gameObject.GetComponent<Slime>();
            if (slime)
            {
                slime.TakeDamage(attackDamage);
            }
        }
    }

     void OnCollisionExit2D(Collision2D other)
    {
       
    }

}
