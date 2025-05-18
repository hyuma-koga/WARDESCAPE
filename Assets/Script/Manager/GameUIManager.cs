using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    public void OpenOptionFromGame()
    {
        PlayerPrefs.SetString("PreviousScene", "GameScene");
        SceneManager.LoadScene("OptionScene");
    }
}