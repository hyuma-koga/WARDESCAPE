using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TitleManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clickSound;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (!audioSource.isPlaying)
            audioSource.Play();
        //カーソル表示
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

    }

    public void StartGame()
    {

        // カーソルを非表示＆ロック（FPS操作用）
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        PlayerPrefs.SetInt("CameFromTitleScene", 1);
        SceneManager.LoadScene("GameScene");
    }

    public void Option()
    {
        SceneManager.LoadScene("OptionScene");
    }

    public void OpenOptionFromGame()
    {
        PlayerPrefs.SetString("PreviousScene", "TitleScene");
        SceneManager.LoadScene("OptionScene");
    }
}
