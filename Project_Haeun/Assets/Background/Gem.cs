using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public RectTransform gemImagePos;
    public Canvas canvas;

    [SerializeField]
    private float rotateSpeed = 50f;
    [SerializeField]
    private float shrinkSpeed = 100f;
    [SerializeField]
    private float moveSpeed = 800f;

    private bool isCollected = false;

    private Vector3 targetPosition;
    // Start is called before the first frame update
    void Start()
    {
        gemImagePos = GameObject.Find("GemImage").GetComponent<RectTransform>();
        canvas = GameObject.Find("UIPanel").GetComponent<Canvas>();
        targetPosition = GetWorldPositionFromUI(gemImagePos);
    }

    // Update is called once per frame
    void Update()
    {
        if(isCollected)
        {
            // Gem 회전
            transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
            
            // 크기 줄이기
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, shrinkSpeed * Time.deltaTime);

            // 상단 UI로 이동
            transform.position = Vector3.Lerp(transform.position, gemImagePos.position, moveSpeed * Time.deltaTime);

            if (transform.localScale.magnitude < 0.1f)
            {
                Destroy(gameObject);
            }
        }
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
           GameManager.Instance.GetGem();
           isCollected = true;
        }
    }
    
    
    private Vector3 GetWorldPositionFromUI(RectTransform uiElement)
    {
         Vector3[] corners = new Vector3[4];
        uiElement.GetWorldCorners(corners);

        Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, corners[0]);
        Vector3 worldPos = canvas.worldCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, canvas.planeDistance));
        worldPos.z = 0f; 

        return worldPos;
    }
}
