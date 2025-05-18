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

        // PlayerPrefs �ɉ����Ė߂�{�^���\��
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
            // Additive�ŊJ���ꂽ OptionScene ���A�����[�h�i�Q�[���ɖ߂�j
            PlayerPrefs.DeleteKey("CameFromGameScene");

            // OptionScene��������āA�Q�[���ĊJ
            SceneManager.UnloadSceneAsync("OptionScene");

            // �Q�[�����̃|�[�Y����
            PauseMenuLauncher.Instance?.ResumeGame();
        }
        else
        {
            // �ʏ탍�[�h���ꂽ OptionScene �� �^�C�g���ɖ߂�iLoadScene�j
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

        // �� GameManager�𖾎��I�ɔj��
        if (GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject);
        }

        SceneManager.LoadScene("TitleScene");
    }


}