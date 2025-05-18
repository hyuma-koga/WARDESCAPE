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
    public float moveSpeed = 5f; //移動速度
    public float mouseSensitivity = 200f; //マウス感度
    public Transform cameraTransform;     //カメラのTransformを設定
    public Transform enemy;           //敵にTransformをインスペクターで設定
    public float alertDistance = 8f;  //近づいたと判定する距離
    public AudioClip alertClip;       //再生するサウンド(鼓動音)など
    private bool isAlertPlaying = false;
    private bool attackTriggered = false;
    public int maxLives = 3; //ライフ
    private int currentLives;  //現在のライフ
    public GameObject[] lifeIcons; //3つのImage

    public Rigidbody rb;
    private Animator animator;
    private AudioSource audiosource;

    //捕獲演出用のUIや音
    public GameObject caughtEffect;
    public AudioSource caughtAudio;
    public GameObject caughtexitButton;
    public GameObject restartButton;

    private float cameraPitch = 0f;  //カメラ上下回転用
    public bool canMove = true;  //外部から移動可否を制御
    private float yRotation = 0f;

    public GameObject[] enemyObjects;
    public Transform[] enemySpawnPoints;
    private List<Transform> usedSpawnPoints = new List<Transform>();


    private PlayerInteraction interaction;

    private void Start()
    {
        currentLives = maxLives;

        //感度の読み込み
        if (PlayerPrefs.HasKey("MouseSensitivity"))
        {
            mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity") * 100f;
        }


        // 最初にUI要素を個別に非表示にする（Canvas全体は触らない）
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
        //Cursor.lockState = CursorLockMode.Locked; //マウスカーソルをロック
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
            //ジャンプ物理
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetTrigger("IsJumping");
            animator.SetBool("IsGrounded", false); //空中にいる

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
            animator.SetBool("IsGrounded", true); //着地

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

        // Rigidbodyで移動
        Vector3 newPosition = rb.position + moveDirection.normalized * moveSpeed * Time.deltaTime;
        rb.MovePosition(newPosition);

        // Animatorに"IsMoving"パラメータを送る
        animator.SetBool("IsMoving", isMoving);
    }

    private void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //プレイヤーの左右回転：変数で管理して明示的に回す
        yRotation += mouseX;
        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);

        //カメラの上下回転
        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -30f, 30f);
        cameraTransform.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
    }
    public void OnFootstep()
    {
        // 今は何もしない
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
        if (attackTriggered) return; //二度と呼ばれないように
        attackTriggered = true;

        enemy = enemyTransform;

        //ライフを減らす
        currentLives--;
        UpdateLivesUI();

        //操作無効化
        this.enabled = false;
        //カーソル表示
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        //NavMeshAgentを使って少し話して移動
        NavMeshAgent agent = enemyTransform.GetComponent<NavMeshAgent>();
        Vector3 backDirection = (enemyTransform.position - transform.position).normalized;
        Vector3 targetPos = transform.position + backDirection * 1.5f;

        if (agent != null)
            agent.Warp(targetPos);
        else
            enemyTransform.position = targetPos;

        //プレイヤーを向かせる
        enemyTransform.LookAt(transform.position);

        //敵アニメーション再生
        Animator enemyAnimator = enemyTransform.GetComponent<Animator>();
        if (enemyAnimator != null)
        {
            enemyAnimator.SetTrigger("Attack");
        }

        //敵の顔方向にカメラを向ける
        Transform faceTarget = enemyTransform.Find("FaceTarget");
        if (faceTarget != null)
        {
            Camera.main.transform.LookAt(faceTarget.position);
        }
        else
        {
            Camera.main.transform.LookAt(enemyTransform.position + Vector3.up * 1.6f);
        }

        //捕獲エフェクトやUI
        Time.timeScale = 0.1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        //ライフが０ならゲームオーバー演出、残ってたらReStartを表示
        if (currentLives <= 0)
            StartCoroutine(PlayCaughtSequence());
        else
            StartCoroutine(ShowRestartButtonsOnly());

    }

    IEnumerator PlayCaughtSequence()
    {
        //フラッシュ演出など
        if (caughtEffect != null)
            caughtEffect.SetActive(true);  //画面赤くするUIなど

        //効果音再生
        if (caughtAudio != null && !caughtAudio.isPlaying)
        {
            caughtAudio.loop = false;
            caughtAudio.Play();
        }
        //敵のAnimator取得
        Animator enemyAnimator = enemy.GetComponent<Animator>();
        if (enemyAnimator != null)
        {
            while (!enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Shadowlop_ATTACK"))
                yield return null;
        }
        // Exitボタン表示
        if (caughtexitButton != null)
            caughtexitButton.SetActive(true);

        //ReStartボタンを表示
        if (restartButton != null && currentLives > 0)
            restartButton.SetActive(true);
    }

    public void OnExitButtonPressed()
    {
        //時間を元に戻す
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        //セーブデータ破棄
        TemporarySaveManager.Instance?.ClearSave();

        //新規プレイ用フラグを立てる（GameSceneで初期化が走るように）
        PlayerPrefs.SetInt("CameFromTitleScene", 1);

        //GameManagerを明示的に破棄（Start() を再実行させるため）
        if (GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject);
        }

        //シーン切り替え
        SceneManager.LoadScene("TitleScene");
    }


    public void OnRestartButtonPressed()
    {
        //スローを解除
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        //UIを非表示に
        if (caughtEffect != null)
            caughtEffect.SetActive(false);
        if (restartButton != null)
            restartButton.SetActive(false);
        if (caughtexitButton != null)
            caughtexitButton.SetActive(false);

        //カーソルを非表示に
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (currentLives <= 0)
        {
            //カーソル表示
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("TitleScene"); //ゲームオーバーとして戻す

            return;
        }

        //プレイヤー復帰
        transform.position = GameManager.Instance.GetPlayerStartPosition();
        this.enabled = true;
        attackTriggered = false;

        //敵の位置をランダムにリセット
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
            // スポーン地点未設定時のフォールバック（必要なら地面高さ調整も）
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

        //攻撃が終わったらボタン表示
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
