using UnityEngine;
using UnityEngine.UI;

public class HintUIController : MonoBehaviour
{
    public GameObject flashlightHintUI; // ライトのヒント
    public GameObject minimapHintUI;    // ミニマップのヒント

    private bool flashlightHintVisible = false;
    private bool minimapHintVisible = false;

    public void ShowHints()
    {
        flashlightHintUI.SetActive(true);
        minimapHintUI.SetActive(true);
        flashlightHintVisible = true;
        minimapHintVisible = true;
    }

    void Update()
    {
        if (flashlightHintVisible && Input.GetKeyDown(KeyCode.R))
        {
            flashlightHintUI.SetActive(false);
            flashlightHintVisible = false;
        }

        if (minimapHintVisible && Input.GetKeyDown(KeyCode.LeftShift))
        {
            minimapHintUI.SetActive(false);
            minimapHintVisible = false;
        }
    }
}
