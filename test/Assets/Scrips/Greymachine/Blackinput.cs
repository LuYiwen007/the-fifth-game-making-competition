using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackinput : MonoBehaviour
{
    public bool hasblackpaint = false; // �Ƿ��к�ɫ����ƿ
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GamePlayerLogic player = collision.GetComponent<GamePlayerLogic>();
            //չʾui��ѯ������Ƿ�����ɫ���ϣ���Fȷ������δ��ӣ�
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (Inventory.Instance.RemoveItemFromInventory("BlackPaintBottle", 1))// �������Ʒ�����Ƴ�һ����ɫ����ƿ
                {
                    Debug.Log("�ɹ�ʹ�ú�ɫ����ƿ");
                    //չʾui:�ɹ������ɫ����ƿ����ʾ�����к�ɫ����ƿ��һ��δ��ӣ�
                    hasblackpaint = true;
                }
                else
                {
                    Debug.Log("û�к�ɫ����ƿ");
                    //չʾui:��ɫ����ƿ���㣨δ��ӣ�
                    return;
                }
            }
        } 
    } 
}
    
