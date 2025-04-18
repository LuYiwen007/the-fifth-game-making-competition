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
    public Button synthesisbutton; //绑定合成按钮

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
            // 关闭合成面板  
            blackinput.hasblackpaint = false;
            whiteinput.haswhitepaint = false;
            // 添加灰色颜料瓶  
            Inventory.Instance.AddItemToInventory("GreyPaintBottle", 1);
            synthesis = false;
        }
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
                synthesisbutton.gameObject.SetActive(true); // 显示合成按钮，合成按钮将绑定Enablesynthesis()方法
            }
        }
    }
}
