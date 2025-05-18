using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuLauncher : MonoBehaviour
{
    public static PauseMenuLauncher Instance { get; private set; }

    private bool isPaused = false;

    private void Awake()
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
        {
            isPaused = true;

            // ゲームから来たことを記録
            PlayerPrefs.SetInt("CameFromGameScene", 1);

            // 時間を止める
            Time.timeScale = 0f;

            // OptionScene を Additive で読み込み
            SceneManager.LoadScene("OptionScene", LoadSceneMode.Additive);
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        // ここでカーソルを非表示に
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
