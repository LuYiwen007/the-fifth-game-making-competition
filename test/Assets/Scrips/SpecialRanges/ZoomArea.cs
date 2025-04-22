using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    public Cam camScript; // 引用 Cam 脚本

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 调用 Cam 脚本中的放大逻辑
            camScript?.StartZoomIn();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 调用 Cam 脚本中的缩小逻辑
            camScript?.StartZoomOut();
        }
    }
}
