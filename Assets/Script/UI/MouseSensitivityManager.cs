using UnityEngine;
using UnityEngine.UI;

public class MouseSensitivityManager : MonoBehaviour
{
    public Slider sensitivitySlider;
    public PlayerController playerController;

    private void Start()
    {
        //•Û‘¶‚³‚ê‚Ä‚¢‚ê‚Î•œŒ³
        float savedSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 1.0f);
        sensitivitySlider.value = savedSensitivity;
        ApplySensitivity(savedSensitivity);

        //ƒŠƒXƒi[“o˜^
        sensitivitySlider.onValueChanged.AddListener(ApplySensitivity);
    }

    public void ApplySensitivity(float value)
    {
        PlayerPrefs.SetFloat("MouseSensitivity", value);
        if(playerController != null)
        {
            playerController.mouseSensitivity = value; //”CˆÓ‚Ì•Ï”–¼‚É‡‚í‚¹‚é
        }
    }
}
