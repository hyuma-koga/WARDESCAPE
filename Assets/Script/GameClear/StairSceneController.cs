using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StairSceneController : MonoBehaviour
{
    public Animator playerAnimator;
    public Transform playerTransform;
    public Image fadeImage;
    public GameObject clearUI; // ゲームクリアUI
    public GameObject scratchImage;

    public float climbDuration = 3.0f;     // アニメ時間
    public float fadeDuration = 1.0f;      // フェード時間（Climb中に開始）
    public float climbSpeed = 0.4f;

    public float scratchDelay = 2.0f;

    public AudioSource audioSource;
    public AudioClip climbSound;
    public AudioSource slashaudioSource;
    public AudioClip scratchSE;
    public AudioSource endaudioSource;
    public AudioClip endSE;

    private void Start()
    {
        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger("Climb");
        }

        StartCoroutine(ClimbAndFadeSequence());
    }

    IEnumerator ClimbAndFadeSequence()
    {
        float elapsed = 0f;
        float fadeStartTime = climbDuration - fadeDuration;

        // 今の位置から、前＋上方向へ進む
        Vector3 climbDir = (playerTransform.forward + Vector3.up * 0.5f).normalized;
        Vector3 startPos = playerTransform.position;
        Vector3 endPos = startPos + climbDir * climbSpeed * climbDuration;

        if(audioSource != null && climbSound != null)
        {
            audioSource.clip = climbSound;
            audioSource.Play();
        }

        // フェード初期化
        fadeImage.enabled = true;
        fadeImage.color = new Color(0, 0, 0, 0);

        while (elapsed < climbDuration)
        {
            float t = elapsed / climbDuration;
            playerTransform.position = Vector3.Lerp(startPos, endPos, t);

            // フェード処理（歩行中にじわじわ）
            if (elapsed >= fadeStartTime)
            {
                float fadeElapsed = elapsed - fadeStartTime;
                float fadeRatio = Mathf.Clamp01(fadeElapsed / fadeDuration);

                Color color = fadeImage.color;
                color.a = fadeRatio;
                fadeImage.color = color;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        if(audioSource != null)
        {
            audioSource.Stop();
        }

        playerTransform.position = endPos;
        fadeImage.color = new Color(0, 0, 0, 1);

        if (clearUI != null && endaudioSource != null && endSE != null) 
        {
            endaudioSource.Play();
            clearUI.SetActive(true);

            //カーソルを表示して、ロックを解除する
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        if(scratchImage != null && slashaudioSource != null && scratchSE != null)
        {
            yield return new WaitForSeconds(scratchDelay);

            slashaudioSource.PlayOneShot(scratchSE);
            scratchImage.SetActive(true);
        }
    }
}
