using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Output : MonoBehaviour
{
    public Blackinput blackinput;
    public Whiteinput whiteinput;
    public TextMeshProUGUI outputText;
    //public Button synthesisbutton; //�󶨺ϳɰ�ť
    //private bool synthesis = false;

    //private void Enablesynthesis()
    //{
    //    synthesis = true;
    //}

    //private void Update()
    //{
    //    if (synthesis == true)
    //    {
    //        // �رպϳ����  
    //        blackinput.hasblackpaint = false;
    //        whiteinput.haswhitepaint = false;
    //        // ��ӻ�ɫ����ƿ  
    //        Inventory.Instance.AddItemToInventory("GreyPaintBottle", 1);
    //        synthesis = false;
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (blackinput.hasblackpaint && whiteinput.haswhitepaint)
        {
            outputText.text = "ԭ����ȫ�����Ժϳ�";
            if (Input.GetKeyDown(KeyCode.F))
            {
                Inventory.Instance.AddItemToInventory("GrayPaintBottle", 1);
            }
        }
    }
}
