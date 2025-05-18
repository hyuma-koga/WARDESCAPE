using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalZone : MonoBehaviour
{
    public GameObject goalUIText;
    public GameObject exitButton;

    private void Start()
    {
        if (goalUIText != null)
            goalUIText.SetActive(false);

        if (exitButton != null)
            exitButton.SetActive(false); // �� �ŏ��͔�\��
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.Instance.HasAllShards())
        {
            Debug.Log("You Win!");

            if (goalUIText != null)
                goalUIText.SetActive(true);

            if (exitButton != null)
                exitButton.SetActive(true); // �� �S�[�����ɕ\���I

            // �J�[�\����\���E����iUI����̂��߁j
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    // UI��Exit�{�^���Ɋ��蓖�Ă�
    public void OnExitToTitle()
    {
        SceneManager.LoadScene("TitleScene"); // ���ۂ̃^�C�g���V�[�����ɒu�������Ă�������
    }
}
