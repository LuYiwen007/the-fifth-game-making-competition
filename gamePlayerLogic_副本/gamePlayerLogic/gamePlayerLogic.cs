using UnityEngine;
using UnityEngine.Events;

namespace gamePlayerLogic;

public class GamePlayerLogic : MonoBehaviour
{
    // 角色移动相关变量
    private float moveSpeed = 5f;
    private float sprintMultiplier = 1.5f;
    
    // 闪避相关变量
    private float dashForce = 20f;
    private float dashDuration = 0.2f;
    private float dashCooldown = 1f;
    
    // 攻击相关变量
    private float attackCooldown = 0.5f;
    private float attackRange = 2f;
    private LayerMask attackableLayers;
    
    // 交互相关变量
    private float interactionRange = 3f;
    private LayerMask interactableLayers;
    
    // 血量相关变量
    private float maxHealth = 100f;
    private float currentHealth;
    private float healthRegenRate = 0f; // 每秒恢复血量
    private float healthRegenDelay = 5f; // 受伤后多少秒开始恢复
    private float invincibilityDuration = 1f; // 受伤后的无敌时间
    
    // 私有变量
    private Rigidbody2D rb;//挂载刚体组件
    private Vector2 moveDirection;
    private bool isDashing = false;
    private float dashTimeLeft;
    private float dashCooldownTimer;
    private float attackCooldownTimer;
    private bool isSprinting = false;
    private float lastDamageTime;
    private float healthRegenTimer;
    private bool isInvincible = false;
    private float invincibilityTimer;
    
    // 事件
    public UnityEvent<float, float> OnHealthChanged; // 参数：当前血量，最大血量
    public UnityEvent OnDeath;
    
    // 交互接口
    public delegate void InteractionCallback(GameObject interactable);
    public event InteractionCallback OnInteract;
    
    
    private void Start()//初始化函数
    {
        rb = GetComponent<Rigidbody2D>();//for debug
        if (rb == null)
        {
            Debug.LogError("未找到Rigidbody2D组件");
        }
        
        // 初始化血量
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
    

    private void Update()//实时更新函数
    {
        // 获取输入
        HandleInput();
        
        // 更新计时器
        UpdateTimers();
        
        // 处理血量恢复
        HandleHealthRegeneration();
    }
    

    private void FixedUpdate()
    {
        // 处理移动
        HandleMovement();
        
        // 处理闪避
        HandleDash();
    }
    

    private void HandleInput()//输入检测函数
    {
        // 获取移动输入
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(horizontalInput, verticalInput).normalized;
        
        // 检测冲刺输入
        isSprinting = Input.GetKey(KeyCode.LeftShift);
        
        // 检测闪避输入
        if (Input.GetMouseButtonDown(1) && dashCooldownTimer <= 0 && !isDashing)
        {
            StartDash();
        }
        
        // 检测攻击输入
        if (Input.GetMouseButtonDown(0) && attackCooldownTimer <= 0)
        {
            Attack();
        }
        
        // 检测交互输入
        if (Input.GetKeyDown(KeyCode.F))
        {
            Interact();
        }
    }
    

    private void UpdateTimers()//计时器函数
    {
        // 更新闪避计时器
        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }
        
        // 更新攻击计时器
        if (attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime;
        }
        
        // 更新闪避持续时间
        if (isDashing)
        {
            dashTimeLeft -= Time.deltaTime;
            if (dashTimeLeft <= 0)
            {
                EndDash();
            }
        }
        
        // 更新无敌时间
        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0)
            {
                isInvincible = false;
            }
        }
    }
    

    private void HandleHealthRegeneration()//血量回复函数
    {
        // 如果血量已满，不进行恢复
        if (currentHealth >= maxHealth)
        {
            return;
        }
        
        // 检查是否已经过了恢复延迟时间
        if (Time.time - lastDamageTime < healthRegenDelay)
        {
            return;
        }
        
        // 恢复血量
        if (healthRegenRate > 0)
        {
            currentHealth = Mathf.Min(currentHealth + healthRegenRate * Time.deltaTime, maxHealth);
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
        }
    }
    

    private void HandleMovement()//移动函数
    {
        if (isDashing)
        {
            // 闪避时保持当前方向
            return;
        }
        
        // 计算移动速度
        float currentSpeed = moveSpeed;
        if (isSprinting)
        {
            currentSpeed *= sprintMultiplier;
        }
        
        // 应用移动
        if (moveDirection != Vector2.zero)
        {
            // 直接修改位置
            Vector2 newPosition = rb.position + moveDirection * currentSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPosition);
            
            // 旋转角色面向移动方向
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            rb.MoveRotation(Mathf.LerpAngle(rb.rotation, angle, 10f * Time.fixedDeltaTime));
        }
    }
    

    private void StartDash()//闪避函数
    {
        isDashing = true;
        dashTimeLeft = dashDuration;
        dashCooldownTimer = dashCooldown;
        
        // 直接设置速度
        Vector2 dashDirection = moveDirection != Vector2.zero ? moveDirection : (Vector2)(transform.right);
        rb.velocity = dashDirection * dashForce;
        
        // 可以在这里添加闪避特效或声音
    }
    

    private void EndDash()//恢复闪避冷却
    {
        isDashing = false;
        // 可以在这里添加闪避结束后的效果
    }
    

    private void HandleDash()
    {
        // 闪避逻辑在FixedUpdate中处理
    }

    
    private void Attack()//攻击函数
    {
        attackCooldownTimer = attackCooldown;
        
        // 检测攻击范围内的敌人
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position + transform.right * attackRange, attackRange, attackableLayers);
        
        foreach (var hitCollider in hitColliders)
        {
            // 这里可以添加对敌人的伤害逻辑
            Debug.Log($"攻击到: {hitCollider.name}");
            
            // 如果有敌人接口，可以调用
            IAttackable attackable = hitCollider.GetComponent<IAttackable>();
            if (attackable != null)
            {
                attackable.TakeDamage(10); // 假设伤害值为10
            }
        }
        
        // 可以在这里添加攻击动画或特效
    }

    
    private void Interact()//玩家交互函数
    {
        // 检测交互范围内的物体
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position + transform.right * interactionRange, interactionRange, interactableLayers);
        
        if (hitColliders.Length > 0)
        {
            // 找到最近的交互物体
            Collider2D nearestCollider = hitColliders[0];
            float nearestDistance = Vector2.Distance(transform.position, nearestCollider.transform.position);
            
            for (int i = 1; i < hitColliders.Length; i++)
            {
                float distance = Vector2.Distance(transform.position, hitColliders[i].transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestCollider = hitColliders[i];
                }
            }
            
            // 触发交互事件
            OnInteract?.Invoke(nearestCollider.gameObject);
            
            // 如果有交互接口，可以调用
            IInteractable interactable = nearestCollider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact(gameObject);
            }
            
            Debug.Log($"与 {nearestCollider.name} 交互");
        }
    }

    
    public void TakeDamage(float damage)//玩家受伤函数
    {
        // 如果处于无敌状态，不受伤害
        if (isInvincible)
        {
            return;
        }
        
        // 应用伤害
        currentHealth = Mathf.Max(0, currentHealth - damage);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        
        // 更新最后受伤时间
        lastDamageTime = Time.time;
        
        // 进入无敌状态
        isInvincible = true;
        invincibilityTimer = invincibilityDuration;
        
        // 检查是否死亡
        if (currentHealth <= 0)
        {
            Die();
        }
        
        // 可以在这里添加受伤特效或声音
    }
    

    public void Heal(float amount)//如有需要，可加
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        
        // 可以在这里添加恢复特效或声音
    }
    
、
    public void SetMaxHealth(float newMaxHealth)//设置最大血量函数
    {
        maxHealth = newMaxHealth;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
    
    
    private void Die()// 死亡方法
    {
        // 触发死亡事件
        OnDeath?.Invoke();
        
        // 禁用控制器
        enabled = false;
        
        // 可以在这里添加死亡动画或特效
        Debug.Log("角色死亡");
    }
    

    private void OnDrawGizmosSelected()//在编辑器中可视化攻击和交互范围
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.right * attackRange, attackRange);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + transform.right * interactionRange, interactionRange);
    }
}


// 可攻击接口
public interface IAttackable
{
    void TakeDamage(float damage);
}

// 可交互接口
public interface IInteractable
{
    void Interact(GameObject player);
}

