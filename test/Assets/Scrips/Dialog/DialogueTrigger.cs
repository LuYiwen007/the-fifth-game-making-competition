using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("对话设置")]
    [TextArea(3, 10)]
    [SerializeField] private string dialogueText;
    [SerializeField] private bool triggerOnce = true;
    [SerializeField] private bool autoHide = true;
    [SerializeField] private float autoHideDelay = 3f;

    private bool hasTriggered = false;
    private float hideTimer = 0f;
    private bool isWaitingToHide = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!triggerOnce || !hasTriggered)
            {
                DialogueSystem.Instance.ShowDialogue(dialogueText);
                hasTriggered = true;
                
                if (autoHide)
                {
                    isWaitingToHide = true;
                    hideTimer = autoHideDelay;
                }
            }
        }
    }

    private void Update()
    {
        if (isWaitingToHide)
        {
            hideTimer -= Time.deltaTime;
            if (hideTimer <= 0)
            {
                DialogueSystem.Instance.HideDialogue();
                isWaitingToHide = false;
            }
        }

        // 按空格键可以跳过打字效果
        if (Input.GetKeyDown(KeyCode.Space) && DialogueSystem.Instance.IsTyping())
        {
            DialogueSystem.Instance.CompleteCurrentText();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !autoHide)
        {
            DialogueSystem.Instance.HideDialogue();
        }
    }

    // 用于编辑器中重置触发状态
    public void ResetTrigger()
    {
        hasTriggered = false;
    }
} 