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
            Debug.Log($"[ShardUI] 実Text内容: {shardText.text}, 表示されているText: {shardText.gameObject.name}");
        }
    }

    void Update()
    {
        // 強制的に毎フレームUI表示を同期（テスト用）
        if (shardText != null && GameManager.Instance != null)
        {
            int forcedRemaining = Mathf.Max(GameManager.Instance.shardsToWin - GameManager.Instance.shardsCollected, 0);
            shardText.text = "" + forcedRemaining;
        }
    }
}
