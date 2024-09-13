using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private float maxLength;
    private float maxWidth;
    private float laserExtendSpeed;
    private float activeDuration;
    private float damageInterval; 
    private float laserDamage;

    private float damageTimer = 0f;
    private List<GameObject> HittedSlimes = new List<GameObject>(); // 충돌 중인 슬라임

    private Vector3 initialScale;
    private Vector3 targetScale;
    private bool isMaxSizeReached = false;

    void Start()
    {
        initialScale = transform.localScale;
        targetScale = new Vector3(maxLength, maxWidth, initialScale.z);
        StartCoroutine(LaserLifecycle());
    }

    private void Update()
    {
        // 데미지 간격 타이머 관리
        damageTimer += Time.deltaTime;
        if (damageTimer >= damageInterval)
        {
            ApplyLaserDamage();
            damageTimer = 0f;
        }
    }

    public void SettingLaser(float newMaxLength,float newMaxWidth,float newLaserExtendSpeed,float newActiveDuration,float newDamageInterval, float newLaserDamage)
    {
        maxLength = newMaxLength;
        maxWidth = newMaxWidth;
        laserExtendSpeed = newLaserExtendSpeed;
        activeDuration = newActiveDuration;
        damageInterval = newDamageInterval; 
        laserDamage = newLaserDamage;
    }
    
    private IEnumerator LaserLifecycle()
    {
        // 레이저가 길어짐
        while (!isMaxSizeReached)
        {
            ExtendLaser();
            yield return null; // 매 프레임마다 체크
        }

        // 레이저가 최대 크기에 도달한 후 일정 시간 동안 유지
        yield return new WaitForSeconds(activeDuration);

        // 레이저가 줄어듦
        while (transform.localScale.x > initialScale.x)
        {
            ShrinkLaser();
            yield return null; // 매 프레임마다 체크
        }

        // 레이저가 파괴됨
        Destroy(gameObject);
    }

    private void ExtendLaser()
    {
        // 레이저가 전개되는 속도에 따라 크기를 늘림
        transform.localScale = Vector3.MoveTowards(transform.localScale, targetScale, laserExtendSpeed * Time.deltaTime);

        // 레이저가 최대 크기에 도달했는지 체크
        if (transform.localScale == targetScale)
        {
            isMaxSizeReached = true;
        }
    }

     private void ShrinkLaser()
    {
        // 레이저가 줄어드는 속도에 따라 크기를 줄임
        transform.localScale = Vector3.MoveTowards(transform.localScale, initialScale, laserExtendSpeed * Time.deltaTime);
    }

    private void ApplyLaserDamage()
    {
        foreach (GameObject slime in HittedSlimes)
        {
            if (slime)
            {
                Slime slimeScript = slime.GetComponent<Slime>();
                if (slimeScript)
                {
                    slimeScript.TakeDamage(laserDamage);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Slime"))
        {
            if (!HittedSlimes.Contains(other.gameObject))
            {
                HittedSlimes.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Slime"))
        {
            HittedSlimes.Remove(other.gameObject);
        }
    }
}
