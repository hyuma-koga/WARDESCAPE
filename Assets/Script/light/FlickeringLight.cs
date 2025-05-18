using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    public Light flickerLight;         //ちらつき効果を演出するライト
    public float minIntensity = 0.4f;
    public float maxIntensity = 0.9f;
    public float flickerSpeed = 0.1f;  //ちらつきの更新間隔

    void Start()
    {
        if (flickerLight == null)
            flickerLight = GetComponent<Light>();

        // 一定間隔で Flicker() を繰り返し実行する
        InvokeRepeating(nameof(Flicker), 0f, flickerSpeed);
    }

    // 明るさをランダムに変更してフリッカー効果を演出
    void Flicker()
    {
        float newIntensity = Random.Range(minIntensity, maxIntensity);
        flickerLight.intensity = newIntensity;
    }
}
