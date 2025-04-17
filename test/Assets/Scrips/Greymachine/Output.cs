using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Output : MonoBehaviour
{
    public Blackinput blackinput = new Blackinput();
    public Whiteinput whiteinput = new Whiteinput();
    private bool synthesis = false;
    private GamePlayerLogic player;

    private void Start()
    {
        player = GetComponent<GamePlayerLogic>();
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
                // ����ϳɳɹ�����synthesis��Ϊtrue  
                synthesis = true;//������ʡ�Ե��ϳɵ��߼���ֱ������Ϊtrue
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
