using UnityEngine;

public class IdleEnemy : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
       animator = GetComponent<Animator>();

        //Idle状態なら明示的にセット
        if(animator != null)
        {
            animator.Play("Idle");
        }
    }

    private void Update()
    {
        if (animator != null && !animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            animator.Play("Idle");
        }
    }
}
