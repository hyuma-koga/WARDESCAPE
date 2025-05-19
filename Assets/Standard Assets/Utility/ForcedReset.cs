using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class ForcedReset : MonoBehaviour
{
    private void Update()
    {
        if (CrossPlatformInputManager.GetButtonDown("ResetObject"))
        {
            // �� Application.LoadLevelAsync �͌Â�API�Ȃ̂� SceneManager ���g�������ǂ�
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }
}
