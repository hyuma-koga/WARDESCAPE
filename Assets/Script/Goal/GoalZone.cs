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
            exitButton.SetActive(false); // ← 最初は非表示
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.Instance.HasAllShards())
        {
            Debug.Log("You Win!");

            if (goalUIText != null)
                goalUIText.SetActive(true);

            if (exitButton != null)
                exitButton.SetActive(true); // ← ゴール時に表示！

            // カーソルを表示・解放（UI操作のため）
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    // UIのExitボタンに割り当てる
    public void OnExitToTitle()
    {
        SceneManager.LoadScene("TitleScene"); // 実際のタイトルシーン名に置き換えてください
    }
}
