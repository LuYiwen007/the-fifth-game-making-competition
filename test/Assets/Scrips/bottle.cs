using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Bottle : MonoBehaviour, IPickupable
{
    public enum BottleType
    {
        Black,
        White,
        Gray
    }
    public BottleType bottleType;
    private int amount = 1;
    public int GetItemAmount()
    {
        return amount;
    }
    public string GetItemName()
    {
        switch (bottleType)
        {
            case BottleType.Black:
                return "BlackPaintBottle";//黑色颜料瓶
            case BottleType.White:
                return "WhitePaintBottle";//白色颜料瓶
            case BottleType.Gray:
                return "GrayPaintBottle";//灰色颜料瓶
            default:
                return "UnknownBottle";//未知颜料瓶
        }
    }
    public void OnPickup(GameObject player)
    {
        GamePlayerLogic playerLogic = player.GetComponent<GamePlayerLogic>();
        if (playerLogic != null)
        {
            if (Inventory.Instance.AddItemToInventory(GetItemName(), GetItemAmount()))
            {
                
                // 拾取成功，销毁物品
                Destroy(gameObject);
                Debug.Log(Inventory.Instance.GetInventory());
            }
            else
            {
                // 拾取失败，物品栏已满
                Debug.Log("物品栏已满，无法拾取 " + GetItemName());
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Bottle bottle = GetComponent<Bottle>();
            if (bottle != null)
            {
                bottle.OnPickup(collision.gameObject); // 调用拾取逻辑
            }
        }
    }

}