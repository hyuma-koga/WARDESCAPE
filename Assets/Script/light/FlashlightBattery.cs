using UnityEngine;
using UnityEngine.UI;

public class FlashlightBattery : MonoBehaviour
{
    public Light flashlight;             // �����d���̃��C�g
    public float maxBattery = 100f;      // �ő�o�b�e���[��
    public float currentBattery;         // ���݂̃o�b�e���[��
    public float drainRate = 20f;        // �b������̏����
    public Slider batterySlider;         //�X�}�zUI�̃X���C�_�[���h���b�O�Őݒ�
    public Text batteryText;

    public bool isOn = true;

    private void Start()
    {
        currentBattery = maxBattery;
        flashlight.enabled = isOn;
    }

    private void Update()
    {
        // R�L�[�� ON/OFF �؂�ւ��i�o�b�e���[������Ƃ��̂݁j
        if (Input.GetKeyDown(KeyCode.R) && currentBattery > 0f)
        {
            isOn = !isOn;
        }

        // �_����Ԃ̏���
        if (isOn && currentBattery > 0f)
        {
            currentBattery -= drainRate * Time.deltaTime;
            currentBattery = Mathf.Clamp(currentBattery, 0f, maxBattery);

            flashlight.enabled = true;
            
            if (currentBattery <= 10f)
            {
                float ratio = currentBattery / 10f; // 0�`1
                flashlight.intensity = Mathf.Lerp(0.1f, 1.5f, ratio); // ��`��
            }
            else
            {
               flashlight.intensity = 1.5f;
         }

            if (currentBattery <= 0f)
            {
                isOn = false;
                flashlight.enabled = false;
            }
        }
        else
        {
            flashlight.enabled = false;
        }

        //�X�}�z�̃X���C�_�[UI����
        if (batterySlider != null)
        {
            batterySlider.value = currentBattery;
        }
        //�X�}�z�̃e�L�X�gUI����
        if(batteryText != null)
        {
            batteryText.text = Mathf.CeilToInt(currentBattery) + "%";
        }
    }

    public void RechargeBattery(float amount)
    {
        currentBattery += amount;
        currentBattery = Mathf.Clamp(currentBattery, 0f, maxBattery);

        Debug.Log("Battery recharged. Current: " + currentBattery);

        // �o�b�e���[���񕜂��AisOn��true�Ȃ烉�C�g�ē_��
        if (currentBattery > 0f && isOn)
        {
            flashlight.enabled = true;
        }
    }
}
