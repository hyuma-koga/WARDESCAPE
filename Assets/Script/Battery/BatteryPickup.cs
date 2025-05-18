using UnityEngine;

public class BatteryPickup : MonoBehaviour
{
    public float chargeAmount = 30f; // Å© Ç±ÇÍÇ™ïKóvÅI
    public AudioSource audioSource;
    public AudioClip batterySound;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Battery Trigger: " + other.name);

        Transform root = other.transform.root;
        if (root.CompareTag("Player"))
        {
            FlashlightBattery battery = root.GetComponentInChildren<FlashlightBattery>();
            if (battery != null)
            {
                battery.RechargeBattery(chargeAmount);
                Debug.Log("Battery recharged!");
                
            }
            if (audioSource != null && batterySound)
            {
                audioSource.PlayOneShot(batterySound);
            }

            Destroy(gameObject, batterySound != null ? batterySound.length : 0f);
        }
    }
}
