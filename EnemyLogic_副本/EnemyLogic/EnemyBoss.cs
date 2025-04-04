using UnityEngine;

public class EnemyBoss : EnemyLogic
{
    //Boss基础属性（看具体情况）
    public float bossHealth = 2.5f;    
    public float bossDamage = 1.8f;    
    public float bossSpeed = 1.2f;     
    
    //Boss普通攻击
    public float normalAttackCooldown = 1.5f;      // 普通攻击冷却时间
    public float normalAttackDamage = 15f;         // 普通攻击伤害
    private float normalAttackTimer = 0f;          // 普通攻击计时器
    private bool canNormalAttack = true;           // 是否可以普通攻击
    
    //Boss特殊攻击
    private int attackCount = 0;                   // 攻击计数器
    private const int ATTACKS_FOR_SPECIAL = 3;     // 触发特殊攻击所需的普通攻击次数
    public float bossAttackCooldown = 5f;         // 特殊攻击冷却时间
    
    [Header("Boss状态")]
    private bool isBossEnraged = false;            // 是否处于狂暴状态
    private int bossPhaseIndex = 0;                // 当前阶段
    public float bossEnrageThreshold = 0.3f;       // 狂暴状态触发血量阈值
    
    [Header("特效")]
    public GameObject normalAttackEffect;          // 普通攻击特效
    public GameObject bossAttackEffect;           // 特殊攻击特效
    public GameObject bossEnrageEffect;           // 狂暴状态特效

    protected override void Start()//初始化Boss
    {
        base.Start();
        InitializeBoss();
    }

    private void InitializeBoss()//强化Boss属性
    {
        // 强化Boss属性
        maxHealth *= bossHealth;
        currentHealth = maxHealth;
        damage *= bossDamage;
        moveSpeed *= bossSpeed;
        normalAttackDamage *= bossDamage;
        bossAttackDamage *= bossDamage;
    }

    protected override void Update()
    {
        base.Update();
        UpdateBossLogic();
    }

    private void UpdateBossLogic()//更新Boss逻辑
    {
        // 更新特殊攻击冷却
        if (bossAttackTimer > 0)
            bossAttackTimer -= Time.deltaTime;

        // 检查是否需要进入狂暴状态
        CheckEnrageState();

        // 在合适的时机使用特殊攻击
        if (CanUseSpecialAttack())
            BossAttack();
    }

    private void CheckEnrageState()//检查是否需要进入狂暴状态
    {
        if (!isBossEnraged && (currentHealth / maxHealth) <= bossEnrageThreshold)
        {
            EnterEnrageState();
        }
    }

    private void EnterEnrageState()//进入狂暴状态（举例）
    {
        isBossEnraged = true;
        normalAttackDamage *= 1.5f;    // 狂暴时普通攻击伤害提升50%
        bossAttackDamage *= 1.5f;      // 狂暴时特殊攻击伤害提升50%
        moveSpeed *= 1.3f;             // 狂暴时速度提升30%
        normalAttackCooldown *= 0.7f;  // 狂暴时普通攻击冷却减少30%
        bossAttackCooldown *= 0.7f;    // 狂暴时特殊攻击冷却减少30%

        if (bossEnrageEffect != null)
            Instantiate(bossEnrageEffect, transform.position, Quaternion.identity);
            
        // 可以触发特殊动画或音效
    }

    private bool CanUseBossAttack()//是否可以使用特殊攻击
    {
        return specialAttackTimer <= 0 && 
               Vector3.Distance(transform.position, playerTransform.position) <= bossAttackRange;
    }

    public virtual void BossAttack()//使用特殊攻击
    {
        bossAttackTimer = bossAttackCooldown;

        if (bossAttackEffect != null)
            Instantiate(bossAttackEffect, transform.position, Quaternion.identity);

        // 根据不同阶段使用不同的特殊攻击
        switch (bossPhaseIndex)
        {
            case 0:
                PhaseOneBossAttack();
                break;
            case 1:
                PhaseTwoBossAttack();
                break;
            default:
                FinalPhaseAttack();
                break;
        }
    }

    protected virtual void PhaseOneBossAttack()//第一阶段特殊攻击
    {
        // 实现第一阶段特殊攻击
    }

    protected virtual void PhaseTwoBossAttack()//第二阶段特殊攻击
    {
        // 实现第二阶段特殊攻击
    }

    protected virtual void FinalPhaseAttack()//最终阶段特殊攻击
    {
        // 实现最终阶段特殊攻击
    }

    protected override void OnDeath()//死亡后掉落物品
    {
        // 触发Boss死亡事件
        GameEvents.OnBossDeath?.Invoke(this);
        
        // 可以在这里掉落特殊物品
        DropRewards();
        
        base.OnDeath();
    }

    protected virtual void DropRewards()//掉落物品
    {
        // 实现奖励掉落逻辑
    }

    // 提供一个公共方法来切换Boss战斗阶段
    public virtual void AdvancePhase()//切换Boss战斗阶段
    {
        bossPhaseIndex++;
        // 可以在这里修改Boss的行为模式或属性
    }

    protected override void DealDamage()
    {
        if (!canNormalAttack) return;

        // 检测攻击范围内的玩家
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange);
        
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                var gamePlayer = hit.GetComponent<PlayerHealth>();
                if (gamePlayer != null)
                {
                    gamePlayer.TakeDamage(normalAttackDamage);
                }
            }
        }

        // 生成普通攻击特效
        if (normalAttackEffect != null)
        {
            Instantiate(normalAttackEffect, transform.position, Quaternion.identity);
        }

        // 重置普通攻击冷却
        normalAttackTimer = normalAttackCooldown;
        canNormalAttack = false;

        // 增加攻击计数，检查是否需要使用特殊攻击
        attackCount++;
        if (attackCount >= ATTACKS_FOR_SPECIAL)
        {
            BossAttack();
            attackCount = 0;  // 重置计数器
        }
    }
}
