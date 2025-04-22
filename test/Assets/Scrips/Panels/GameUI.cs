using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static GamePlayerLogic;

//这个类只是用于控制游戏时玩家颜料指示条，并非用于控制UI切换
//建议创建Canvas后挂载在里面的GamePanel下
public class GameUI : MonoBehaviour
{
    public static GameUI Instance;

    //下面这两个mask就是创建一个长方体来遮挡指示条
    //通过减小mask的高度来减小指示值显示区域，mask的锚点需要设置在血条底部
    //可以试试指针能不能和mask绑定在一起移动
    public Image blackpaintmask;
    public Image whitepaintmask;

    public float originalHeight;//指示条原始高度
    public GameObject wjiasubiaoshi;//白色变化加速标识
    public GameObject bjiasubiaoshi;//黑色变化加速标识
    public GamePlayerLogic player;

    void Awake()
    {
        Instance = this;
        originalHeight = blackpaintmask.rectTransform.rect.height;
        wjiasubiaoshi.SetActive(false);
    }

    private void Update()
    {
        Showjiasubiaoshi();//更新是否显示加速标识
    }

    //设置黑白颜料指示条ui
    public void WhitePaintValueUI(float Percentofwhitepaintvalue)
    {
        whitepaintmask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Percentofwhitepaintvalue * originalHeight);
    }
    public void BlackPaintValueUI(float Percentofblackpaintvalue)
    {
        blackpaintmask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Percentofblackpaintvalue * originalHeight);
    }
    
    public void Showjiasubiaoshi()
    {
        if (Inventory.Instance.HasItem("BlackPaintBottle") && player.GetCurrentAreaType() == AreaType.Black) 
        {
            bjiasubiaoshi.SetActive(true);
        }
        else if(Inventory.Instance.HasItem("WhitePaintBottle") && player.GetCurrentAreaType() == AreaType.White)
        {
            wjiasubiaoshi.SetActive(true);
        }
        else
        {
            wjiasubiaoshi.SetActive(false);
        }
    }

    public void OnPauseButtonClick()
    {
         
    }
}
