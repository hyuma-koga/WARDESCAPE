using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
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

    private bool isBlocked = false; // ← 障害物でブロックされたかどうか

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
                agent.speed = 4f;
                agent.SetDestination(player.position);

                //足音を再生
                if(!runaudioSource.isPlaying && runSound != null)
                {
                    runaudioSource.clip = runSound;
                    runaudioSource.loop = true;
                    runaudioSource.Play();
                }

                if(!voiceaudioSource.isPlaying && voiceSound != null)
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

                //足音を再生開始
                if (!runaudioSource.isPlaying && runSound != null)
                {
                    runaudioSource.clip = runSound;
                    runaudioSource.loop = true;
                    runaudioSource.Play();
                }

                if(!voiceaudioSource.isPlaying && voiceSound != null)
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

                //足音を止める
                if (runaudioSource.isPlaying)
                    runaudioSource.Stop();

                if(voiceaudioSource.isPlaying)
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
            // ゲームオーバー演出などをここに
            other.GetComponent<PlayerController>().OnCaughtByEnemy(transform);
        }

        if (other.CompareTag("Obstacle")) // ← Stretcher などに "Obstacle" タグを付ける
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
