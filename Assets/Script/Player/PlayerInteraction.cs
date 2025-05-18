using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public AudioSource pushAudioSource;
    public AudioClip pushSound;

    public AudioSource jumpAudioSource;
    public AudioClip jumpSound;

    public AudioSource landAudioSource;
    public AudioClip landSound;



    public void PlayPushSound()
    {
        if(pushAudioSource != null && pushSound != null)
        {
            pushAudioSource.PlayOneShot(pushSound);
        }
    }

    public void PlayJumpSound()
    {
        if(jumpAudioSource != null && jumpSound != null)
        {
            jumpAudioSource.PlayOneShot(jumpSound);
        }
    }

    public void PlayLandSound()
    {
        if(landAudioSource != null && landSound != null)
        {
            landAudioSource.PlayOneShot(landSound);
        }
    }
}
