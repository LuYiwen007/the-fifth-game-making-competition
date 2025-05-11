using UnityEngine;

// 可以被灰色颜料穿过的红色墙壁
public class BreakableRedWall : RedWallBase
{
    protected override void HandleCollision(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isTemporaryGray)
            {
                // 如果墙不是灰色的，阻止玩家通过
                BlockPlayer(other.gameObject);
            }
            // 如果是灰色的，允许玩家通过
        }
    }
}

// 不能被穿过的红色墙壁
public class UnbreakableRedWall : RedWallBase
{
    // 添加一个标志来跟踪视觉状态
    private bool isVisuallyGray = false;

    protected override void HandleCollision(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 阻止玩家通过
            BlockPlayer(other.gameObject);
        }
    }

    // 覆盖变灰方法，但只改变外观，不改变行为
    public override void OnTemporaryGrayArea()
    {

        isVisuallyGray = true;
        UpdateWallColor(new Color(0.5f, 0.5f, 0.5f, 1f));
    }

    public override void OnTemporaryGrayAreaEnd()
    {
        // 恢复原始颜色
        isVisuallyGray = false;
        UpdateWallColor(originalColor);
    }

    // 提供一个方法来检查墙壁的视觉状态
    public bool IsVisuallyGray()
    {
        return isVisuallyGray;
    }
}