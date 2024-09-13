using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract  class Slime : MonoBehaviour
{
    public enum SlimeState
    {
        Idle,
        Attack,
        Dead
    }

    public SlimeHpBar hpBar;

    private SlimeState currentState;

    protected float maxHp;
    protected float currentHp;
    protected float attackDamage;
    protected float attackSpeed;
    protected float attackTimer;

    protected bool isDead;

    private Animator slimeAnimator;

    // Getter Setter
    public SlimeState GetSlimeState() { return currentState;}
    public float GetMaxHp() {return maxHp;}
    public float GetCurrentHp() { return currentHp;}
    public void SetCurrentHp(float newHp) { currentHp = Mathf.Clamp(newHp, 0, maxHp);}
   
    void Awake() 
    {
        slimeAnimator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        ChangeSlimeState(SlimeState.Idle);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public virtual void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHp -= damage;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);
        hpBar.SetHp(currentHp);

        if (currentHp <= 0)
        {
           ChangeSlimeState(SlimeState.Dead);  // 사망 상태로 전환
        }
        else
        {   
            Debug.Log("TakeDamage called: currentHp = " + currentHp + ", maxHp = " + maxHp);
            slimeAnimator.SetTrigger("isHurt");
            if(currentState != SlimeState.Attack)
            {
                ChangeSlimeState(SlimeState.Attack);
            }
        }
    }

  void OnCollisionEnter2D(Collision2D other)
    {
        if(isDead) return;
        if (other.gameObject.CompareTag("Player"))
        {
            ChangeSlimeState(SlimeState.Attack);
        }
    }   

    void OnCollisionStay2D(Collision2D other)
    {
        if(isDead) return;

        if (other.gameObject.CompareTag("Player"))
        {
           if (currentState == SlimeState.Attack)
            {
                Attack(other.gameObject.GetComponent<Player>());
            }
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if(isDead) return;

        if (other.gameObject.CompareTag("Player"))
        {
            ChangeSlimeState(SlimeState.Idle);
        }
    }

    public virtual void Attack(Player player)
    {
        if(attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
            return;
        }
        else
        {
            player.TakeDamage(attackDamage);  // 공격 실행
            attackTimer = attackSpeed;  // 타이머를 attackSpeed로 리셋
        }
    }

    protected virtual void ChangeSlimeState(SlimeState state)
    {
        currentState = state;

        switch (state)
        {
            case SlimeState.Idle:
            slimeAnimator.CrossFade("Idle", 0.1f);
            break;
            case SlimeState.Attack:
            break;
            case SlimeState.Dead:
            slimeAnimator.CrossFade("Dead", 0.1f, 0, 0);  
            isDead = true;
            //HandleDeath();  
            break;
        }
    }

    public virtual void KillSlime()
    {
        Destroy(gameObject);
    }
}
