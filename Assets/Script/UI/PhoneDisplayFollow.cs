using UnityEngine;

public class PhoneDisplayFollow : MonoBehaviour
{
    public Transform cameraTransform;

    private void LateUpdate()
    {
        //画面左下にオフセット表示(調整OK)
        Vector3 offset = cameraTransform.TransformDirection(new Vector3(-0.7f, -0.1f, 1f));
        transform.position = cameraTransform.position + offset;
        transform.rotation = cameraTransform.rotation;
    }
}
