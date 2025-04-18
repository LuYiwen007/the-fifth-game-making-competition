using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }
    private Dictionary<string, int> inventory = new Dictionary<string, int>(); // 背包字典，键为物品名称，值为数量
    public UnityEvent<string, int> OnInventoryChanged; // 参数：物品名称，当前数量
    private int maxInventorySize = 20; // 背包最大容量

    private void Awake()//初始化Instance
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // 背包系统相关方法
    public void InitializeInventory()//初始化背包函数
    {
        // 添加初始物品
        AddItemToInventory("BlackPaintBottle", 0);
        AddItemToInventory("WhitePaintBottle", 0);
        AddItemToInventory("GrayPaintBottle", 0);
    }

    public bool AddItemToInventory(string itemName, int amount = 1)//添加物品到背包
    {
        // 检查背包是否已满
        if (GetInventoryItemCount() >= maxInventorySize)
        {
            //Debug.Log("背包已满，无法添加物品");
            return false;
        }

        // 如果物品已存在，增加数量
        if (inventory.ContainsKey(itemName))
        {
            inventory[itemName] += amount;
        }
        else
        {
            // 否则添加新物品
            inventory[itemName] = amount;
        }

        // 触发背包变化事件
        OnInventoryChanged?.Invoke(itemName, inventory[itemName]);
        return true;
    }

    public bool RemoveItemFromInventory(string itemName, int amount = 1)//从背包移除物品
    {
        // 检查物品是否存在
        if (!inventory.ContainsKey(itemName))
        {
            //Debug.Log($"背包中没有{itemName}");
            return false;
        }

        // 检查物品数量是否足够
        if (inventory[itemName] < amount)
        {
            //Debug.Log($"背包中{itemName}数量不足");
            return false;
        }

        // 减少物品数量
        inventory[itemName] -= amount;

        // 如果物品数量为0，从背包中移除
        if (inventory[itemName] <= 0)
        {
            inventory.Remove(itemName);
        }

        // 触发背包变化事件
        OnInventoryChanged?.Invoke(itemName, inventory.ContainsKey(itemName) ? inventory[itemName] : 0);
        return true;
    }

    public int GetItemCount(string itemName)//获取物品数量
    {
        if (inventory.ContainsKey(itemName))
        {
            return inventory[itemName];
        }
        return 0;
    }

    public bool HasItem(string itemName, int amount = 1)//检查是否有足够数量的物品
    {
        return GetItemCount(itemName) >= amount;
    }

    public int GetInventoryItemCount()//获取背包中物品总数
    {
        int totalCount = 0;
        foreach (var item in inventory)
        {
            totalCount += item.Value;
        }
        return totalCount;
    }

    public int GetMaxInventorySize()//获取背包最大容量
    {
        return maxInventorySize;
    }

    public void SetMaxInventorySize(int size)//设置背包最大容量
    {
        maxInventorySize = size;
    }

    public Dictionary<string, int> GetInventory()//获取整个背包
    {
        return new Dictionary<string, int>(inventory);
    }




}
