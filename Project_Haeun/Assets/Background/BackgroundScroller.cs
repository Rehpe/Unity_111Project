using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    private float backgroundHeight;

    public Transform background1;  
    public Transform background2;  

    // Start is called before the first frame update
    void Start()
    {
        backgroundHeight = background1.GetComponent<SpriteRenderer>().bounds.size.y;
    }

    // Update is called once per frame
    void Update()
    {
        // 배경을 아래로 스크롤
        float backgroundSpeed = GameManager.Instance.currentMoveSpeed;
        if(backgroundSpeed > 0)
        {
            background1.position = Vector3.Lerp(background1.position, background1.position + Vector3.down * backgroundSpeed * Time.deltaTime, 1f);
            background2.position = Vector3.Lerp(background2.position, background2.position + Vector3.down * backgroundSpeed * Time.deltaTime, 1f);
        }
        
        // 첫 번째 배경이 화면 아래로 벗어나면 위로 이동
        if (background1.position.y <= -backgroundHeight)
        {
            RepositionBackground(background1);
        }

        // 두 번째 배경이 화면 아래로 벗어나면 위로 이동
        if (background2.position.y <= -backgroundHeight)
        {
            RepositionBackground(background2);
        }
    }

    void RepositionBackground(Transform background)
    {
        background.position += new Vector3(0, backgroundHeight * 2f, 0);
    }
}
