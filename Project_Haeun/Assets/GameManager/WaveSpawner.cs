using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    // State
    public enum WaveState
    {
        Block,          // 멈춤
        WaveSpawn,      // 일반 슬라임 + 젬
        BuffSpawn,      // 버프
        StoreSpawn,     // 상점
        MidBossSpawn,   // 중간보스
        BossSpawn,      // 보스
    }
    public WaveState currentWaveState;
    public void ChangeWaveState(WaveState newState) { currentWaveState = newState;}
    public WaveState GetCurrentWaveState() { return currentWaveState;}

    // Objects
    public GameObject[] slimes;  // 슬라임(0: Green, 1: Red, 2: Blue)
    public GameObject[] buffs;
    public GameObject gem;
    public Transform[] spawnPositions; // 5개의 고정된 라인

    // Timer
    public float spawnInterval = 0.5f;
    private float timer;
    private float buffInterval = 200f;
    private float midBossInterval = 500f;
    private float bossInterval = 1000f;
    private float spawnOffset = 10f;

    // Gem Spawn Chance
    [SerializeField, Header("Gem Spawn Chance")]
    private float gemSpawnChance = 0.2f;

    private float moveDistance;
    private bool isBossSpawned = false;

    void Start()
    {
        ChangeWaveState(WaveState.WaveSpawn);
    }

    // Update is called once per frame
    void Update()
    {
        // Player 이동거리 갱신
        moveDistance = GameManager.Instance.GetMoveDistance();

        // Blocking 상태일 때에는 스폰 타이머 off
        switch(currentWaveState)
        {
            case WaveState.Block:
                return;
            case WaveState.WaveSpawn:
            {
                timer += Time.deltaTime;
                if (timer >= spawnInterval)
                {
                    SpawnWave();
                    timer = 0;
                }
            }
                break;
            case WaveState.BuffSpawn:
                break;
            case WaveState.StoreSpawn:
                break;
            case WaveState.MidBossSpawn:
                break;
            case WaveState.BossSpawn:
                break;
        }
        
        // 이동거리에 따른 상태 전환
        SwitchStateByDistance();
    }

    private void SpawnWave()
    {
        bool[] occupiedPositions = new bool[spawnPositions.Length];
        
        // 이동 거리에 따라 스폰할 슬라임 개수 지정
        int slimeCount = CalculateSlimeCount();

        // 가능한 자리 인덱스를 리스트로 저장
        List<int> availableIndex = new List<int>();
        for (int i = 0; i < spawnPositions.Length; i++)
        {
            availableIndex.Add(i);
        }

        // 슬라임 스폰
        for (int i = 0; i < slimeCount && availableIndex.Count > 0; i++)
        {
            // 랜덤으로 인덱스 선택
            int randomIndex = Random.Range(0, availableIndex.Count);
            int spawnIndex = availableIndex[randomIndex];
            
            // 슬라임 스폰 후 해당 인덱스 제거
            SpawnAtPosition(slimes[0],spawnPositions[spawnIndex]);
            occupiedPositions[spawnIndex] = true;
            availableIndex.RemoveAt(randomIndex);
        }

        // 빈 칸에 gem 스폰
        for (int i = 0; i < spawnPositions.Length; i++)
        {
            if (!occupiedPositions[i])
            {
                // 20% 확률로 gem 생성
                float randomChance = Random.Range(0f, 1f);
                if (randomChance <= gemSpawnChance)
                {
                    SpawnAtPosition(gem,spawnPositions[i]);
                    occupiedPositions[i] = true; // Gem이 스폰되면 해당 칸도 차지한 것으로 처리
                }
            }
        }
    }

    private void SpawnAtPosition(GameObject objects, Transform spawnPosition)
    {
        Instantiate(objects, spawnPosition.position, Quaternion.identity);
    }

    private int CalculateSlimeCount()
    {
        float distance = GameManager.Instance.GetMoveDistance();

        // 이동 거리가 짧으면 슬라임이 적게 등장
        if (distance < 200f)
        {
            return Random.Range(0, 3);  // 0~2개 등장
        }
        else if (distance < 500f)
        {
            return Random.Range(1, 4);  // 1~3개 등장
        }
        else
        {
            return Random.Range(3, 6);  // 3~5개 등장
        }
    }

    public void SpawnBuffs()
    {
        int randomIndex1 = Random.Range(1, buffs.Length);
        int randomIndex2 = Random.Range(1, buffs.Length);
            
        // Buff    
        Instantiate(buffs[randomIndex1], spawnPositions[1].position + new Vector3(-35f, 0f, 0f), Quaternion.identity);
        Instantiate(buffs[randomIndex2], spawnPositions[3].position + new Vector3(35f, 0f, 0f), Quaternion.identity);

        // Block
        Instantiate(buffs[0], spawnPositions[2].position, Quaternion.identity);

    }

    public void SpawnMidBoss()
    {
        Instantiate(slimes[1], spawnPositions[2].position, Quaternion.identity);
    }

    public void SpawnBoss()
    {
        Instantiate(slimes[2], spawnPositions[2].position, Quaternion.identity);
    }

    public void SwitchStateByDistance()
    {
        // Boss
        if(moveDistance >= bossInterval)
        {
            if(currentWaveState != WaveState.BossSpawn)
            {
                ChangeWaveState(WaveState.BossSpawn);
                SpawnBoss();
                return;
            }
        }
        // MidBoss
        else if (midBossInterval - spawnOffset <= moveDistance && moveDistance < midBossInterval + spawnOffset )
        {
            if(currentWaveState != WaveState.MidBossSpawn)
            {
                ChangeWaveState(WaveState.MidBossSpawn);
                SpawnMidBoss();
                return;
            }
        }
        else if ( 10f < moveDistance && 0<= (moveDistance % buffInterval) && (moveDistance % buffInterval) <= 10f)
        {
            //200~210, 400~410, 600~610, 800~810
            if(currentWaveState != WaveState.BuffSpawn)
            {
                ChangeWaveState(WaveState.BuffSpawn);
                SpawnBuffs();
                return;
            }
        }
        else
        {
            ChangeWaveState(WaveState.WaveSpawn);
        }
    }
}
