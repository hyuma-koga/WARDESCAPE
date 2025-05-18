using UnityEngine;

public class CameraWallClipFix : MonoBehaviour
{
    public Transform cameraTarget;           // �v���C���[�̖ڂ̈ʒu�iCamera�̗��z�ʒu�j
    public float defaultDistance = 0.5f;     // �ʏ�̋���
    public float minDistance = 0.1f;         // �ǂɓ��������Ƃ��̍ŒZ����
    public float smoothSpeed = 10f;          // �J������ԑ��x
    public LayerMask collisionMask;          // �ǂ̃��C���[�iPlayer�܂߂Ȃ��j

    private float currentDistance;

    private void Start()
    {
        currentDistance = defaultDistance;
    }

    private void LateUpdate()
    {
        Vector3 origin = cameraTarget.position;
        Vector3 direction = -cameraTarget.forward;

        RaycastHit hit;

        float targetDistance = defaultDistance;

        // �v���C���[���g���܂߂��� Raycast
        if (Physics.Raycast(origin, direction, out hit, defaultDistance + 0.1f, collisionMask, QueryTriggerInteraction.Ignore))
        {
            // �ǂɃq�b�g�����狗���𒲐�
            targetDistance = Mathf.Clamp(hit.distance, minDistance, defaultDistance);
        }

        // �Ȃ߂炩�ɋ�������
        currentDistance = Mathf.Lerp(currentDistance, targetDistance, Time.deltaTime * smoothSpeed);

        // �J�����ʒu���X�V
        transform.position = origin + direction * currentDistance;
        //transform.rotation = cameraTarget.rotation;
    }
}
