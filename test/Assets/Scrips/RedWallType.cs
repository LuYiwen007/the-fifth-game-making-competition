using UnityEngine;

// ���Ա���ɫ���ϴ����ĺ�ɫǽ��
public class BreakableRedWall : RedWallBase
{
    protected override void HandleCollision(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isTemporaryGray)
            {
                // ���ǽ���ǻ�ɫ�ģ���ֹ���ͨ��
                BlockPlayer(other.gameObject);
            }
            // ����ǻ�ɫ�ģ��������ͨ��
        }
    }
}

// ���ܱ������ĺ�ɫǽ��
public class UnbreakableRedWall : RedWallBase
{
    // ���һ����־�������Ӿ�״̬
    private bool isVisuallyGray = false;

    protected override void HandleCollision(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // ��ֹ���ͨ��
            BlockPlayer(other.gameObject);
        }
    }

    // ���Ǳ�ҷ�������ֻ�ı���ۣ����ı���Ϊ
    public override void OnTemporaryGrayArea()
    {

        isVisuallyGray = true;
        UpdateWallColor(new Color(0.5f, 0.5f, 0.5f, 1f));
    }

    public override void OnTemporaryGrayAreaEnd()
    {
        // �ָ�ԭʼ��ɫ
        isVisuallyGray = false;
        UpdateWallColor(originalColor);
    }

    // �ṩһ�����������ǽ�ڵ��Ӿ�״̬
    public bool IsVisuallyGray()
    {
        return isVisuallyGray;
    }
}