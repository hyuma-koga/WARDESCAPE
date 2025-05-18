using UnityEngine;

public class SmartphoneToggle : MonoBehaviour
{
    public GameObject smartphoneObject; //�X�}�z���f���̐e�I�u�W�F�N�g

    private bool isVisible = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isVisible = !isVisible;
            smartphoneObject.SetActive(isVisible);
        }
    }
}
