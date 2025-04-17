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
            Debug.Log("原料齐全，可以合成");
            // 显示ui：按F键合成 （未添加）  
            if (Input.GetKeyDown(KeyCode.F))
            {
                // 显示合成面板  
                // 如果合成成功，将synthesis改为true  
                synthesis = true;//这里先省略掉合成的逻辑，直接设置为true
                if (synthesis == true)
                {
                    // 关闭合成面板  
                    blackinput.hasblackpaint = false;
                    whiteinput.haswhitepaint = false;
                    player.AddItemToInventory("GreyPaintBottle", 1); // 添加灰色颜料瓶  
                }
            }
        }
    }
}
