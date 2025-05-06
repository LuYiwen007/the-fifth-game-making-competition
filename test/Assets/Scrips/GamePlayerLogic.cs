using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;
using Unity.Burst.CompilerServices;
using Unity.PlasticSCM.Editor.WebApi;


public class GamePlayerLogic : MonoBehaviour
{
    //玩家血量
    public int maxhp { get; set; } = 3;
    public int currenthp { get; set; }

    // 角色移动相关变量
    private float moveSpeed = 5f;
    private float sprintMultiplier = 1.5f;

    // 攻击相关变量
    private float attackRange = 2f;

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
    public GrayPaintBottle greyPaint;

    // 区域类型枚举
    public enum AreaType
    {
        Black,
        White,
        PermanentGray,
        TemporaryGray,
        Red // 添加红色区域类型
    }

    // 当前区域类型
    private AreaType currentAreaType/* = AreaType.Black*/;

    // 临时灰色区域相关
    private GameObject temporaryGrayAreaPrefab; // 临时灰色区域的预制体
    private GameObject permanentGrayAreaPrefab; // 永久灰色区域的预制体
    private float temporaryGrayAreaDuration = 2f; // 临时灰色区域持续时间
    private bool canCreateTemporaryGray = true; // 是否可以创建临时灰色区域

    // 私有变量
    private Rigidbody2D rb;//挂载刚体组件
    private Vector2 moveDirection;
    //private bool isDashing = false;
    private bool isSprinting = false;
    private Vector2 lastPosition; // 上一帧的位置
    
    // 事件
    public UnityEvent<float, float> OnPaintValuesChanged; // 参数：黑色颜料值，白色颜料值
    public UnityEvent OnDeath;

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
        Gray,
        Red
    }

    //重生点
    private Vector2 currentspwam;

    //关卡出生点
    [Header("一至三关出生点")]
    [SerializeField] private Vector2 _position1;
    [SerializeField] private Vector2 _position2;
    [SerializeField] private Vector2 _position3;

    public void InitializePlayer()//初始化玩家，重新开始关卡和从记录点重生都要调用
    {
        enabled = true; // 启用控制器
        rb = GetComponent<Rigidbody2D>();//for debug
        if (rb == null)
        {
            Debug.LogError("未找到Rigidbody2D组件");
        }

        // 记录初始位置
        //lastPosition = rb.position;

        //记录出生点
        //currentspwam = rb.position;
    }

    public void PlayerRespwam()//这个方法用于玩家从记录点重生
    {
        currenthp--; // 减少血量
        rb.MovePosition(currentspwam);
        InitializePlayer();
        blackPaintValue = 0;
        whitePaintValue = 0;
        OnDeath = null;
    }

    private void Update()//实时更新函数
    {
        // 获取输入
        HandleInput();

        // 处理颜料值变化
        HandlePaintValues();

        // 处理第三关玩家颜色变化
        if (GameManager.Instance != null && GameManager.Instance.CurrentLevel == 3)
        {
            PaintController();
        }
    }
    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleInput()//输入检测函数
    {
        // 获取移动输入
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(horizontalInput, verticalInput).normalized;
        
        // 检测交互输入
        if (Input.GetKeyDown(KeyCode.F))
        {
            Interact();
        }

        //if(Input.GetKeyDown(KeyCode.B))
        //{
        //    Debug.Log(Inventory.Instance.GetInventory());
        //}
    }

    // 死亡方法
    public void Die()
    {
        Debug.Log("角色死亡");
        // 触发死亡事件
        OnDeath?.Invoke();

        // 禁用控制器
        enabled = false;

        //死亡动画

        //切换死亡界面
        UIController.Instance.SetGameState(UIController.GameState.GameOver);

        //GameUI.Instance.HPUI();
    }

    //private void DieAndChangeHP()
    //{
    //    Die();
    //    currenthp--;
    //    if(currenthp < 0)
    //    {
    //        currenthp = 0;
    //    }
    //}

    private void HandlePaintValues()//颜料值处理函数
    {
        switch (currentAreaType)
        {
            case AreaType.Black:
                // 黑色区域增加黑色颜料值
                blackPaintValue = Mathf.Min(maxPaintValue, blackPaintValue + blackPaintIncreaseRate * Time.deltaTime);
                if (whitePaintValue > 0) // 只要有白色颜料就自动生成
                {
                    CreateTemporaryGrayArea();
                }
                break;
                
            case AreaType.White:
                whitePaintValue = Mathf.Min(maxPaintValue, whitePaintValue + whitePaintIncreaseRate * Time.deltaTime);
                if (blackPaintValue > 0) // 只要有黑色颜料就自动生成
                {
                    CreateTemporaryGrayArea();
                }
                break;
                
            case AreaType.PermanentGray:
                // 永久灰色区域减少两种颜料值
                blackPaintValue = Mathf.Max(0, blackPaintValue - paintDecreaseRate * Time.deltaTime);
                whitePaintValue = Mathf.Max(0, whitePaintValue - paintDecreaseRate * Time.deltaTime);
                break;

            case AreaType.Red:
                // 在红色区域不允许创建临时灰色区域
                canCreateTemporaryGray = false;
                break;
        }

        // 更新颜料值UI
        OnPaintValuesChanged?.Invoke(blackPaintValue, whitePaintValue);
        


        // 检查是否死亡
        if (blackPaintValue >= maxPaintValue || whitePaintValue >= maxPaintValue)
        {
            Die();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)// 检测进入的区域类型
    {
        AreaTypeComponent areaType = collision.gameObject.GetComponent<AreaTypeComponent>();
        // 更新当前区域类型
        if (/*collision.CompareTag("BlackArea")*/areaType.areaType == AreaType.Black) 
        {
            currentAreaType = AreaType.Black;
        }
        else if (/*collision.CompareTag("WhiteArea")*/areaType.areaType == AreaType.White)
        {
            currentAreaType = AreaType.White;
        }
        else if (/*collision.CompareTag("PermanentGrayArea")*/areaType.areaType == AreaType.PermanentGray)
        {
            currentAreaType = AreaType.PermanentGray;
        }
        else if (/*collision.CompareTag("TemporaryGray")*/areaType.areaType == AreaType.TemporaryGray)
        {
            currentAreaType = AreaType.TemporaryGray;
        }
        else if (/*collision.CompareTag("RedWall")*/areaType.areaType == AreaType.Red)
        {
            currentAreaType = AreaType.Red;
            canCreateTemporaryGray = false;
        }

        // 触发区域变化事件
        OnAreaChanged?.Invoke(currentAreaType);
    }

    private void OnTriggerEnter2D(Collider2D collision)//捡到钥匙、踩到陷阱、保存记录点
    {
        if (collision.CompareTag("key"))
        {
            Inventory.Instance.AddItemToInventory("key", 1);
            Destroy(collision.gameObject);
            Debug.Log("获得钥匙,当前"+ Inventory.Instance.GetItemCount("key"));
        }
        if (collision.CompareTag("trap"))
        {
            Debug.Log("踩到陷阱");
            Die();
        }
        if (collision.CompareTag("spwam"))
        {
            Debug.Log("设置出生点");
            currentspwam = collision.gameObject.transform.position;
            GameManager.Instance.hasrespwam = true;
        }
    }

    public void TransfornToLevel(int level)
    {
        switch (level)
        {
            case 1:
                rb.gameObject.transform.position = _position1;
                break;
            case 2:
                rb.gameObject.transform.position = _position2;
                break;
            case 3:
                rb.gameObject.transform.position = _position3;
                break;
        }
    }
    private void HandleMovement()//移动函数
    {
        //    if (isDashing)
        //    {
        //        // 闪避时保持当前方向
        //        return;
        //    }

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
        // 如果在红色区域或者不允许创建，直接返回
        if (currentAreaType == AreaType.Red || !canCreateTemporaryGray)
            return;

        float paintToDecrease = 5f; // 每次创建消耗的颜料值
        bool canCreate = false;

        // 根据当前区域类型决定消耗哪种颜料
        if (currentAreaType == AreaType.Black && whitePaintValue >= paintToDecrease)
        {
            whitePaintValue -= paintToDecrease;
            canCreate = true;
        }
        else if (currentAreaType == AreaType.White && blackPaintValue >= paintToDecrease)
        {
            blackPaintValue -= paintToDecrease;
            canCreate = true;
        }

        if (canCreate && temporaryGrayAreaPrefab != null)
        {
            // 创建临时灰色区域
            Vector3 position = transform.position;
            GameObject grayArea = Instantiate(temporaryGrayAreaPrefab, position, Quaternion.identity);
            
            // 设置持续时间
            TemporaryGrayArea temporaryGrayAreaComponent = grayArea.GetComponent<TemporaryGrayArea>();
            if (temporaryGrayAreaComponent != null)
            {
                temporaryGrayAreaComponent.SetDuration(temporaryGrayAreaDuration);
            }
        }
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

    // 使用灰色颜料瓶创建永久灰色区域
    public void CreatePermanentGrayArea()
    {
        if (Inventory.Instance.GetItemCount("GrayPaintBottle") >= 1)
        {
            Inventory.Instance.RemoveItemFromInventory("GrayPaintBottle", 1);
            greyPaint.InitializeGreyPaint();
        }

        if (permanentGrayAreaPrefab != null && GetCurrentAreaType() != AreaType.PermanentGray)
        {
            // 消耗颜料并生成区域
            greyPaint.ConsumePaint();
            Vector3 position = transform.position;
            Instantiate(permanentGrayAreaPrefab, position, Quaternion.identity);
        }
    }

    //public float GetGreyPaintValue()
    //{
    //    return greyPaintValue;
    //}

    // 当获得灰色颜料瓶时调用此方法
    //public void AddGreyPaint()
    //{
    //    greyPaintValue = maxPaintValue; // 新的颜料瓶有满值的颜料
    //}

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("RedWall"))
        {
            canCreateTemporaryGray = true;
        }
    }
}

// 可交互接口
public interface IInteractable
{
    void Interact(GameObject player);
}

// 可拾取物品接口
public interface IPickupable
{
    string GetItemName();
    int GetItemAmount();
    void OnPickup(GameObject player);
}

// 临时灰色区域组件
public class TemporaryGrayArea : MonoBehaviour
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