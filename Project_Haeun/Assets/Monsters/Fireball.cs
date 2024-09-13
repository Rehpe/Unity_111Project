using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float fallSpeed = 5f;
    public int damage = 20;

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        // 불꽃이 밑으로 떨어짐
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

        // 화면 아래로 벗어나면 삭제
        if (transform.position.y < -1100f)
        {
            Destroy(gameObject);
        }    
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 플레이어 체력 감소
            Player player = collision.gameObject.GetComponent<Player>();
            if (player)
            {
                player.TakeDamage(damage); // 플레이어에게 데미지 적용
            }

            Destroy(gameObject); // 불꽃 삭제
        }
    }
}
