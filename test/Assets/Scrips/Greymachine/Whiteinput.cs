using System.Collections;
using TMPro;
using UnityEngine;

public class Whiteinput : MonoBehaviour
{
    public bool haswhitepaint = false; // �Ƿ��а�ɫ����ƿ
    public TextMeshProUGUI inputText;
    private Coroutine messageCoroutine;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GamePlayerLogic player = collision.GetComponent<GamePlayerLogic>();
            ShowMessage($"F�����ɫ���ϣ���ǰ��{Inventory.Instance.GetItemCount("WhitePaintBottle")}ƿ");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
        {
            if (Inventory.Instance.RemoveItemFromInventory("WhitePaintBottle", 1)) // �������Ʒ�����Ƴ�һ����ɫ����ƿ
            {
                ShowMessage("�ɹ������ɫ����ƿ");
                haswhitepaint = true;
            }
            else
            {
                ShowMessage("û�а�ɫ����ƿ");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HideMessageAfterDelay(2f);
        }
    }

    private void ShowMessage(string message)
    {
        if (messageCoroutine != null)
        {
            StopCoroutine(messageCoroutine);
        }
        inputText.text = message;
        inputText.gameObject.SetActive(true);
    }

    private void HideMessageAfterDelay(float delay)
    {
        if (messageCoroutine != null)
        {
            StopCoroutine(messageCoroutine);
        }
        messageCoroutine = StartCoroutine(HideMessageCoroutine(delay));
    }

    private IEnumerator HideMessageCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        inputText.gameObject.SetActive(false);
    }
}

