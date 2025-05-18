using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalTriggerZone : MonoBehaviour
{
    public string nextSceneName = "StairScene";
    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;
            SceneManager.LoadScene(nextSceneName);
        }
    }
}