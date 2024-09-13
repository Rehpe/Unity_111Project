using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserShooter : Weapon
{
    public GameObject laserPrefab;

    [SerializeField]
    private float laserShootInterval = 5f;
    private float laserTimer;

    [SerializeField, Header("Laser")]
    private float maxLength = 6f; // 레이저 최대 길이
    [SerializeField]
    private float maxWidth = 2f; // 레이저 최대 너비
    [SerializeField]
    private float laserExtendSpeed = 50f;   // 레이저 확장 속도
    [SerializeField]
    private float activeDuration = 1.5f; // 레이저 유지되는 시간
    [SerializeField]
    private float damageInterval = 0.3f; // 데미지 간격
    [SerializeField]
    private float laserDamage = 10f;


    // Start is called before the first frame update
    void Start()
    {
        laserTimer = laserShootInterval;
    }

    // Update is called once per frame
    void Update()
    {
        if(isActivate)
        {
            ShootLaser();
        }
    }

    private void ShootLaser()
    {
        laserTimer -= Time.deltaTime;

        if(laserTimer <= 0)
        {
            GameObject newLaser = Instantiate(laserPrefab,transform.position, laserPrefab.transform.rotation);
            newLaser.transform.parent = transform;
            Laser laser = newLaser.GetComponent<Laser>();
            if(laser)
            {
                laser.SettingLaser(maxLength, maxWidth,laserExtendSpeed,activeDuration,damageInterval,laserDamage);
            }
            laserTimer = laserShootInterval;
        }
    }
}
