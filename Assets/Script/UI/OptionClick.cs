using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;

public class OptionClick : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image bloodImage;
    public AudioSource clickAudioSource;

    public string sceneToLoad; // 次に読み込むシーン名（例: "GameScene"）

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (bloodImage != null)
            bloodImage.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (bloodImage != null)
            bloodImage.enabled = false;
    }

    public void PlayClickSoundAndLoadScene()
    {
        StartCoroutine(PlayAndLoad());
    }

    IEnumerator PlayAndLoad()
    {
        if (clickAudioSource != null)
        {
            clickAudioSource.Play();
            yield return new WaitForSeconds(clickAudioSource.clip.length); // 再生終了まで待つ
        }

        SceneManager.LoadScene("OptionScene");
    }
}
