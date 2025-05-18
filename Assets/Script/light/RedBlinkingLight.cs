using UnityEngine;
using System.Collections;

public class RedBlinkingLight : MonoBehaviour
{
    public Light redLight;
    public float baseInterval = 0.2f;  //��{���o

    private readonly float[] blinkPattern =
    {
        1.5f, // �����_���i�^�[�[�[���j
        0.2f, // �����i�^�j
        0.2f, // �_���i�^�[�j
        1.5f  // �����_���i�[�[�[���j
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

            index = (index + 1) % blinkPattern.Length;  //���[�v
        }
    }
}
