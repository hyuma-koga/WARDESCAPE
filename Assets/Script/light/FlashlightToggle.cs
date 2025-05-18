using UnityEngine;

public class FlashlightToggle : MonoBehaviour
{
    public Light flashlight;

    private void Start()
    {
        flashlight.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            flashlight.enabled = !flashlight.enabled;
        }
    }
}
