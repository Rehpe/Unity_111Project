using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private float arrowSpeed;
    private float arrowDamage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GoUp();
        SelfDestroy();
    }

    public void SettingArrow(float newArrowSpeed, float newArrowDamage)
    {
        arrowSpeed = newArrowSpeed;
        arrowDamage = newArrowDamage;
    }

    private void GoUp()
    {
        transform.Translate(Vector2.up * arrowSpeed * Time.deltaTime);
    }

    private void SelfDestroy()
    {
        if(transform.position.y >= 1100f)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Slime"))
        {
            Slime slime = other.gameObject.GetComponent<Slime>();
            if (slime)
            {
                slime.TakeDamage(arrowDamage);
                Destroy(gameObject);
            }
        }
    }
}
