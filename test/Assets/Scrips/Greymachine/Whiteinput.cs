using System.Collections;
using TMPro;
using UnityEngine;

public class Whiteinput : MonoBehaviour
{
    public bool haswhitepaint = false; // 是否有白色颜料瓶
    public TextMeshProUGUI inputText;
    private Coroutine messageCoroutine;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GamePlayerLogic player = collision.GetComponent<GamePlayerLogic>();
            ShowMessage($"F放入白色颜料，当前有{Inventory.Instance.GetItemCount("WhitePaintBottle")}瓶");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
        {
            if (Inventory.Instance.RemoveItemFromInventory("WhitePaintBottle", 1)) // 从玩家物品栏中移除一个白色颜料瓶
            {
                ShowMessage("成功放入白色颜料瓶");
                haswhitepaint = true;
            }
            else
            {
                ShowMessage("没有白色颜料瓶");
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

