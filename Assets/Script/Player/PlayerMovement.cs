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

        // 入力方向ベクトル
        Vector3 input = new Vector3(h, 0, v);

        // ワールド空間に変換
        if (input.magnitude > 0.1f)
        {
            // 移動方向をカメラに合わせて回転
            moveDirection = Camera.main.transform.TransformDirection(input);
            moveDirection.y = 0f;
            moveDirection.Normalize();
            controller.Move(moveDirection * moveSpeed * Time.deltaTime);
        }

        // アニメーターに歩いてるか伝える
        bool isWalking = input.magnitude > 0.1f;
        animator.SetBool("isWalking", isWalking);
    }
}
