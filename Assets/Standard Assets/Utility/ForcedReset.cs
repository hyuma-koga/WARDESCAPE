using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class ForcedReset : MonoBehaviour
{
    private void Update()
    {
        if (CrossPlatformInputManager.GetButtonDown("ResetObject"))
        {
            // ※ Application.LoadLevelAsync は古いAPIなので SceneManager を使う方が良い
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }
}
