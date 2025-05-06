using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Blackinput : MonoBehaviour
{
    public bool hasblackpaint = false; // �Ƿ��к�ɫ����ƿ
    public TextMeshProUGUI inputText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GamePlayerLogic player = collision.GetComponent<GamePlayerLogic>();
            inputText.text = $"F�����ɫ���ϣ���ǰ��{Inventory.Instance.GetItemCount("BlackPaintBottle")}ƿ";
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (Inventory.Instance.RemoveItemFromInventory("BlackPaintBottle", 1))// �������Ʒ�����Ƴ�һ����ɫ����ƿ
                {
                    inputText.text = "�ɹ������ɫ����ƿ";
                    hasblackpaint = true;
                }
                else
                {
                    inputText.text = "û�к�ɫ����ƿ";
                    return;
                }
            }
            Invoke("Disabletext", 2f);
        } 
    }
    private void Disabletext()
    {
        inputText = null;
    }
}
    
