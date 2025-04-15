using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Exit : MonoBehaviour
{
    GamePlayerLogic gamePlayerLogic;

    //�����Ҵ��������ڡ��Ƿ�������Կ��
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))//��Ҵ������ڼ��
        {
            gamePlayerLogic = collision.gameObject.GetComponent<GamePlayerLogic>();
            if (gamePlayerLogic.HasItem("key", 3))//�ж�����Ƿ���3��Կ��
            {
                gamePlayerLogic.RemoveItemFromInventory("key", 3);
                Animator animator = gameObject.GetComponent<Animator>();
                animator.SetTrigger("Exit");// ���Ŷ���
                GameManager.Instance.NextLevel();//������һ���ؿ�
            }
            else
            {
                Debug.Log("Կ�ײ��㣬�޷��뿪����ǰ��"+gamePlayerLogic.GetItemCount("key"));
            }
        }
    }

    //���������󣬼�����һ������
    public void LoadNextScene()
    {
        GameManager.Instance.NextLevel();//������һ���ؿ�
    }
}
