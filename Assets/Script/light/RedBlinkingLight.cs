using UnityEngine;
using System.Collections;

public class RedBlinkingLight : MonoBehaviour
{
    public Light redLight;
    public float baseInterval = 0.2f;  //基本感覚

    private readonly float[] blinkPattern =
    {
        1.5f, // 長く点灯（ターーーン）
        0.2f, // 消灯（タ）
        0.2f, // 点灯（ター）
        1.5f  // 長く点灯（ーーーン）
    };

    void Start()
    {
        if (redLight == null)
            redLight = GetComponent<Light>();

        StartCoroutine(BlinkFlickerRoutine());
    }

    IEnumerator BlinkFlickerRoutine()
    {
        int index = 0;

        while (true)
        {
            redLight.enabled = !redLight.enabled;
            yield return new WaitForSeconds(blinkPattern[index]);

            index = (index + 1) % blinkPattern.Length;  //ループ
        }
    }
}
