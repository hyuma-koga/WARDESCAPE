using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;

public class ButtonHoverAndClick : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image bloodImage;
    public AudioSource clickAudioSource;
    public string sceneToLoad; // �C�ӂ̃V�[����


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (bloodImage != null) bloodImage.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (bloodImage != null) bloodImage.enabled = false;
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
            yield return new WaitForSeconds(clickAudioSource.clip.length);
        }

        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            // �܂�Set����Ă��Ȃ��������t���O���Z�b�g
            if (PlayerPrefs.GetInt("CameFromTitleScene", 0) == 0)
            {
                PlayerPrefs.SetInt("CameFromTitleScene", 1);
            }

            SceneManager.LoadScene(sceneToLoad);
        }
    }

}
