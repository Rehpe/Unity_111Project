using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // 싱글톤 패턴으로 GameManager의 인스턴스 만들기
    public static GameManager Instance { get; private set; }

    public WaveSpawner waveSpawner;
    public Slider distanceSlider;
    public TextMeshProUGUI distanceText;

    public float moveDistance = 0f; // 플레이어 이동 거리
    private bool isBlocking = false; // 슬라임과 전투 중인지 여부
    public WaveSpawner.WaveState prevWaveState;

    public float moveSpeed = 1500f;
    public float currentMoveSpeed;

    public int collectedGems;
    public TextMeshProUGUI gemCountText;

    //무기 관련
    private int itemPrice = 5;
    public bool isOwningBlade = false;
    public bool isOwningArrow = false;
    public bool isOwningLaser = false;

    private bool isPaused = false;
    
    // 패널
    public GameObject shop;
    public GameObject gameOverPanel;
    public GameObject gameClearPanel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
      
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Init(); // 초기화 함수 호출
    }
    
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // 이벤트 해제
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        if(!isBlocking)
        {
            CaculateDistance();
        }
    }
    
    public void Init()
    {
        currentMoveSpeed = moveSpeed;
        moveDistance = 0;
        distanceSlider.value = 0f;
        DisableAllWeapons();
        collectedGems = 0;
        isBlocking = false;
        waveSpawner = GameObject.Find("WaveSpawner").GetComponent<WaveSpawner>();

        gameOverPanel = GameObject.Find("UIPanel/GameOverPanel");
        shop = GameObject.Find("UIPanel/Shop");
        gameClearPanel = GameObject.Find("UIPanel/GameClearPanel");

        shop.SetActive(false);
        gameOverPanel.SetActive(false);
        gameClearPanel.SetActive(false);

        gemCountText.text = collectedGems.ToString("F0");
        distanceText.text = moveDistance.ToString("F0") + "m";
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            Pause();
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f; // 게임 일시정지
        isPaused = true;
        // UI 띄움
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // 게임 재개
        isPaused = false;
        // UI 숨김
    }

    private void CaculateDistance()
    {
        moveDistance += currentMoveSpeed * Time.deltaTime * 0.01f;
        distanceSlider.value = moveDistance / 1000f;
        distanceText.text = moveDistance.ToString("F0") + "m";
    }

    public float GetMoveDistance()
    {
        return moveDistance;
    }

    public void SetBlocking(bool block)
    {
        // 상태가 변경될 때만 실행
        if (isBlocking != block)
        {
            isBlocking = block;

            if (isBlocking)
            {
                // Block 상태로 전환
                currentMoveSpeed = 0f;
                prevWaveState = waveSpawner.GetCurrentWaveState();
                waveSpawner.ChangeWaveState(WaveSpawner.WaveState.Block);
            }
            else
            {
                // Block 상태 해제 후 이전 상태로 복구
                currentMoveSpeed = moveSpeed;
                waveSpawner.ChangeWaveState(prevWaveState);
            }
        }
    }

    public bool GetBlocking()
    {
        return isBlocking;
    }

    public void GetGem()
    {
        collectedGems++;
        gemCountText.text = collectedGems.ToString("F0");
    }

    public void BuyBlade()
    {
        if(collectedGems < itemPrice) return;
        if(isOwningBlade) return;

        collectedGems -= itemPrice;
        if(!isOwningBlade)
        {
            isOwningBlade = true;
        }
        gemCountText.text = collectedGems.ToString("F0");
    }

    public void BuyArrow()
    {
        if(collectedGems < itemPrice) return;
        if(isOwningArrow) return;
        
        collectedGems -= itemPrice;
        if(!isOwningArrow)
        {
            isOwningArrow = true;
        }
        gemCountText.text = collectedGems.ToString("F0");
    }

    public void BuyLaser()
    {
        if(collectedGems < itemPrice) return;
        if(isOwningLaser) return;
        
        collectedGems -= itemPrice;
        if(!isOwningLaser)
        {
            isOwningLaser = true;
        }
        gemCountText.text = collectedGems.ToString("F0");
    }

    public void DisableAllWeapons()
    {
        isOwningBlade = false;
        isOwningArrow = false;
        isOwningLaser = false;
    }

    public void GameOver()
    {
        Pause();
        gameOverPanel.SetActive(true);
    }

        public void GameClear()
    {
        Pause();
        gameClearPanel.SetActive(true);
    }

    public void Restart()
    {
        ResumeGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
