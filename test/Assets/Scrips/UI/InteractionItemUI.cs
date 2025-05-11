using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InteractionItemUI : MonoBehaviour
{
    //物品栏物品数量
    public TextMeshProUGUI WhiteBottle;
    public TextMeshProUGUI BlackBottle;
    public TextMeshProUGUI GeryBottle;
    public TextMeshProUGUI Key;

    // 物品名称：UI预制体（需带Image和Text组件）
    public GameObject labelPrefab;
    // UI在角色右侧的水平偏移距离
    public float labelOffsetX = 1.5f;
    // 多个标签的垂直间距
    public float labelSpacing = 0.3f;
    // 交互检测的最大距离
    public float interactRange = 2.5f;
    // 可交互物体的Layer涂层
    public LayerMask interactableLayer;
    // 当前选中物体的高亮颜色
    public Color highlightColor = Color.white;
    // 未选中物体的常规颜色
    public Color normalColor = new Color(1,1,1,0.5f);

    // 当前场景中已生成的标签UI对象
    private List<GameObject> currentLabels = new List<GameObject>();
    // 当前靠近的可交互物体列表
    private List<GameObject> nearbyItems = new List<GameObject>();
    // 当前选中的物体索引
    private int selectedIndex = 0;
    // 角色自身的Transform
    private Transform player;

    void Start()
    {
        player = this.transform; // 角色自身
    }

    void Update()
    {
        // 每帧更新：检测附近物体、刷新UI、处理选择和交互输入
        UpdateNearbyItems();
        UpdateLabels();
        HandleSelectionInput();
        HandleInteractionInput();
    }

    // 检测角色周围的可交互物体
    void UpdateNearbyItems()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(player.position, interactRange, interactableLayer);
        nearbyItems.Clear();
        foreach (var hit in hits)
        {
            // 只检测带有Pickable或greymachine标签的物体
            if (hit.CompareTag("Pickable") || hit.CompareTag("greymachine"))
            {
                nearbyItems.Add(hit.gameObject);
            }
        }
        // 防止索引越界
        if (selectedIndex >= nearbyItems.Count) selectedIndex = 0;
    }

    // 刷新并显示所有可交互物体的标签UI
    void UpdateLabels()
    {
        // 先清理旧的UI
        foreach (var label in currentLabels)
            Destroy(label);
        currentLabels.Clear();

        // 为每个物体生成一个标签
        for (int i = 0; i < nearbyItems.Count; i++)
        {
            GameObject item = nearbyItems[i];
            // 实例化标签UI
            GameObject label = Instantiate(labelPrefab, GetLabelWorldPosition(i), Quaternion.identity, null);
            label.transform.SetParent(null);
            label.transform.position = GetLabelWorldPosition(i);
            // 设置标签文本
            var text = label.GetComponentInChildren<Text>();
            if (item.CompareTag("greymachine"))
                text.text = "使用";
            else
                text.text = item.name;
            // 设置标签颜色和高亮
            var img = label.GetComponent<Image>();
            if (img != null)
                img.color = (i == selectedIndex) ? highlightColor : normalColor;
            // 设置高亮边框（需有Outline组件）
            var outline = label.GetComponent<Outline>();
            if (outline != null)
                outline.enabled = (i == selectedIndex);
            currentLabels.Add(label);
        }
    }

    // 计算每个标签的世界坐标（右侧依次向下排列）
    Vector3 GetLabelWorldPosition(int index)
    {
        Vector3 basePos = player.position + Vector3.right * labelOffsetX;
        basePos += Vector3.down * labelSpacing * index;
        return basePos;
    }

    // 处理玩家的选择输入（鼠标滚轮和上下键切换选中物体）
    void HandleSelectionInput()
    {
        if (nearbyItems.Count == 0) return;
        // 鼠标滚轮切换
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0.01f)
            selectedIndex = (selectedIndex - 1 + nearbyItems.Count) % nearbyItems.Count;
        else if (scroll < -0.01f)
            selectedIndex = (selectedIndex + 1) % nearbyItems.Count;
        // 键盘上下键切换
        if (Input.GetKeyDown(KeyCode.UpArrow))
            selectedIndex = (selectedIndex - 1 + nearbyItems.Count) % nearbyItems.Count;
        if (Input.GetKeyDown(KeyCode.DownArrow))
            selectedIndex = (selectedIndex + 1) % nearbyItems.Count;
    }

    // 处理玩家的交互输入（F键拾取或使用）
    void HandleInteractionInput()
    {
        if (nearbyItems.Count == 0) return;
        if (Input.GetKeyDown(KeyCode.F))
        {
            GameObject target = nearbyItems[selectedIndex];
            if (target.CompareTag("Pickable"))
            {
                // 假设有AddItemToInventory接口，拾取物品加入背包
                Inventory.Instance.AddItemToInventory(target.name, 1);
                Destroy(target); // 拾取后物体消失
            }
            else if (target.CompareTag("greymachine"))
            {
                // 进入greymachine的UI界面（需实现GreyMachineUI脚本）
                var gm = target.GetComponent<GreyMachineUI>();
                if (gm != null) gm.OpenUI();
            }
        }
    }

    // 在Scene视图中显示交互检测范围（辅助开发）
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
} 