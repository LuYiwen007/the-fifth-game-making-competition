using UnityEngine;
using UnityEngine.AI;

namespace EnemyLogic;

/// <summary>
/// 敌人基类，包含所有敌人的通用行为和属性
/// 这是一个抽象类，需要被具体的敌人类型继承
/// </summary>
public abstract class EnemyLogic : MonoBehaviour
{
    [Header("基础属性")]
    public float maxHealth = 100f;        // 最大生命值
    protected float currentHealth;        // 当前生命值
    public float moveSpeed = 3f;          // 移动速度
    public float attackRange = 1.5f;      // 攻击范围
    public float detectionRange = 5f;     // 检测范围（超出此范围敌人将进入待机状态）

    [Header("组件引用")]
    protected Animator animator;           // 动画控制器组件
    protected SpriteRenderer spriteRenderer; // 精灵渲染器组件
    protected Rigidbody2D rb2d;           // 2D刚体组件
    protected Transform gamePlayer;        // 玩家对象的Transform组件

    // 状态标记
    protected bool isFlipped = false;     // 是否已翻转（用于控制朝向）
    protected bool isAttacking = false;   // 是否正在攻击
    protected EnemyState currentState;    // 当前状态

    /// <summary>
    /// 初始化敌人
    /// </summary>
    protected virtual void Start()
    {
        InitializeComponents();
        currentHealth = maxHealth;
        gamePlayer = GameObject.FindGameObjectWithTag("Player").transform;
    }

    /// <summary>
    /// 初始化所需的组件引用
    /// 子类可以重写此方法来初始化额外的组件
    /// </summary>
    protected virtual void InitializeComponents()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// 每帧更新敌人的行为
    /// </summary>
    protected virtual void Update()
    {
        UpdateEnemyState();
        UpdateEnemyBehavior();
        FlipSprite();
    }

    /// <summary>
    /// 更新敌人状态
    /// 基于与玩家的距离和攻击欲望来决定状态
    /// </summary>
    protected virtual void UpdateEnemyState()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, gamePlayer.position);
        
        if (distanceToPlayer <= attackRange && CanAttack())
        {
            currentState = EnemyState.Attacking;
        }
        else if (distanceToPlayer <= detectionRange)
        {
            currentState = EnemyState.Chasing;
        }
        else
        {
            currentState = EnemyState.Idle;
        }
    }

    /// <summary>
    /// 判断敌人是否可以攻击
    /// 子类可以重写此方法来实现自定义的攻击欲望逻辑
    /// </summary>
    protected virtual bool CanAttack()
    {
        return true; // 默认总是可以攻击
    }

    /// <summary>
    /// 根据当前状态执行相应的行为
    /// </summary>
    protected virtual void UpdateEnemyBehavior()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                HandleIdleState();
                break;
            case EnemyState.Chasing:
                ChasePlayer();
                break;
            case EnemyState.Attacking:
                AttackPlayer();
                break;
        }
    }

    /// <summary>
    /// 处理待机状态的行为
    /// </summary>
    protected virtual void HandleIdleState()
    {
        PlayIdleAnimation();
    }

    /// <summary>
    /// 追逐玩家的行为
    /// </summary>
    protected virtual void ChasePlayer()
    {
        Vector2 direction = (gamePlayer.position - transform.position).normalized;
        rb2d.velocity = direction * moveSpeed;
        PlayMoveAnimation();
    }

    /// <summary>
    /// 攻击玩家的行为
    /// </summary>
    protected virtual void AttackPlayer()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            PlayAttackAnimation();
            DealDamage();
        }
    }

    /// <summary>
    /// 处理造成伤害的逻辑
    /// 子类必须实现具体的伤害处理方式
    /// </summary>
    protected virtual void DealDamage()
    {
        // 子类实现具体的伤害处理逻辑
    }

    #region 动画相关方法
    /// <summary>
    /// 播放待机动画
    /// </summary>
    protected virtual void PlayIdleAnimation()
    {
        animator?.SetBool("IsMoving", false);
    }

    /// <summary>
    /// 播放移动动画
    /// </summary>
    protected virtual void PlayMoveAnimation()
    {
        animator?.SetBool("IsMoving", true);
    }

    /// <summary>
    /// 播放攻击动画
    /// </summary>
    protected virtual void PlayAttackAnimation()
    {
        animator?.SetTrigger("Attack");
    }

    /// <summary>
    /// 播放受伤动画
    /// </summary>
    protected virtual void PlayHitAnimation()
    {
        animator?.SetTrigger("Hit");
    }

    /// <summary>
    /// 播放死亡动画
    /// </summary>
    protected virtual void PlayDeathAnimation()
    {
        animator?.SetTrigger("Die");
    }
    #endregion

    /// <summary>
    /// 控制敌人朝向
    /// 根据玩家位置翻转敌人精灵
    /// </summary>
    protected virtual void FlipSprite()
    {
        if (gamePlayer.position.x > transform.position.x && isFlipped)
        {
            transform.localScale = new Vector3(1, 1, 1);
            isFlipped = false;
        }
        else if (gamePlayer.position.x < transform.position.x && !isFlipped)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            isFlipped = true;
        }
    }

    /// <summary>
    /// 处理受到伤害的逻辑
    /// </summary>
    /// <param name="damage">受到的伤害值</param>
    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        PlayHitAnimation();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// 处理死亡逻辑
    /// </summary>
    protected virtual void Die()
    {
        PlayDeathAnimation();
        OnDeath();
    }

    /// <summary>
    /// 死亡后的处理
    /// 子类可以重写此方法来添加额外的死亡效果
    /// </summary>
    protected virtual void OnDeath()
    {
        Destroy(gameObject, 1f); // 1秒后销毁对象
    }
}

/// <summary>
/// 敌人状态枚举
/// </summary>
public enum EnemyState
{
    Idle,       // 待机状态
    Chasing,    // 追逐状态
    Attacking   // 攻击状态
}

