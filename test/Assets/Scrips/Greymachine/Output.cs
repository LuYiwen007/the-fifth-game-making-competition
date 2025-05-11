//using System.Collections;
//using System.Collections.Generic;
//using TMPro;
//using UnityEngine;
//using UnityEngine.UI;

//public class Output : MonoBehaviour
//{
//    public Blackinput blackinput;
//    public Whiteinput whiteinput;
//    public TextMeshProUGUI outputText;
//    //public Button synthesisbutton; //绑定合成按钮
//    //private bool synthesis = false;

//    //private void Enablesynthesis()
//    //{
//    //    synthesis = true;
//    //}

//    //private void Update()
//    //{
//    //    if (synthesis == true)
//    //    {
//    //        // 关闭合成面板  
//    //        blackinput.hasblackpaint = false;
//    //        whiteinput.haswhitepaint = false;
//    //        // 添加灰色颜料瓶  
//    //        Inventory.Instance.AddItemToInventory("GreyPaintBottle", 1);
//    //        synthesis = false;
//    //    }
//    //}

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (blackinput.hasblackpaint && whiteinput.haswhitepaint)
//        {
//            outputText.text = "原料齐全，可以合成";
//            if (Input.GetKeyDown(KeyCode.F))
//            {
//                Inventory.Instance.AddItemToInventory("GrayPaintBottle", 1);
//            }
//        }
//    }
//}
using System.Collections;
using TMPro;
using UnityEngine;

public class Output : MonoBehaviour
{
    public Blackinput blackinput;
    public Whiteinput whiteinput;
    public TextMeshProUGUI outputText;
    private Coroutine messageCoroutine;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (blackinput.hasblackpaint && whiteinput.haswhitepaint)
            {
                ShowMessage("原料齐全，按F合成灰色颜料瓶");
            }
            else
            {
                ShowMessage("需要黑色和白色颜料瓶才能合成");
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && blackinput.hasblackpaint && whiteinput.haswhitepaint)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Inventory.Instance.AddItemToInventory("GrayPaintBottle", 1);
                blackinput.hasblackpaint = false;
                whiteinput.haswhitepaint = false;
                ShowMessage("成功合成灰色颜料瓶！");
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
        outputText.text = message;
        outputText.gameObject.SetActive(true);
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
        outputText.gameObject.SetActive(false);
    }
}
