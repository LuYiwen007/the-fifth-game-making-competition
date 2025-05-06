using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Whiteinput : MonoBehaviour
{
    private TextMeshProUGUI inputText;
    public bool haswhitepaint = false; // �Ƿ��а�ɫ����ƿ

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GamePlayerLogic player = collision.GetComponent<GamePlayerLogic>();
            inputText.text = $"F�����ɫ���ϣ���ǰ��{Inventory.Instance.GetItemCount("WhitePaintBottle")}ƿ";
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (Inventory.Instance.RemoveItemFromInventory("WhitePaintBottle", 1))// �������Ʒ�����Ƴ�һ����ɫ����ƿ
                {
                    inputText.text = "�ɹ������ɫ����ƿ��";
                    haswhitepaint = true;
                }
                else
                {
                    inputText.text = "û�а�ɫ����ƿ";
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
