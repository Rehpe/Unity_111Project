using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueSlime : Slime
{
    public GameObject fireballPrefab; 
    public float spawnInterval = 0.3f;

    private bool isFiring = false;

    // Start is called before the first frame update
    void Start()
    {
        maxHp = 1000f;
        attackDamage = 100f;
        attackSpeed = 1f;

        currentHp = maxHp;
        attackTimer = attackSpeed;
        hpBar.SetMaxHp(maxHp);
        hpBar.SetHp(currentHp);

        StartFireRain();
    }

    // Update is called once per frame
    void Update()
    {
    }

     public void StartFireRain()
    {
        if (!isFiring)
        {
            StartCoroutine(FireRainRoutine());
        }
    }

       // 불꽃을 일정 시간마다 소환하는 코루틴
    private IEnumerator FireRainRoutine()
    {
        isFiring = true;

        while (isFiring)
        {
            // 랜덤한 위치에서 불꽃 소환
            float randomX = Random.Range(-500f, 500f);
            Vector3 spawnPosition = new Vector3(randomX, 1000f, 0f);
            Instantiate(fireballPrefab, spawnPosition, Quaternion.identity);

            yield return new WaitForSeconds(spawnInterval); // 불꽃이 떨어지는 간격
        }
    }

    // 보스가 죽으면 불꽃 소환 중단
    public void StopFireRain()
    {
        isFiring = false;
    }

    public void OnDeath()
    {
        GameManager.Instance.GameClear();
    }
}
