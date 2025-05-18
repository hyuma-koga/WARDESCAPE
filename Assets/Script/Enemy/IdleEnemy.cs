using UnityEngine;

public class IdleEnemy : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
       animator = GetComponent<Animator>();

        //Idle��ԂȂ疾���I�ɃZ�b�g
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
