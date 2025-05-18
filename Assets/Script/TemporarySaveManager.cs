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

            // �O�̂��ߏ�����
            savedData = new GameSaveData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ClearSave()
    {
        // �� null�ł͂Ȃ��A��̐V�����f�[�^�𐶐�
        savedData = new GameSaveData();
        hasSavedData = false;
    }
}
