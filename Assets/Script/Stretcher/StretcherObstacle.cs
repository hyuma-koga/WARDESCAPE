using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class StretcherObstacle : MonoBehaviour
{
    public Transform stretcher;           // �x�b�h�{�́i��]�E�ړ��Ώہj
    public Transform fallenRotation;      // �|�ꂽ�Ƃ��̉�]
    public Transform fallenPosition;      // �|�ꂽ�Ƃ��̈ʒu
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
        //NavMashObstacle�̃L���b�V���i���O��OFF�ɂ��Ă������Ɓj
        navObstacle = stretcher.GetComponent<NavMeshObstacle>();
        if(navObstacle != null)
        {
            navObstacle.enabled = false;
        }

        //������ԂŃq���gUI���\��
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
        //UI�̐���F�܂��|��ĂȂ��ċ߂Â�����\��
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
        //�v���C���[�̈ړ��X�N���v�g���~
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerController movement = player.GetComponent<PlayerController>();
        if(movement != null)
        {
            movement.canMove = false;
        }

        //�v���C���[�̌��ʉ�
        PlayerInteraction pi = player.GetComponent<PlayerInteraction>();
        if(pi != null)
        {
            pi.PlayPushSound();
        }
        isFallen = true;

        //���ʉ��Đ�
        if(audioSource != null && fallSound != null)
        {
            audioSource.PlayOneShot(fallSound);
        }


        Quaternion targetRotation = fallenRotation.rotation;
        Vector3 targetPosition = fallenPosition.position;

        while (Quaternion.Angle(stretcher.rotation, targetRotation) > 0.1f ||
               Vector3.Distance(stretcher.position, targetPosition) > 0.01f)
        {
            // ��]�ƈړ��𓯎��ɕ�Ԃ���
            stretcher.rotation = Quaternion.Slerp(stretcher.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            stretcher.position = Vector3.Lerp(stretcher.position, targetPosition, Time.deltaTime * moveSpeed);
            yield return null;
        }

        // �ŏI�I�Ȍ덷���C��
        stretcher.rotation = targetRotation;
        stretcher.position = targetPosition;

        // NavMeshObstacle�̗L�����Ȃǂ̏���������΂�����
        if(navObstacle != null)
        {
            navObstacle.enabled = true;
        }

        //(�C��)�������Z�̍X�V������
        Physics.SyncTransforms();

        //�v���C���[�̈ړ��X�N���v�g���ĂїL����
        if(movement != null)
        {
            movement.canMove = true;
        }

        //�Ō��UI���\����
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
