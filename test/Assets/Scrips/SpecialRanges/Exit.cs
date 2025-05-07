using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Exit : MonoBehaviour
{
    GamePlayerLogic gamePlayerLogic;
    public TrapLogic trapLogic; // �󶨳����е� TrapLogic �ű�

    //�����Ҵ��������ڡ��Ƿ�������Կ��
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))//��Ҵ������ڼ��
        {
            gamePlayerLogic = collision.gameObject.GetComponent<GamePlayerLogic>();
            if (Inventory.Instance.HasItem("key", 3))//�ж�����Ƿ���3��Կ��
            {
                    trapLogic.DeactivateTrap(); // ���������߼�
                    Inventory.Instance.RemoveItemFromInventory("key", 3);
                    //Animator animator = gameObject.GetComponent<Animator>();
                    //animator.SetTrigger("Exit");// ���Ŷ���
                    UIController.Instance.SetGameState(UIController.GameState.Story);
                    GameManager.Instance.hasrespwam = false;
            }
            else
            {
                Debug.Log("Կ�ײ��㣬�޷��뿪����ǰ��" + Inventory.Instance.GetItemCount("key"));
            }
        }
    }
}
