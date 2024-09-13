using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum PlayerState
    {
        Idle,
        Attack,
        Dead
    }

    private PlayerState currentState;

    public PlayerState GetPlayerState() { return currentState;}

    private Vector2 movement;
    private Animator playerAnimator;

    //기본 이동속도
    private float moveSpeed = 700f;

    //기본 공격속도
    private float attackSpeed = 0.5f;
    public void BuffAttackSpeed(float newAttackSpeed) { attackSpeed -= newAttackSpeed;}
    private float attackTimer;

    //기본 데미지
    private float attackDamage = 50f;
    public void BuffAttackDamage(float newAttackDamage) { attackDamage += newAttackDamage;}

    //체력
    private float maxHp = 300f;
    private float currentHp;
    public PlayerHpBar hpBar;
    private bool isDead = false;

    //버프 관련
    [SerializeField]
    private bool isNearlyBuffed = false;
    public void NearlyBuffedOn() { isNearlyBuffed = true;}
    public void NearlyBuffedOff() { isNearlyBuffed = false; Debug.Log("버프오프");}

    //무기
    public GameObject Blade;
    public GameObject Arrow;
    public GameObject Laser;

    //화면 경계
    private float minX, maxX;

    // Start is called before the first frame update
    void Start()
    {
       Init();
    }

    // Update is called once per frame
    void Update()
    {
        if(isDead) return;

         // Movement
        float moveInput = Input.GetAxis("Horizontal");
        float newPositionX = transform.position.x + (moveInput * moveSpeed * Time.deltaTime);
        float clampedX = Mathf.Clamp(newPositionX, minX, maxX);
        transform.position = new Vector3(clampedX, transform.position.y, 0);

        // Weapon Check
        if(GameManager.Instance.isOwningBlade)
        {
            Blade.SetActive(true);
        }
         if(GameManager.Instance.isOwningArrow)
        {
            Arrow.SetActive(true);
        }
         if(GameManager.Instance.isOwningLaser)
        {
            Laser.SetActive(true);
        }
    }

    private void Init()
    {
       playerAnimator = GetComponent<Animator>();
       currentHp = maxHp;
       hpBar.SetMaxHp(maxHp);
       hpBar.SetHp(currentHp);
       ChangePlayerState(PlayerState.Idle);

       moveSpeed = 700f;
       attackDamage = 50f;
       attackDamage = 50f;
       isNearlyBuffed = false;
       isDead = false;

       minX = -350f;
       maxX = 350f;

       DisableAllWeapons();
    }

    private void ChangePlayerState(PlayerState state) 
    { 
        if(isDead) return;

        currentState = state;

        switch (state)
        {
            case PlayerState.Idle:
            playerAnimator.CrossFade("Idle", 0.1f);
            break;
        case PlayerState.Attack:
            playerAnimator.CrossFade("Attack", 0.1f);  
            break;
        case PlayerState.Dead:
            playerAnimator.CrossFade("Dead", 0.1f, 0, 0);  
            isDead = true;
            break;
        }
    }


    void OnCollisionEnter2D(Collision2D other)
    {
        if(isDead) return;

        // 슬라임
        if (other.gameObject.CompareTag("Slime"))
        {
            GameManager.Instance.SetBlocking(true);
            ChangePlayerState(PlayerState.Attack);
        }
    }   

    void OnTriggerEnter2D(Collider2D other)
    {
        if(isDead) return;

        if (other.CompareTag("Buff"))
        {        
            //if(!isNearlyBuffed)
            //{
                //NearlyBuffedOn();
                
                Buff buff = other.GetComponent<Buff>();

                if(buff)
                {
                    buff.Execute();
                }
            //}   
        }
    }   


    void OnCollisionStay2D(Collision2D other)
    {
        if(isDead) return;

        // 슬라임
        if (other.gameObject.CompareTag("Slime"))
        {
           if (currentState == PlayerState.Attack)
            {
                Attack(other.gameObject.GetComponent<Slime>());
            }
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if(isDead) return;

        // 슬라임
        if (other.gameObject.CompareTag("Slime"))
        {
            GameManager.Instance.SetBlocking(false);
            ChangePlayerState(PlayerState.Idle);
            attackTimer = 0f;
        }
    }

    private void Attack(Slime slime)
    {
        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f)
        {
            slime.TakeDamage(attackDamage);  // 공격 실행
            Debug.Log("슬라임 공격");
            attackTimer = attackSpeed;  // 타이머를 attackSpeed로 리셋
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHp -= damage;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp); 
        hpBar.SetHp(currentHp);

        if (currentHp <= 0)
        {
            ChangePlayerState(PlayerState.Dead);
        }
    }

    public void Heal(float heal)
    {
        if(isDead) return;

        currentHp += heal;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);
        hpBar.SetHp(currentHp);
    }

    public void DisableAllWeapons()
    {
        Blade.SetActive(false);
        Arrow.SetActive(false);
        Laser.SetActive(false);
    }

    public void Dead()
    {
        GameManager.Instance.GameOver();
    }
}
