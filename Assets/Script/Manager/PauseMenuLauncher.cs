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

            // �Q�[�����痈�����Ƃ��L�^
            PlayerPrefs.SetInt("CameFromGameScene", 1);

            // ���Ԃ��~�߂�
            Time.timeScale = 0f;

            // OptionScene �� Additive �œǂݍ���
            SceneManager.LoadScene("OptionScene", LoadSceneMode.Additive);
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        // �����ŃJ�[�\�����\����
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
