using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClearUIController : MonoBehaviour
{
    // タイトルシーンの名前（Build Settings に登録されている必要あり）
    public string titleSceneName = "TitleScene";

    // ボタンから呼び出す用の関数
    public void ReturnToTitle()
    {
        SceneManager.LoadScene(titleSceneName);
    }
}
