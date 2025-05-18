using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 6f;
    public float moveSpeed = 5f; //�ړ����x
    public float mouseSensitivity = 200f; //�}�E�X���x
    public Transform cameraTransform;     //�J������Transform��ݒ�
    public Transform enemy;           //�G��Transform���C���X�y�N�^�[�Őݒ�
    public float alertDistance = 8f;  //�߂Â����Ɣ��肷�鋗��
    public AudioClip alertClip;       //�Đ�����T�E���h(�ۓ���)�Ȃ�
    private bool isAlertPlaying = false;
    private bool attackTriggered = false;
    public int maxLives = 3; //���C�t
    private int currentLives;  //���݂̃��C�t
    public GameObject[] lifeIcons; //3��Image

    public Rigidbody rb;
    private Animator animator;
    private AudioSource audiosource;

    //�ߊl���o�p��UI�≹
    public GameObject caughtEffect;
    public AudioSource caughtAudio;
    public GameObject caughtexitButton;
    public GameObject restartButton;

    private float cameraPitch = 0f;  //�J�����㉺��]�p
    public bool canMove = true;  //�O������ړ��ۂ𐧌�
    private float yRotation = 0f;

    public GameObject[] enemyObjects;
    public Transform[] enemySpawnPoints;
    private List<Transform> usedSpawnPoints = new List<Transform>();


    private PlayerInteraction interaction;

    private void Start()
    {
        currentLives = maxLives;

        //���x�̓ǂݍ���
        if (PlayerPrefs.HasKey("MouseSensitivity"))
        {
            mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity") * 100f;
        }


        // �ŏ���UI�v�f���ʂɔ�\���ɂ���iCanvas�S�̂͐G��Ȃ��j
        if (caughtEffect != null)
            caughtEffect.SetActive(false);

        if (caughtexitButton != null)
            caughtexitButton.SetActive(false);

        if (restartButton != null)
            restartButton.SetActive(false);
    }


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        //Cursor.lockState = CursorLockMode.Locked; //�}�E�X�J�[�\�������b�N
        audiosource = GetComponent<AudioSource>();
        interaction = GetComponent<PlayerInteraction>();
    }

    private void Update()
    {
        if (canMove)
        {
            Move();
        }

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            //�W�����v����
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetTrigger("IsJumping");
            animator.SetBool("IsGrounded", false); //�󒆂ɂ���

            if (interaction != null)
            {
                interaction.PlayJumpSound();
            }
        }

        LookAround();
        CheckEnemyDistance();
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, 0.2f);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            animator.SetBool("IsGrounded", true); //���n

            if (interaction != null)
            {
                interaction.PlayLandSound();
            }
        }
    }


    private void Move()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = cameraTransform.right * moveX + cameraTransform.forward * moveZ;
        moveDirection.y = 0f;

        bool isMoving = moveDirection.sqrMagnitude > 0.01f;

        // Rigidbody�ňړ�
        Vector3 newPosition = rb.position + moveDirection.normalized * moveSpeed * Time.deltaTime;
        rb.MovePosition(newPosition);

        // Animator��"IsMoving"�p�����[�^�𑗂�
        animator.SetBool("IsMoving", isMoving);
    }

    private void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //�v���C���[�̍��E��]�F�ϐ��ŊǗ����Ė����I�ɉ�
        yRotation += mouseX;
        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);

        //�J�����̏㉺��]
        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -30f, 30f);
        cameraTransform.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
    }
    public void OnFootstep()
    {
        // ���͉������Ȃ�
    }

    public void CheckEnemyDistance()
    {
        if (enemy == null || alertClip == null) return;

        float distance = Vector3.Distance(transform.position, enemy.position);

        if (distance <= alertDistance)
        {
            if (!isAlertPlaying)
            {
                audiosource.clip = alertClip;
                audiosource.loop = true;
                audiosource.Play();
                isAlertPlaying = true;
            }
        }
        else
        {
            if (isAlertPlaying)
            {
                audiosource.Stop();
                isAlertPlaying = false;
            }
        }
    }

    public void OnCaughtByEnemy(Transform enemyTransform)
    {
        if (attackTriggered) return; //��x�ƌĂ΂�Ȃ��悤��
        attackTriggered = true;

        enemy = enemyTransform;

        //���C�t�����炷
        currentLives--;
        UpdateLivesUI();

        //���얳����
        this.enabled = false;
        //�J�[�\���\��
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        //NavMeshAgent���g���ď����b���Ĉړ�
        NavMeshAgent agent = enemyTransform.GetComponent<NavMeshAgent>();
        Vector3 backDirection = (enemyTransform.position - transform.position).normalized;
        Vector3 targetPos = transform.position + backDirection * 1.5f;

        if (agent != null)
            agent.Warp(targetPos);
        else
            enemyTransform.position = targetPos;

        //�v���C���[����������
        enemyTransform.LookAt(transform.position);

        //�G�A�j���[�V�����Đ�
        Animator enemyAnimator = enemyTransform.GetComponent<Animator>();
        if (enemyAnimator != null)
        {
            enemyAnimator.SetTrigger("Attack");
        }

        //�G�̊�����ɃJ������������
        Transform faceTarget = enemyTransform.Find("FaceTarget");
        if (faceTarget != null)
        {
            Camera.main.transform.LookAt(faceTarget.position);
        }
        else
        {
            Camera.main.transform.LookAt(enemyTransform.position + Vector3.up * 1.6f);
        }

        //�ߊl�G�t�F�N�g��UI
        Time.timeScale = 0.1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        //���C�t���O�Ȃ�Q�[���I�[�o�[���o�A�c���Ă���ReStart��\��
        if (currentLives <= 0)
            StartCoroutine(PlayCaughtSequence());
        else
            StartCoroutine(ShowRestartButtonsOnly());

    }

    IEnumerator PlayCaughtSequence()
    {
        //�t���b�V�����o�Ȃ�
        if (caughtEffect != null)
            caughtEffect.SetActive(true);  //��ʐԂ�����UI�Ȃ�

        //���ʉ��Đ�
        if (caughtAudio != null && !caughtAudio.isPlaying)
        {
            caughtAudio.loop = false;
            caughtAudio.Play();
        }
        //�G��Animator�擾
        Animator enemyAnimator = enemy.GetComponent<Animator>();
        if (enemyAnimator != null)
        {
            while (!enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Shadowlop_ATTACK"))
                yield return null;
        }
        // Exit�{�^���\��
        if (caughtexitButton != null)
            caughtexitButton.SetActive(true);

        //ReStart�{�^����\��
        if (restartButton != null && currentLives > 0)
            restartButton.SetActive(true);
    }

    public void OnExitButtonPressed()
    {
        //���Ԃ����ɖ߂�
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        //�Z�[�u�f�[�^�j��
        TemporarySaveManager.Instance?.ClearSave();

        //�V�K�v���C�p�t���O�𗧂Ă�iGameScene�ŏ�����������悤�Ɂj
        PlayerPrefs.SetInt("CameFromTitleScene", 1);

        //GameManager�𖾎��I�ɔj���iStart() ���Ď��s�����邽�߁j
        if (GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject);
        }

        //�V�[���؂�ւ�
        SceneManager.LoadScene("TitleScene");
    }


    public void OnRestartButtonPressed()
    {
        //�X���[������
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        //UI���\����
        if (caughtEffect != null)
            caughtEffect.SetActive(false);
        if (restartButton != null)
            restartButton.SetActive(false);
        if (caughtexitButton != null)
            caughtexitButton.SetActive(false);

        //�J�[�\�����\����
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (currentLives <= 0)
        {
            //�J�[�\���\��
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("TitleScene"); //�Q�[���I�[�o�[�Ƃ��Ė߂�

            return;
        }

        //�v���C���[���A
        transform.position = GameManager.Instance.GetPlayerStartPosition();
        this.enabled = true;
        attackTriggered = false;

        //�G�̈ʒu�������_���Ƀ��Z�b�g
        if (enemyObjects != null && enemyObjects.Length > 0)
        {
            foreach (GameObject enemy in enemyObjects)
            {
                Vector3 newPos = GetUniqueRandomSpawnPoint();

                NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
                if (agent != null)
                    agent.Warp(newPos);
                else
                    enemy.transform.position = newPos;
            }
        }

    }



    private Vector3 GetRandomEnemySpawnPoint()
    {
        if (enemySpawnPoints != null && enemySpawnPoints.Length > 0)
        {
            int index = Random.Range(0, enemySpawnPoints.Length);
            return enemySpawnPoints[index].position;
        }
        else
        {
            // �X�|�[���n�_���ݒ莞�̃t�H�[���o�b�N�i�K�v�Ȃ�n�ʍ����������j
            return new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f));
        }
    }

    private Vector3 GetUniqueRandomSpawnPoint()
    {
        if (enemySpawnPoints == null || enemySpawnPoints.Length == 0)
            return new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f));

        Transform[] availablePoints = enemySpawnPoints.Except(usedSpawnPoints).ToArray();
        if (availablePoints.Length == 0) usedSpawnPoints.Clear();

        int index = Random.Range(0, availablePoints.Length);
        Transform chosen = availablePoints[index];
        usedSpawnPoints.Add(chosen);
        return chosen.position;
    }



    IEnumerator ShowRestartButtonsOnly()
    {
        if (caughtEffect != null)
            caughtEffect.SetActive(true);

        if (caughtAudio != null && !caughtAudio.isPlaying)
        {
            caughtAudio.loop = false;
            caughtAudio.Play();
        }

        Animator enemyAnimator = enemy.GetComponent<Animator>();
        if (enemyAnimator != null)
        {
            while (!enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Shadowlop_ATTACK"))
                yield return null;
        }

        //�U�����I�������{�^���\��
        if (restartButton != null) restartButton.SetActive(true);
        if (caughtexitButton != null) caughtexitButton.SetActive(true);
    }

    public void UpdateLivesUI()
    {
        for (int i = 0; i < lifeIcons.Length; i++)
        {
            lifeIcons[i].SetActive(i < currentLives);
        }
    }
}
