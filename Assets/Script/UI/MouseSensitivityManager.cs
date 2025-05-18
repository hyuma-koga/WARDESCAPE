using UnityEngine;
using UnityEngine.UI;

public class MouseSensitivityManager : MonoBehaviour
{
    public Slider sensitivitySlider;
    public PlayerController playerController;

    private void Start()
    {
        //�ۑ�����Ă���Ε���
        float savedSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 1.0f);
        sensitivitySlider.value = savedSensitivity;
        ApplySensitivity(savedSensitivity);

        //���X�i�[�o�^
        sensitivitySlider.onValueChanged.AddListener(ApplySensitivity);
    }

    public void ApplySensitivity(float value)
    {
        PlayerPrefs.SetFloat("MouseSensitivity", value);
        if(playerController != null)
        {
            playerController.mouseSensitivity = value; //�C�ӂ̕ϐ����ɍ��킹��
        }
    }
}
