using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float chaseStartDistance = 20f;     // �v���C���[��ǂ������n�߂鋗���i����ȉ��ŒǐՊJ�n�j
    public float chaseStopDistance = 23f;      // �v���C���[���痣�ꂽ��ǐՂ���߂鋗���i����ȏ�Œ�~�j
    public float wanderRadius = 100f;          // �p�j���̈ړ��͈͂̔��a�i���S����ǂꂾ������Ă悢���j
    public float wanderInterval = 20f;         // �p�j���[�g���X�V����܂ł̎��ԁi�b�j


    private NavMeshAgent agent;
    private Animator animator;
    private float wanderTimer;
    private bool isChasing = false;
    public AudioSource voiceaudioSource;
    public AudioClip voiceSound;

    public AudioSource runaudioSource;
    public AudioClip runSound;

    private bool isBlocked = false; // �� ��Q���Ńu���b�N���ꂽ���ǂ���

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

                //�������~�߂�
                if (runaudioSource.isPlaying)
                    runaudioSource.Stop();
                if (voiceaudioSource.isPlaying)
                    voiceaudioSource.Stop();
            }
            else
            {
                agent.speed = 4f;
                agent.SetDestination(player.position);

                //�������Đ�
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

                //�������Đ��J�n
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

                //�������~�߂�
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
            // �Q�[���I�[�o�[���o�Ȃǂ�������
            other.GetComponent<PlayerController>().OnCaughtByEnemy(transform);
        }

        if (other.CompareTag("Obstacle")) // �� Stretcher �Ȃǂ� "Obstacle" �^�O��t����
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
