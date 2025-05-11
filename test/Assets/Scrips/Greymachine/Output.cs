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
//    //public Button synthesisbutton; //�󶨺ϳɰ�ť
//    //private bool synthesis = false;

//    //private void Enablesynthesis()
//    //{
//    //    synthesis = true;
//    //}

//    //private void Update()
//    //{
//    //    if (synthesis == true)
//    //    {
//    //        // �رպϳ����  
//    //        blackinput.hasblackpaint = false;
//    //        whiteinput.haswhitepaint = false;
//    //        // ��ӻ�ɫ����ƿ  
//    //        Inventory.Instance.AddItemToInventory("GreyPaintBottle", 1);
//    //        synthesis = false;
//    //    }
//    //}

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (blackinput.hasblackpaint && whiteinput.haswhitepaint)
//        {
//            outputText.text = "ԭ����ȫ�����Ժϳ�";
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
                ShowMessage("ԭ����ȫ����F�ϳɻ�ɫ����ƿ");
            }
            else
            {
                ShowMessage("��Ҫ��ɫ�Ͱ�ɫ����ƿ���ܺϳ�");
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
                ShowMessage("�ɹ��ϳɻ�ɫ����ƿ��");
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
