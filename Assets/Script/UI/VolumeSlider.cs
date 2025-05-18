using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public Slider volumeSlider;

    void Start()
    {
        if (volumeSlider != null)
        {
            volumeSlider.value = AudioListener.volume * 100f;
        }
    }

    public void OnVolumeChanged(float value)
    {
        AudioListener.volume = value / 100f;
    }
}
