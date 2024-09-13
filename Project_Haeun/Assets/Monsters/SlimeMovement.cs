using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMovement : MonoBehaviour
{
    float slimeSpeed;

    void Start()
    {
        slimeSpeed = GameManager.Instance.currentMoveSpeed; // 처음에는 기본 속
    }

    void Update()
    {
        slimeSpeed = GameManager.Instance.currentMoveSpeed;
        transform.position += Vector3.down * slimeSpeed * Time.deltaTime;

        DestroySlime();
    }

    private void DestroySlime()
    {
        if(transform.position.y <= -1100f)
        {
            Destroy(gameObject);
        }
    }
}
