using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClearUIController : MonoBehaviour
{
    // �^�C�g���V�[���̖��O�iBuild Settings �ɓo�^����Ă���K�v����j
    public string titleSceneName = "TitleScene";

    // �{�^������Ăяo���p�̊֐�
    public void ReturnToTitle()
    {
        SceneManager.LoadScene(titleSceneName);
    }
}
