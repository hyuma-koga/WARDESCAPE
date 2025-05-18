using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject player;

    public int shardsCollected = 0;
    public int shardsToWin = 1;
    public FlashlightBattery flashlightBattery; // インスペクターでアタッチ

    public ShardUIManager shardUIManager;

    private Vector3 playerStartPosition;
    void Start()
    {

        if(player != null)
        {
            playerStartPosition = player.transform.position;
            Debug.Log("GameManagerプレイヤー初期位置記録:" + playerStartPosition);
        }

        Debug.Log($"[Start] shardsCollected={shardsCollected}, shardsToWin={shardsToWin}, hasSavedData={TemporarySaveManager.Instance?.hasSavedData}");


        if (TemporarySaveManager.Instance != null && TemporarySaveManager.Instance.hasSavedData)
        {
            LoadState();
        }

        //シャード数UIを常に更新
        if (shardUIManager != null)
        {
            shardUIManager.UpdateShardDisplay(shardsCollected, shardsToWin);
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }




    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);

            if (PlayerPrefs.GetInt("CameFromTitleScene", 0) == 1)
            {
                //Awake でセーブデータを完全クリア！
                TemporarySaveManager.Instance?.ClearSave();

                //hardsCollected のリセット
                shardsCollected = 0;

                //ラグ削除
                PlayerPrefs.DeleteKey("CameFromTitleScene");
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }





    public void CollectShard()
    {
        shardsCollected++;
        Debug.Log("Shard Collected! Total:" + shardsCollected);

        //残り数をUIに反映
        if (shardUIManager != null)
        {
            shardUIManager.UpdateShardDisplay(shardsCollected, shardsToWin);
        }
    }

    public bool HasAllShards()
    {
        return shardsCollected >= shardsToWin;
    }

    public void SaveState()
    {
        Debug.Log("SaveState() が呼ばれた");
        if (TemporarySaveManager.Instance == null || player == null)
        {
            Debug.LogWarning("SaveState: 条件が未満足");
            return;
        }
            

        GameSaveData data = new GameSaveData();
        data.playerPosition = player.transform.position;

        // これを追加！
        data.shardsCollected = this.shardsCollected;

        TemporarySaveManager.Instance.savedData = data;
        TemporarySaveManager.Instance.hasSavedData = true;

        Debug.Log("Save Complete:" + data.playerPosition);
    }

    public void LoadState()
    {
        if(TemporarySaveManager.Instance == null || !TemporarySaveManager.Instance.hasSavedData) return;

        GameSaveData data = TemporarySaveManager.Instance.savedData;

        if(player != null)
        {
            player.transform.position = data.playerPosition;
            Debug.Log("Load Complete:" + data.playerPosition);
        }
        shardsCollected = data.shardsCollected;

        //UI更新（ロード後に確実に最新化）
        if (shardUIManager != null)
        {
            shardUIManager.UpdateShardDisplay(shardsCollected, shardsToWin);
        }
    }
    public Vector3 GetPlayerStartPosition()
    {
        return playerStartPosition;
    }
}