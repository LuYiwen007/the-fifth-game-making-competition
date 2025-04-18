using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Output : MonoBehaviour
{
    public Blackinput blackinput = new Blackinput();
    public Whiteinput whiteinput = new Whiteinput();
    private bool synthesis = false;
    private GamePlayerLogic player;
    public Button synthesisbutton; //�󶨺ϳɰ�ť

    private void Start()
    {
        player = GetComponent<GamePlayerLogic>();
    }
    
    private void Enablesynthesis()
    {
        synthesis = true;
    }

    private void Update()
    {
        if (synthesis == true)
        {
            // �رպϳ����  
            blackinput.hasblackpaint = false;
            whiteinput.haswhitepaint = false;
            // ��ӻ�ɫ����ƿ  
            Inventory.Instance.AddItemToInventory("GreyPaintBottle", 1);
            synthesis = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (blackinput.hasblackpaint && whiteinput.haswhitepaint)
        {
            Debug.Log("ԭ����ȫ�����Ժϳ�");
            // ��ʾui����F���ϳ� ��δ��ӣ�  
            if (Input.GetKeyDown(KeyCode.F))
            {
                // ��ʾ�ϳ����  
                synthesisbutton.gameObject.SetActive(true); // ��ʾ�ϳɰ�ť���ϳɰ�ť����Enablesynthesis()����
            }
        }
    }
}
