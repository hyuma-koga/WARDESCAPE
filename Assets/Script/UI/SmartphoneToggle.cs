using UnityEngine;

public class SmartphoneToggle : MonoBehaviour
{
    public GameObject smartphoneObject; //スマホモデルの親オブジェクト

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
