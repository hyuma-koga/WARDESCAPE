using UnityEngine;
using UnityEngine.UI;

public class ShardUIManager : MonoBehaviour
{
    public Text shardText;

    public void UpdateShardDisplay(int collected, int total)
    {
        int remaining = Mathf.Max(total - collected, 0);

        if (shardText != null)
        {
            shardText.text = "" + remaining;
            Debug.Log($"[ShardUI] ��Text���e: {shardText.text}, �\������Ă���Text: {shardText.gameObject.name}");
        }
    }

    void Update()
    {
        // �����I�ɖ��t���[��UI�\���𓯊��i�e�X�g�p�j
        if (shardText != null && GameManager.Instance != null)
        {
            int forcedRemaining = Mathf.Max(GameManager.Instance.shardsToWin - GameManager.Instance.shardsCollected, 0);
            shardText.text = "" + forcedRemaining;
        }
    }
}
