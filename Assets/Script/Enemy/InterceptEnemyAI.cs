using UnityEngine;
using UnityEngine.AI;

public class InterceptEnemyAI : MonoBehaviour
{
    public Transform player;
    public float chaseStartDistance = 20f;     // プレイヤーを追いかけ始める距離（これ以下で追跡開始）
    public float chaseStopDistance = 23f;      // プレイヤーから離れたら追跡をやめる距離（これ以上で停止）
    public float wanderRadius = 100f;          // 徘徊時の移動範囲の半径（中心からどれだけ離れてよいか）
    public float wanderInterval = 20f;         // 徘徊ルートを更新するまでの時間（秒）

    private NavMeshAgent agent;
    private Animator animator;
    private float wanderTimer;
    private bool isChasing = false;
    public AudioSource voiceaudioSource;
    public AudioClip voiceSound;

    public AudioSource runaudioSource;
    public AudioClip runSound;

    private bool isBlocked = false; //障害物でブロックされたかどうか

    private bool hasCaughtPlayer = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        wanderTimer = wanderInterval;
    }

    private void Update()
    {
        if (isBlocked)
        {
            agent.isStopped = true;
            return;
        }
        else
        {
            agent.isStopped = false;
        }

        float distance = Vector3.Distance(transform.position, player.position);
        animator.SetFloat("PlayerDistance", distance);

        if (isChasing)
        {
            if (distance > chaseStopDistance)
            {
                isChasing = false;
                agent.ResetPath();

                //足音を止める
                if (runaudioSource.isPlaying)
                    runaudioSource.Stop();
                if (voiceaudioSource.isPlaying)
                    voiceaudioSource.Stop();
            }
            else
            {
                //足音を再生
                Rigidbody playerRb = player.GetComponent<Rigidbody>();
                Vector3 direction = playerRb.linearVelocity.normalized;

                if (direction.magnitude > 0.1f)
                {
                    // 予測位置：プレイヤーの進行方向に3m先を設定
                    Vector3 predictedPos = player.position + direction * 3f;

                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(predictedPos, out hit, 2f, NavMesh.AllAreas))
                    {
                        agent.speed = 4.5f;
                        agent.SetDestination(hit.position);
                    }
                    else
                    {
                        // NavMeshに載っていない場合は現在のプレイヤー位置に移動
                        agent.SetDestination(player.position);
                    }
                }
                else
                {
                    // プレイヤーがほぼ停止している場合は現在位置へ移動
                    agent.SetDestination(player.position);
                }

                // 足音・ボイスの再生
                if (!runaudioSource.isPlaying && runSound != null)
                {
                    runaudioSource.clip = runSound;
                    runaudioSource.loop = true;
                    runaudioSource.Play();
                }

                if (!voiceaudioSource.isPlaying && voiceSound != null)
                {
                    voiceaudioSource.clip = voiceSound;
                    voiceaudioSource.loop = true;
                    voiceaudioSource.Play();
                }
            }

        }
        else
        {
            if (distance < chaseStartDistance)
            {
                isChasing = true;

                // 足音・ボイスを再生開始
                if (!runaudioSource.isPlaying && runSound != null)
                {
                    runaudioSource.clip = runSound;
                    runaudioSource.loop = true;
                    runaudioSource.Play();
                }

                if (!voiceaudioSource.isPlaying && voiceSound != null)
                {
                    voiceaudioSource.clip = voiceSound;
                    voiceaudioSource.loop = true;
                    voiceaudioSource.Play();
                }
            }
            else
            {
                agent.speed = 3f;
                wanderTimer += Time.deltaTime;

                // 足音・ボイスを停止
                if (runaudioSource.isPlaying)
                    runaudioSource.Stop();

                if (voiceaudioSource.isPlaying)
                    voiceaudioSource.Stop();


                if (!agent.hasPath || agent.remainingDistance < 0.5f || wanderTimer >= wanderInterval)
                {
                    Vector3 newPos = RandomPointOnNavMesh(transform.position, wanderRadius);
                    agent.SetDestination(newPos);
                    wanderTimer = 0f;
                }
            }
        }
    }

    Vector3 RandomPointOnNavMesh(Vector3 center, float radius)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPos = center + Random.insideUnitSphere * radius;
            if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }
        return center;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasCaughtPlayer) return;

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player caught!");
            // ゲームオーバー演出などをここに記述
            other.GetComponent<PlayerController>().OnCaughtByEnemy(transform);
        }

        if (other.CompareTag("Obstacle")) if (other.CompareTag("Obstacle")) // 例えば "Stretcher" などに "Obstacle" タグを付ける
        {
                isBlocked = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            isBlocked = false;
        }
    }
}
