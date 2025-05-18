using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovemwnt : MonoBehaviour
{
    public float moveSpeed = 3.0f;
    public Animator animator;

    private CharacterController controller;
    private Vector3 moveDirection;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // ���͕����x�N�g��
        Vector3 input = new Vector3(h, 0, v);

        // ���[���h��Ԃɕϊ�
        if (input.magnitude > 0.1f)
        {
            // �ړ��������J�����ɍ��킹�ĉ�]
            moveDirection = Camera.main.transform.TransformDirection(input);
            moveDirection.y = 0f;
            moveDirection.Normalize();
            controller.Move(moveDirection * moveSpeed * Time.deltaTime);
        }

        // �A�j���[�^�[�ɕ����Ă邩�`����
        bool isWalking = input.magnitude > 0.1f;
        animator.SetBool("isWalking", isWalking);
    }
}
