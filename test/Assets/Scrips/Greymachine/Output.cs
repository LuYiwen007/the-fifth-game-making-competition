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

    private void Synthesis()
    {
        synthesis = true;
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
                synthesisbutton.gameObject.SetActive(true); // ��ʾ�ϳɰ�ť���ϳɰ�ť����Synthesis()����
                if (synthesis == true)
                {
                    // �رպϳ����  
                    blackinput.hasblackpaint = false;
                    whiteinput.haswhitepaint = false;
                    player.AddItemToInventory("GreyPaintBottle", 1); // ��ӻ�ɫ����ƿ  
                }
            }
        }
    }
}
