using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    public Light flickerLight;         //��������ʂ����o���郉�C�g
    public float minIntensity = 0.4f;
    public float maxIntensity = 0.9f;
    public float flickerSpeed = 0.1f;  //������̍X�V�Ԋu

    void Start()
    {
        if (flickerLight == null)
            flickerLight = GetComponent<Light>();

        // ���Ԋu�� Flicker() ���J��Ԃ����s����
        InvokeRepeating(nameof(Flicker), 0f, flickerSpeed);
    }

    // ���邳�������_���ɕύX���ăt���b�J�[���ʂ����o
    void Flicker()
    {
        float newIntensity = Random.Range(minIntensity, maxIntensity);
        flickerLight.intensity = newIntensity;
    }
}
