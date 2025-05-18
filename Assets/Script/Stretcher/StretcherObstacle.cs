using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class StretcherObstacle : MonoBehaviour
{
    public Transform stretcher;           // ベッド本体（回転・移動対象）
    public Transform fallenRotation;      // 倒れたときの回転
    public Transform fallenPosition;      // 倒れたときの位置
    public float rotationSpeed = 2f;
    public float moveSpeed = 2f;

    public AudioSource audioSource;
    public AudioClip fallSound;

    public GameObject eKeyHintUI;
    public GameObject spaceKeyHintUI;

    private bool isFallen = false;
    private bool playerInRange = false;

    private NavMeshObstacle navObstacle;

    private void Start()
    {
        //NavMashObstacleのキャッシュ（事前にOFFにしておくこと）
        navObstacle = stretcher.GetComponent<NavMeshObstacle>();
        if(navObstacle != null)
        {
            navObstacle.enabled = false;
        }

        //初期状態でヒントUIを非表示
        if(eKeyHintUI != null)
        {
            eKeyHintUI.SetActive(false);
        }

        if(spaceKeyHintUI != null)
        {
            spaceKeyHintUI.SetActive(false);
        }
    }

    private void Update()
    {
        //UIの制御：まだ倒れてなくて近づいたら表示
        if(eKeyHintUI != null)
        {
            if (isFallen)
            {
                eKeyHintUI.SetActive(false);
            }
            else if (playerInRange)
            {
                eKeyHintUI.SetActive(true);
            }
            else
            {
                eKeyHintUI.SetActive(false);
            }
        }

        if (playerInRange && !isFallen && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(FallStretcher());
        }
    }

    IEnumerator FallStretcher()
    {
        //プレイヤーの移動スクリプトを停止
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerController movement = player.GetComponent<PlayerController>();
        if(movement != null)
        {
            movement.canMove = false;
        }

        //プレイヤーの効果音
        PlayerInteraction pi = player.GetComponent<PlayerInteraction>();
        if(pi != null)
        {
            pi.PlayPushSound();
        }
        isFallen = true;

        //効果音再生
        if(audioSource != null && fallSound != null)
        {
            audioSource.PlayOneShot(fallSound);
        }


        Quaternion targetRotation = fallenRotation.rotation;
        Vector3 targetPosition = fallenPosition.position;

        while (Quaternion.Angle(stretcher.rotation, targetRotation) > 0.1f ||
               Vector3.Distance(stretcher.position, targetPosition) > 0.01f)
        {
            // 回転と移動を同時に補間する
            stretcher.rotation = Quaternion.Slerp(stretcher.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            stretcher.position = Vector3.Lerp(stretcher.position, targetPosition, Time.deltaTime * moveSpeed);
            yield return null;
        }

        // 最終的な誤差を修正
        stretcher.rotation = targetRotation;
        stretcher.position = targetPosition;

        // NavMeshObstacleの有効化などの処理があればここで
        if(navObstacle != null)
        {
            navObstacle.enabled = true;
        }

        //(任意)物理演算の更新を強制
        Physics.SyncTransforms();

        //プレイヤーの移動スクリプトを再び有効化
        if(movement != null)
        {
            movement.canMove = true;
        }

        //最後にUIを非表示に
        if(eKeyHintUI != null)
        {
            eKeyHintUI.SetActive(false);
        }

        if (playerInRange && spaceKeyHintUI != null)
        {
            spaceKeyHintUI.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (isFallen && spaceKeyHintUI != null)
            {
                spaceKeyHintUI.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (spaceKeyHintUI != null)
            {
                spaceKeyHintUI.SetActive(false);
            }
        }
    }
}
