using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;
using Unity.Burst.CompilerServices;


public class GamePlayerLogic : MonoBehaviour
{
    // 角色移动相关变量
    private float moveSpeed = 5f;
    private float sprintMultiplier = 1.5f;


    //// 闪避相关变量
    //private float dashForce = 20f;
    //private float dashDuration = 0.2f;
    //private float dashCooldown = 1f;

    // 攻击相关变量
    //private float attackCooldown = 0.5f;
    private float attackRange = 2f;
    //private LayerMask attackableLayers;


    // 交互相关变量
    private float interactionRange = 3f;
    private LayerMask interactableLayers;
    
    // 颜料值相关变量
    private float maxPaintValue = 100f;
    private float blackPaintValue = 0f;
    private float whitePaintValue = 0f;
    private float blackPaintIncreaseRate = 2f; // 每秒增加黑色颜料值
    private float whitePaintIncreaseRate = 2f; // 每秒增加白色颜料值
    private float paintDecreaseRate = 5f; // 每秒减少颜料值
    private float blackPaintDecreaseRate = 1f; // 每移动两个坐标减少的黑色颜料值
    private float whitePaintDecreaseRate = 1f; // 每移动两个坐标减少的白色颜料值
    private float moveDistanceCounter = 0f; // 移动距离计数器
    private float moveDistanceThreshold = 2f; // 移动距离阈值
    
    // 区域类型枚举
    public enum AreaType
    {
        Black,
        White,
        PermanentGray,
        TemporaryGray
    }
    
    // 当前区域类型
    private AreaType currentAreaType = AreaType.Black;
    
    // 临时灰色区域相关
    private GameObject temporaryGrayAreaPrefab; // 临时灰色区域的预制体
    private float temporaryGrayAreaDuration = 2f; // 临时灰色区域持续时间
    
    // 背包系统相关
    private Dictionary<string, int> inventory = new Dictionary<string, int>(); // 背包字典，键为物品名称，值为数量
    private int maxInventorySize = 20; // 背包最大容量
    
    // 私有变量
    private Rigidbody2D rb;//挂载刚体组件
    private Vector2 moveDirection;
    private bool isDashing = false;
    private float dashTimeLeft;
    private float dashCooldownTimer;
    private float attackCooldownTimer;
    private bool isSprinting = false;
    private Vector2 lastPosition; // 上一帧的位置
    
    // 事件
    public UnityEvent<float, float> OnPaintValuesChanged; // 参数：黑色颜料值，白色颜料值
    public UnityEvent OnDeath;
    public UnityEvent<string, int> OnInventoryChanged; // 参数：物品名称，当前数量
    
    // 交互接口
    public delegate void InteractionCallback(GameObject interactable);
    public event InteractionCallback OnInteract;
    
    // 区域变化接口
    public delegate void AreaChangeCallback(AreaType newAreaType);
    public event AreaChangeCallback OnAreaChanged;

    //第三关玩家自身染色枚举类型
    public enum PlayerColorState
    {
        Black,
        White,
        Transparent,
        Gray
    }
    private void Start()//初始化函数
    {
        Initialize();
    }
    
    public void Initialize()
    {
        rb = GetComponent<Rigidbody2D>();//for debug
        if (rb == null)
        {
            //Debug.LogError("未找到Rigidbody2D组件");
        }

        // 初始化颜料值---已经移动到Gamemeneger中
        //blackPaintValue = 0f;
        //whitePaintValue = 0f;
        //OnPaintValuesChanged?.Invoke(blackPaintValue, whitePaintValue);

        // 记录初始位置
        lastPosition = rb.position;

        // 初始化背包
        InitializeInventory();
    }

    private void InitializeInventory()//初始化背包函数
    {
        // 添加初始物品
        AddItemToInventory("BlackPaintBottle", 0);
        AddItemToInventory("WhitePaintBottle", 0);
        AddItemToInventory("GrayPaintBottle", 0);
    }


    private void Update()//实时更新函数
    {
        // 获取输入
        HandleInput();

        /*
         // 更新计时器
         UpdateTimers();
         */

        // 处理颜料值变化
        HandlePaintValues();

        // 检测区域类型
        //DetectAreaType();

        // 处理第三关玩家颜色变化
        if (GameManager.Instance.CurrentLevel == 3)
        {
            PaintController();
        }
    }


    private void FixedUpdate()
    {
        // 处理移动
        HandleMovement();

        /*
         // 处理闪避
         HandleDash();
        */

    }
    

    private void HandleInput()//输入检测函数
    {
        // 获取移动输入
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(horizontalInput, verticalInput).normalized;
        
       /*
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
        */
        
        // 检测交互输入
        if (Input.GetKeyDown(KeyCode.F))
        {
            Interact();
        }

        if(Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log(GetInventory());
        }
    }
    

    private void UpdateTimers()//计时器函数
    {
       /* // 更新闪避计时器
        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }
        */

        /*
        // 更新攻击计时器
        if (attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime;
        }
        */
        
       /* // 更新闪避持续时间
        if (isDashing)
        {
            dashTimeLeft -= Time.deltaTime;
            if (dashTimeLeft <= 0)
            {
                EndDash();
            }
        }
        */
    }
    

    private void HandlePaintValues()//颜料值处理函数
    {
        //Debug.Log("黑色" + blackPaintValue + "白色" + whitePaintValue);
        // 根据当前区域类型处理颜料值
        switch (currentAreaType)
        {
            case AreaType.Black:
                // 黑色区域增加黑色颜料值
                blackPaintValue = Mathf.Min(maxPaintValue, blackPaintValue + blackPaintIncreaseRate * Time.deltaTime);
                break;
                
            case AreaType.White:
                // 白色区域增加白色颜料值
                whitePaintValue = Mathf.Min(maxPaintValue, whitePaintValue + whitePaintIncreaseRate * Time.deltaTime);
                break;
                
            case AreaType.PermanentGray:
                // 永久灰色区域减少两种颜料值
                blackPaintValue = Mathf.Max(0, blackPaintValue - paintDecreaseRate * Time.deltaTime);
                whitePaintValue = Mathf.Max(0, whitePaintValue - paintDecreaseRate * Time.deltaTime);
                break;
                
            case AreaType.TemporaryGray:
                // 临时灰色区域减少两种颜料值
                blackPaintValue = Mathf.Max(0, blackPaintValue - paintDecreaseRate * Time.deltaTime);
                whitePaintValue = Mathf.Max(0, whitePaintValue - paintDecreaseRate * Time.deltaTime);
                break;
        }
        
        // 触发颜料值变化事件
        OnPaintValuesChanged?.Invoke(blackPaintValue, whitePaintValue);
        
        // 检查是否死亡
        if (blackPaintValue >= maxPaintValue || whitePaintValue >= maxPaintValue)
        {
            Die();
        }
    }
      

    //private void DetectAreaType()//检测区域类型函数
    //{
    //    // 这里应该实现检测玩家当前所在区域类型的逻辑
    //    // 可以通过射线检测、触发器检测等方式实现
    //    // 实际实现需要根据游戏场景设计调整
    //    RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero, 0.1f);
    //    if (hit.collider != null)
    //    {
    //        // 假设区域有AreaTypeComponent组件，包含区域类型信息
    //        AreaTypeComponent areaComponent = hit.collider.GetComponent<AreaTypeComponent>();
    //        Debug.Log(hit.collider);
    //        if (areaComponent != null)
    //        {
    //            AreaType newAreaType = areaComponent.areaType;
    //            if (newAreaType != currentAreaType)
    //            {
    //                currentAreaType = newAreaType;
    //                OnAreaChanged?.Invoke(currentAreaType);
    //            }
    //        }
    //    }
    //}

    private void OnTriggerStay2D(Collider2D collision)// 检测进入的区域类型
    {
        AreaTypeComponent areaComponent = collision.GetComponent<AreaTypeComponent>();
        if (areaComponent != null)
        {
            AreaType newAreaType = areaComponent.areaType;
            if (newAreaType != currentAreaType)
            {
                currentAreaType = newAreaType;
                OnAreaChanged?.Invoke(currentAreaType);
                Debug.Log(collision);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)//捡到钥匙
    {
        if (collision.CompareTag("key"))
        {
            AddItemToInventory("key", 1);
            Destroy(collision.gameObject);
            Debug.Log("获得钥匙,当前"+GetItemCount("key"));
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
            
            // 计算移动距离
            float moveDistance = Vector2.Distance(lastPosition, newPosition);
            moveDistanceCounter += moveDistance;
            
            // 检查是否达到移动距离阈值
            if (moveDistanceCounter >= moveDistanceThreshold)
            {
                moveDistanceCounter = 0f;
                
                // 在白色区域时，减少黑色颜料值并生成临时灰色区域
                if (currentAreaType == AreaType.White && blackPaintValue > 0)
                {
                    blackPaintValue = Mathf.Max(0, blackPaintValue - blackPaintDecreaseRate);
                    CreateTemporaryGrayArea();
                }
                // 在黑色区域时，减少白色颜料值并生成临时灰色区域
                if (currentAreaType == AreaType.Black && whitePaintValue > 0)
                {
                    whitePaintValue = Mathf.Max(0, whitePaintValue - whitePaintDecreaseRate);
                    CreateTemporaryGrayArea();
                }
            }
            
            // 更新上一帧位置
            lastPosition = newPosition;
        }
    }
    

    private void CreateTemporaryGrayArea()//创建临时灰色区域函数
    {
        // 如果黑色颜料值为0，不创建临时灰色区域
        if (blackPaintValue <= 0)
        {
            return;
        }
        
        // 创建临时灰色区域
        if (temporaryGrayAreaPrefab != null)
        {
            GameObject temporaryArea = Instantiate(temporaryGrayAreaPrefab, transform.position, Quaternion.identity);
            
            // 设置临时灰色区域的持续时间
            TemporaryGrayArea temporaryGrayArea = temporaryArea.GetComponent<TemporaryGrayArea>();
            if (temporaryGrayArea != null)
            {
                temporaryGrayArea.SetDuration(temporaryGrayAreaDuration);
            }
        }
    }
    

    /*private void StartDash()//闪避函数
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
    */
    
   /* private void Attack()//攻击函数
    {
        attackCooldownTimer = attackCooldown;
        
        // 检测攻击范围内的敌人
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position + transform.right * attackRange, attackRange, attackableLayers);
        
        foreach (var hitCollider in hitColliders)
        {
            // 这里可以添加对敌人的伤害逻辑
            //Debug.Log("攻击到: {hitCollider.name}");
            
            // 如果有敌人接口，可以调用
            IAttackable attackable = hitCollider.GetComponent<IAttackable>();
            if (attackable != null)
            {
                attackable.TakeDamage(10); // 假设伤害值为10
            }
        }
        
        // 可以在这里添加攻击动画或特效
    }
    */
    
    private void Interact()//玩家交互函数
    {
        // 检测交互范围内的物体
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position + transform.right * interactionRange, interactionRange, interactableLayers);
        
        if (hitColliders.Length > 0)
        {
            // 找到最近的交互物体
            Collider2D nearestCollider = hitColliders[0];
            float nearestDistance = Vector2.Distance(transform.position, nearestCollider.transform.position);
            Debug.Log($"与 {nearestCollider.name} 交互");
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
            
            //Debug.Log($"与 {nearestCollider.name} 交互");
        }
    }

    /*
    public void TakeDamage(float damage)//（这是玩家受伤函数，保留接口但不再使用
    {
        // 此函数保留但不再使用，因为已经移除了血量系统
        //Debug.Log("");
    }
    */

    /*  
    public void Heal(float amount)//如有需要，可加（这是血量系统的函数），保留接口但不再使用
    {
        //Debug.Log("");
    }   
    */  

    public void SetMaxPaintValue(float newMaxPaintValue)//设置最大颜料值函数
    {
        maxPaintValue = newMaxPaintValue;
        blackPaintValue = Mathf.Min(blackPaintValue, maxPaintValue);
        whitePaintValue = Mathf.Min(whitePaintValue, maxPaintValue);
        OnPaintValuesChanged?.Invoke(blackPaintValue, whitePaintValue);
    }
    
    public void SetBlackPaintValue(float value)//设置黑色颜料值函数
    {
        blackPaintValue = Mathf.Clamp(value, 0, maxPaintValue);
        OnPaintValuesChanged?.Invoke(blackPaintValue, whitePaintValue);
        
        // 检查角色是否死亡
        if (blackPaintValue >= maxPaintValue)
        {
            Die();
        }
    }
    
    public void SetWhitePaintValue(float value)//设置白色颜料值函数
    {
        whitePaintValue = Mathf.Clamp(value, 0, maxPaintValue);
        OnPaintValuesChanged?.Invoke(blackPaintValue, whitePaintValue);
        
        // 检查角色是否死亡
        if (whitePaintValue >= maxPaintValue)
        {
            Die();
        }
    }
    
    public float GetBlackPaintValue()//获取黑色颜料值函数
    {
        return blackPaintValue;
    }
    
    public float GetWhitePaintValue()//获取白色颜料值函数
    {
        return whitePaintValue;
    }
    
    public float GetMaxPaintValue()//获取最大颜料值函数
    {
        return maxPaintValue;
    }
    
    public AreaType GetCurrentAreaType()//获取当前区域类型函数
    {
        return currentAreaType;
    }
    

    // 背包系统相关方法
    public bool AddItemToInventory(string itemName, int amount = 1)//添加物品到背包
    {
        // 检查背包是否已满
        if (GetInventoryItemCount() >= maxInventorySize)
        {
            //Debug.Log("背包已满，无法添加物品");
            return false;
        }
        
        // 如果物品已存在，增加数量
        if (inventory.ContainsKey(itemName))
        {
            inventory[itemName] += amount;
        }
        else
        {
            // 否则添加新物品
            inventory[itemName] = amount;
        }
        
        // 触发背包变化事件
        OnInventoryChanged?.Invoke(itemName, inventory[itemName]);
        return true;
    }
    
    public bool RemoveItemFromInventory(string itemName, int amount = 1)//从背包移除物品
    {
        // 检查物品是否存在
        if (!inventory.ContainsKey(itemName))
        {
            //Debug.Log($"背包中没有{itemName}");
            return false;
        }
        
        // 检查物品数量是否足够
        if (inventory[itemName] < amount)
        {
            //Debug.Log($"背包中{itemName}数量不足");
            return false;
        }
        
        // 减少物品数量
        inventory[itemName] -= amount;
        
        // 如果物品数量为0，从背包中移除
        if (inventory[itemName] <= 0)
        {
            inventory.Remove(itemName);
        }
        
        // 触发背包变化事件
        OnInventoryChanged?.Invoke(itemName, inventory.ContainsKey(itemName) ? inventory[itemName] : 0);
        return true;
    }
    
    public int GetItemCount(string itemName)//获取物品数量
    {
        if (inventory.ContainsKey(itemName))
        {
            return inventory[itemName];
        }
        return 0;
    }
    
    public bool HasItem(string itemName, int amount = 1)//检查是否有足够数量的物品
    {
        return GetItemCount(itemName) >= amount;
    }
    
    public int GetInventoryItemCount()//获取背包中物品总数
    {
        int totalCount = 0;
        foreach (var item in inventory)
        {
            totalCount += item.Value;
        }
        return totalCount;
    }
    
    public int GetMaxInventorySize()//获取背包最大容量
    {
        return maxInventorySize;
    }
    
    public void SetMaxInventorySize(int size)//设置背包最大容量
    {
        maxInventorySize = size;
    }
    
    public Dictionary<string, int> GetInventory()//获取整个背包
    {
        return new Dictionary<string, int>(inventory);
    }


    // 死亡方法
    private void Die()
    {
        // 触发死亡事件
        OnDeath?.Invoke();
        
        // 禁用控制器
        enabled = false;
        
        // 可以在这里添加死亡动画或特效
        //Debug.Log("角色死亡");
    }
    
    //在编辑器中可视化攻击和交互范
    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.red;//红色
        Gizmos.DrawWireSphere(transform.position + transform.right * attackRange, attackRange);//绘制攻击范围   
        
        Gizmos.color = Color.blue;//蓝色    
        Gizmos.DrawWireSphere(transform.position + transform.right * interactionRange, interactionRange);//绘制交互范围 
    }

    //第三关主角自身的颜色变化方法
    private void ChangeColor(PlayerColorState colorState)
    {
        switch (colorState)
        {
            case PlayerColorState.Black:
                // 设置为黑色
                GetComponent<SpriteRenderer>().color = Color.black;
                break;
            case PlayerColorState.White:
                // 设置为白色
                GetComponent<SpriteRenderer>().color = Color.white;
                break;
            case PlayerColorState.Gray:
                // 设置为灰色
                GetComponent<SpriteRenderer>().color = Color.gray;
                break;
            case PlayerColorState.Transparent:
                // 设置为透明
                GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0); // RGBA(0,0,0,0)
                break;
        }
    }
    //这里是控制第三关玩家自身染色的函数
    public void PaintController()
    {
        PlayerColorState colorState;
        // 如果黑白颜料值都大于0，则为灰色
        if (blackPaintValue > 0 && whitePaintValue > 0)
        {
            colorState = PlayerColorState.Gray;
        }
        else if (currentAreaType == AreaType.Black && blackPaintValue > 0)
        {
            // 如果玩家在黑色区域且当前染色为黑色，则变为透明
            colorState = PlayerColorState.Transparent;
        }
        else if (currentAreaType == AreaType.White && whitePaintValue > 0)
        {
            // 如果玩家在白色区域且当前染色为白色，则变为透明
            colorState = PlayerColorState.Transparent;
        }
        else if (blackPaintValue > whitePaintValue)
        {
            // 黑色颜料值更高，则染色为黑色
            colorState = PlayerColorState.Black;
        }
        else if (whitePaintValue > blackPaintValue)
        {
            // 白色颜料值更高，则染色为白色
            colorState = PlayerColorState.White;
        }
        else
        {
            // 默认情况，透明
            colorState = PlayerColorState.Transparent;
        }
        ChangeColor(colorState);
    }

    


}

/*
// 可攻击接口
public interface IAttackable
{
    void TakeDamage(float damage);
}
    */
// 可交互接口
public interface IInteractable
{
    void Interact(GameObject player);
}

// 区域类型组件
//public class AreaTypeComponent : MonoBehaviour
//{
//    public GamePlayerLogic.AreaType areaType;
//}

// 可拾取物品接口
public interface IPickupable
{
    string GetItemName();
    int GetItemAmount();
    void OnPickup(GameObject player);
}

// 颜料瓶物品类
public class PaintBottle : MonoBehaviour, IPickupable
{
    public enum BottleType
    {
        Black,
        White,
        Gray
    }
    
    public BottleType bottleType;
    public int amount = 1;
    
    public string GetItemName()
    {
        switch (bottleType)
        {
            case BottleType.Black:
                return "BlackPaintBottle";//黑色颜料瓶
            case BottleType.White:
                return "WhitePaintBottle";//白色颜料瓶
            case BottleType.Gray:
                return "GrayPaintBottle";//灰色颜料瓶
            default:
                return "UnknownBottle";//未知颜料瓶
        }
    }
    
    public int GetItemAmount()
    {
        return amount;
    }
    
    public void OnPickup(GameObject player)
    {
        GamePlayerLogic playerLogic = player.GetComponent<GamePlayerLogic>();
        if (playerLogic != null)
        {
            if (playerLogic.AddItemToInventory(GetItemName(), GetItemAmount()))
            {
                // 拾取成功，销毁物品
                Destroy(gameObject);
            }
        }
    }

}

public class TemporaryGrayArea : MonoBehaviour// 临时灰色区域组件
{
    private float duration;//灰色区域持续时间
    private float timer;//计时器
    
    public void SetDuration(float duration)//设置灰色区域持续时间
    {
        this.duration = duration;
        timer = 0f;
    }
    
    private void Update()//每帧检测灰色区域
    {
        timer += Time.deltaTime;
        if (timer >= duration)//超时则灰色区域消失
        {
            Destroy(gameObject);
        }
    }
}

