using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whiteinput : MonoBehaviour
{
    public bool haswhitepaint = false; // �Ƿ��а�ɫ����ƿ
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GamePlayerLogic player = collision.GetComponent<GamePlayerLogic>();
            //չʾui��ѯ������Ƿ�����ɫ���ϣ���Fȷ������δ��ӣ�
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (player.RemoveItemFromInventory("WhitePaintBottle", 1))// �������Ʒ�����Ƴ�һ����ɫ����ƿ
                {
                    Debug.Log("�ɹ�ʹ�ð�ɫ����ƿ");
                    //չʾui:�ɹ������ɫ����ƿ����ʾ�����а�ɫ����ƿ��һ��δ��ӣ�
                    haswhitepaint = true;
                }
                else
                {
                    Debug.Log("û�а�ɫ����ƿ");
                    //չʾui:��ɫ����ƿ���㣨δ��ӣ�
                    return;
                }
            }
        }
    }
}
