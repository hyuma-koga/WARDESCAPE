using UnityEngine;

public class TemporarySaveManager : MonoBehaviour
{
    public static TemporarySaveManager Instance;
    public GameSaveData savedData;
    public bool hasSavedData = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);

            // 念のため初期化
            savedData = new GameSaveData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ClearSave()
    {
        // ★ nullではなく、空の新しいデータを生成
        savedData = new GameSaveData();
        hasSavedData = false;
    }
}
