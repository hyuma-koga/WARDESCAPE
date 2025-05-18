using UnityEngine;

public class CameraWallClipFix : MonoBehaviour
{
    public Transform cameraTarget;           // プレイヤーの目の位置（Cameraの理想位置）
    public float defaultDistance = 0.5f;     // 通常の距離
    public float minDistance = 0.1f;         // 壁に当たったときの最短距離
    public float smoothSpeed = 10f;          // カメラ補間速度
    public LayerMask collisionMask;          // 壁のレイヤー（Player含めない）

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

        // プレイヤー自身を含めずに Raycast
        if (Physics.Raycast(origin, direction, out hit, defaultDistance + 0.1f, collisionMask, QueryTriggerInteraction.Ignore))
        {
            // 壁にヒットしたら距離を調整
            targetDistance = Mathf.Clamp(hit.distance, minDistance, defaultDistance);
        }

        // なめらかに距離を補間
        currentDistance = Mathf.Lerp(currentDistance, targetDistance, Time.deltaTime * smoothSpeed);

        // カメラ位置を更新
        transform.position = origin + direction * currentDistance;
        //transform.rotation = cameraTarget.rotation;
    }
}
