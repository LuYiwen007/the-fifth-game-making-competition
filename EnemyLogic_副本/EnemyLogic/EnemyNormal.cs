using UnityEngine;
using EnemyLogic;

public class EnemyNormal : EnemyLogic
{
    //普通敌人属性
    public float normalAttackCooldown = 2f;      // 普通攻击冷却时间
    public float normalAttackDamage = 10f;       // 普通攻击伤害
    
    //普通敌人状态
    private float normalAttackTimer = 0f;        // 攻击计时器
    private bool canAttack = true;               // 是否可以攻击


    protected override void Start()//初始化普通敌人
    {
        base.Start();
        InitializeNormalEnemy();
    }

    private void InitializeNormalEnemy()
    {
        // 初始化普通敌人的特定属性
        currentHealth = maxHealth;
    }

    protected override void Update()//更新普通敌人
    {
        base.Update();
        UpdateAttackCooldown();
    }

    private void UpdateAttackCooldown()//更新普通敌人攻击冷却
    {
        if (normalAttackTimer > 0)
        {
            normalAttackTimer -= Time.deltaTime;
            canAttack = false;
        }
        else
        {
            canAttack = true;
        }
    }

    protected override bool CanAttack()//是否可以攻击
    {
        return canAttack && base.CanAttack();
    }

    protected override void DealDamage()//处理普通敌人攻击
    {
        if (!canAttack) return;

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

        // 生成攻击特效
        if (normalAttackEffect != null)
        {
            Instantiate(normalAttackEffect, transform.position, Quaternion.identity);
        }

        // 重置攻击冷却
        normalAttackTimer = normalAttackCooldown;
        canAttack = false;
    }

    protected override void OnDeath()
    {
        // 可以在这里添加简单的掉落物品逻辑
        DropItems();
        
        base.OnDeath();
    }

    private void DropItems()
    {
        // 实现简单的掉落物品逻辑
        // 比如随机掉落金币或其他物品
    }

    // 用于在编辑器中显示攻击范围
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
