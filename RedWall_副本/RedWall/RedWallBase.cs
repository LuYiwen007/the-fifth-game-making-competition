using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class RedWallBase : MonoBehaviour
{
    protected bool isTemporaryGray = false;
    protected SpriteRenderer spriteRenderer;
    protected Color originalColor;
    [SerializeField] protected float pushForce = 3f; // 默认推力

    protected virtual void Start()
    {
        // 获取必要的组件
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }

        // 设置初始颜色为红色
        originalColor = new Color(1f, 0f, 0f, 1f); // 红色
        spriteRenderer.color = originalColor;

        // 确保有碰撞器且设置为触发器
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.isTrigger = true;
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        HandleCollision(other);
    }

    protected virtual void OnTriggerStay2D(Collider2D other)
    {
        HandleCollision(other);
    }

    protected virtual void HandleCollision(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 阻止玩家通过
            BlockPlayer(other.gameObject);
        }
    }

    protected virtual void BlockPlayer(GameObject player)
    {
        // 获取玩家的位置和刚体组件
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        if (playerRb != null && !playerRb.isKinematic)
        {
            // 计算推力方向
            Vector2 pushDirection = (player.transform.position - transform.position).normalized;
            
            // 只使用一种移动方式，避免冲突
            if (playerRb.velocity.magnitude < pushForce)
            {
                // 如果速度较小，应用推力
                playerRb.velocity = pushDirection * pushForce;
            }
            
            // 防止卡墙
            float minDistance = 0.1f;
            float currentDistance = Vector2.Distance(player.transform.position, transform.position);
            if (currentDistance < minDistance)
            {
                Vector2 safePosition = (Vector2)transform.position + pushDirection * minDistance;
                playerRb.position = safePosition;
            }
        }
    }

    public virtual void OnTemporaryGrayArea()
    {
        // 临时变灰效果
        isTemporaryGray = true;
        UpdateWallColor(new Color(0.5f, 0.5f, 0.5f, 1f));
    }

    public virtual void OnTemporaryGrayAreaEnd()
    {
        // 恢复原始颜色
        isTemporaryGray = false;
        UpdateWallColor(originalColor);
    }

    protected virtual void UpdateWallColor(Color newColor)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = newColor;
        }
    }
} 