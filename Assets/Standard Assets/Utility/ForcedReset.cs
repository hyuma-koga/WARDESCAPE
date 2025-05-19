using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class ForcedReset : MonoBehaviour
{
    private void Update()
    {
        if (CrossPlatformInputManager.GetButtonDown("ResetObject"))
        {
            // ¦ Application.LoadLevelAsync ‚ÍŒÃ‚¢API‚È‚Ì‚Å SceneManager ‚ğg‚¤•û‚ª—Ç‚¢
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }
}
