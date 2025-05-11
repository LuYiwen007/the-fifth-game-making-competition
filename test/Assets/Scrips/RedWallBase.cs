using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class RedWallBase : MonoBehaviour
{
    protected bool isTemporaryGray = false;
    protected SpriteRenderer spriteRenderer;
    protected Color originalColor;
    //[SerializeField] protected float pushForce = 3f; // Ĭ������

    protected virtual void Start()
    {
        // ��ȡ��Ҫ�����
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }

        // ���ó�ʼ��ɫΪ��ɫ
        originalColor = new Color(1f, 0f, 0f, 1f); // ��ɫ
        spriteRenderer.color = originalColor;

        // ȷ������ײ��������Ϊ������
        /*Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.isTrigger = true;
        }*/
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
            // ��ֹ���ͨ��
            BlockPlayer(other.gameObject);
        }
    }

    protected virtual void BlockPlayer(GameObject player)
    {
        // ��ȡ��ҵ�λ�ú͸������
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        if (playerRb != null && !playerRb.isKinematic)
        {
            // ������������
            //Vector2 pushDirection = (player.transform.position - transform.position).normalized;

            // ֻʹ��һ���ƶ���ʽ�������ͻ
            /*if (playerRb.velocity.magnitude < pushForce)
            {
                // ����ٶȽ�С��Ӧ������
                playerRb.velocity = pushDirection * pushForce;
            }*/

            // ��ֹ��ǽ
            /*float minDistance = 0.1f;
            float currentDistance = Vector2.Distance(player.transform.position, transform.position);
            if (currentDistance < minDistance)
            {
                Vector2 safePosition = (Vector2)transform.position + pushDirection * minDistance;
                playerRb.position = safePosition;
            }*/
        }
    }

    public virtual void OnTemporaryGrayArea()
    {
        // ��ʱ���Ч��
        isTemporaryGray = true;
        UpdateWallColor(new Color(0.5f, 0.5f, 0.5f, 1f));
    }

    public virtual void OnTemporaryGrayAreaEnd()
    {
        // �ָ�ԭʼ��ɫ
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