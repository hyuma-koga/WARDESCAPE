using UnityEngine;
using UnityEngine.UI;

public class FlashlightBattery : MonoBehaviour
{
    public Light flashlight;             // 懐中電灯のライト
    public float maxBattery = 100f;      // 最大バッテリー量
    public float currentBattery;         // 現在のバッテリー量
    public float drainRate = 20f;        // 秒あたりの消費量
    public Slider batterySlider;         //スマホUIのスライダーをドラッグで設定
    public Text batteryText;

    public bool isOn = true;

    private void Start()
    {
        currentBattery = maxBattery;
        flashlight.enabled = isOn;
    }

    private void Update()
    {
        // Rキーで ON/OFF 切り替え（バッテリーがあるときのみ）
        if (Input.GetKeyDown(KeyCode.R) && currentBattery > 0f)
        {
            isOn = !isOn;
        }

        // 点灯状態の処理
        if (isOn && currentBattery > 0f)
        {
            currentBattery -= drainRate * Time.deltaTime;
            currentBattery = Mathf.Clamp(currentBattery, 0f, maxBattery);

            flashlight.enabled = true;
            
            if (currentBattery <= 10f)
            {
                float ratio = currentBattery / 10f; // 0～1
                flashlight.intensity = Mathf.Lerp(0.1f, 1.5f, ratio); // 弱～強
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

        //スマホのスライダーUI処理
        if (batterySlider != null)
        {
            batterySlider.value = currentBattery;
        }
        //スマホのテキストUI処理
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

        // バッテリーが回復し、isOnがtrueならライト再点灯
        if (currentBattery > 0f && isOn)
        {
            flashlight.enabled = true;
        }
    }
}
