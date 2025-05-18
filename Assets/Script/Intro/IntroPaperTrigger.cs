using UnityEngine;

public class IntroPaperTrigger : MonoBehaviour
{
    public GameIntroManager introManager;
    public GameObject eKeyHintUI;
 
    private bool isPlayerNear = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            eKeyHintUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            eKeyHintUI.SetActive(false);
        }
    }

    private void Update()
    {
        if(eKeyHintUI != null)
        {
            if(introManager != null && introManager.isIntroPlaying)
            {
                eKeyHintUI.SetActive(false);
            }
            else if (isPlayerNear)
            {
                eKeyHintUI.SetActive(true);
            }
        }


        if(isPlayerNear && Input.GetKeyDown(KeyCode.E) && !introManager.isIntroPlaying)
        {
            introManager.TriggerIntroSequence();   //Intro‚ðŠJŽn
        }
    }
}
