using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class OptionManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clickSound;
    public GameObject returnToGameButton;
    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // PlayerPrefs に応じて戻るボタン表示
        bool cameFromGame = PlayerPrefs.GetInt("CameFromGameScene", 0) == 1;
        returnToGameButton.SetActive(cameFromGame);

        audioSource = GetComponent<AudioSource>();
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void Option_Exit()
    {
        bool fromGame = PlayerPrefs.GetInt("CameFromGameScene", 0) == 1;

        if (fromGame && SceneManager.sceneCount > 1)
        {
            // Additiveで開かれた OptionScene をアンロード（ゲームに戻る）
            PlayerPrefs.DeleteKey("CameFromGameScene");

            // OptionSceneだけを閉じて、ゲーム再開
            SceneManager.UnloadSceneAsync("OptionScene");

            // ゲーム内のポーズ解除
            PauseMenuLauncher.Instance?.ResumeGame();
        }
        else
        {
            // 通常ロードされた OptionScene → タイトルに戻る（LoadScene）
            Time.timeScale = 1f;
            TemporarySaveManager.Instance?.ClearSave();
            SceneManager.LoadScene("TitleScene");
        }
    }

    public void ReturnToTitle()
    {
        Time.timeScale = 1f;

        PlayerPrefs.SetInt("CameFromTitleScene", 1);

        TemporarySaveManager.Instance?.ClearSave();

        // ★ GameManagerを明示的に破棄
        if (GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject);
        }

        SceneManager.LoadScene("TitleScene");
    }


}