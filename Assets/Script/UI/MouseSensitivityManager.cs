using UnityEngine;
using UnityEngine.UI;

public class MouseSensitivityManager : MonoBehaviour
{
    public Slider sensitivitySlider;
    public PlayerController playerController;

    private void Start()
    {
        //保存されていれば復元
        float savedSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 1.0f);
        sensitivitySlider.value = savedSensitivity;
        ApplySensitivity(savedSensitivity);

        //リスナー登録
        sensitivitySlider.onValueChanged.AddListener(ApplySensitivity);
    }

    public void ApplySensitivity(float value)
    {
        PlayerPrefs.SetFloat("MouseSensitivity", value);
        if(playerController != null)
        {
            playerController.mouseSensitivity = value; //任意の変数名に合わせる
        }
    }
}
