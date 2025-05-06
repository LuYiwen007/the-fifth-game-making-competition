using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//这个类只是用于控制游戏时玩家颜料指示条，并非用于控制UI切换
//建议创建Canvas后挂载在里面的GamePanel下
public class GameUI : MonoBehaviour
{

    public static GameUI Instance; 
    public GamePlayerLogic player;

    //加速标识
    public GameObject wjiasubiaoshi;//白色变化加速标识
    public GameObject bjiasubiaoshi;//黑色变化加速标识

    //颜料值
    private float originalWidth;//指示条原始宽度
    public Image blackpaintmask;
    public Image whitepaintmask; 

    //物品栏物品数量
    public TextMeshProUGUI WhiteBottle;
    public TextMeshProUGUI BlackBottle;
    public TextMeshProUGUI GeryBottle;
    public TextMeshProUGUI Key;

    //生命值
    private float originalhpwidth;//血条原始宽度
    public Image HPMask;

    public void Awake()
    {
        originalWidth = blackpaintmask.rectTransform.rect.height;
        originalhpwidth = HPMask.rectTransform.rect.width;
    }

    private void Update()
    {
        Showjiasubiaoshi();//更新是否显示加速标识
    }
    public void FixedUpdate()
    {
        ////更新物品栏物品数量
        //if (Inventory.Instance != null)
        //{
        //    int whiteBottleCount = Inventory.Instance.GetItemCount("WhitePaintBottle");
        //    int blackBottleCount = Inventory.Instance.GetItemCount("BlackPaintBottle");
        //    int geryBottleCount = Inventory.Instance.GetItemCount("GrayPaintBottle");
        //    int keyCount = Inventory.Instance.GetItemCount("Key");
        //    WhiteBottle.text = whiteBottleCount.ToString();
        //    BlackBottle.text = blackBottleCount.ToString();
        //    GeryBottle.text = geryBottleCount.ToString();
        //    Key.text = keyCount.ToString();
        //}
    }

    //设置黑白颜料指示条ui
    public void WhitePaintValueUI()
    {
        whitepaintmask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, player.GetWhitePaintValue()/ player.GetMaxPaintValue() * originalWidth);
    }
    public void BlackPaintValueUI()
    {
        blackpaintmask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, player.GetBlackPaintValue()/ player.GetMaxPaintValue() * originalWidth);
    }
    
    //设置血条UI
    public void HPUI()
    {
        float percent = (float)player.currenthp / player.maxhp;
        HPMask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, percent * originalhpwidth);
    }

    //加速标识控制器
    public void Showjiasubiaoshi()
    {
        if (Inventory.Instance.HasItem("BlackPaintBottle") && player.GetCurrentAreaType() == GamePlayerLogic.AreaType.Black) 
        {
            bjiasubiaoshi.SetActive(true);
            wjiasubiaoshi.SetActive(false);
        }
        else if(Inventory.Instance.HasItem("WhitePaintBottle") && player.GetCurrentAreaType() == GamePlayerLogic.AreaType.White)
        {
            bjiasubiaoshi.SetActive(false);
            wjiasubiaoshi.SetActive(true);
        }
        else if (Inventory.Instance.HasItem("WhitePaintBottle")&& Inventory.Instance.HasItem("BlackPaintBottle")&& player.GetCurrentAreaType() == GamePlayerLogic.AreaType.Black)
        {
            bjiasubiaoshi.SetActive(true);
            wjiasubiaoshi.SetActive(false);
        }
        else if (Inventory.Instance.HasItem("WhitePaintBottle") && Inventory.Instance.HasItem("BlackPaintBottle") && player.GetCurrentAreaType() == GamePlayerLogic.AreaType.White)
        {
            bjiasubiaoshi.SetActive(false);
            wjiasubiaoshi.SetActive(true);
        }
        else
        {
            wjiasubiaoshi.SetActive(false);
            bjiasubiaoshi.SetActive(false);
        }
    }

    public void OnPauseButtonClick()
    {
        UIController.Instance.SetGameState(UIController.GameState.Pause);
    }
}

