using UnityEngine;

public class MinimapFollow : MonoBehaviour
{
    public Transform player;

    private void LateUpdate()
    {
        Vector3 pos = player.position;
        pos.y = transform.position.y; //�����Œ�
        transform.position = pos;
    }
}
