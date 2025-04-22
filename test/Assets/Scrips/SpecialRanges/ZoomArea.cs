using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    public Cam camScript; // ���� Cam �ű�

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // ���� Cam �ű��еķŴ��߼�
            camScript?.StartZoomIn();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // ���� Cam �ű��е���С�߼�
            camScript?.StartZoomOut();
        }
    }
}
