using UnityEngine;

public class GoalDoorController : MonoBehaviour
{
    public Transform doorHinge;
    public float openAngle = 90f;
    public float openSpeed = 2f;
    public int requiredShards = 6;

    private bool isOpen = false;
    private bool isPlayerNear = false;

    public AudioSource GdooropenSource;
    public AudioSource GdoorcloseSource;
    public AudioClip GdooropenAudio;
    public AudioClip GdoorcloseAudio;

    public GameObject openEKeyHintUI;
    public GameObject closeEKeyHintUI;

    // 🔽 外部から読み取れるように
    public bool IsDoorOpen => isOpen;

    private void Start()
    {
        if (openEKeyHintUI != null) openEKeyHintUI.SetActive(false);
        if (closeEKeyHintUI != null) closeEKeyHintUI.SetActive(false);
    }

    private void Update()
    {
        if(openEKeyHintUI != null && closeEKeyHintUI != null)
        {
            if(isPlayerNear && GameManager.Instance.HasAllShards())
            {
                if (isOpen)
                {
                    openEKeyHintUI.SetActive(false);
                    closeEKeyHintUI.SetActive(true);
                }
                else
                {
                    openEKeyHintUI.SetActive(true);
                    closeEKeyHintUI.SetActive(false);
                }
            }
            else
            {
                //条件未達成 or 離れていたら非表示
                openEKeyHintUI.SetActive(false);
                closeEKeyHintUI.SetActive(false);
            }
        }

        //ドアの開閉処理
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E) && GameManager.Instance.HasAllShards())
        {
            isOpen = !isOpen;

            if (isOpen)
            {
                if (GdooropenSource && GdooropenAudio)
                    GdooropenSource.PlayOneShot(GdooropenAudio);
            }
            else
            {
                if (GdoorcloseSource && GdoorcloseAudio)
                    GdoorcloseSource.PlayOneShot(GdoorcloseAudio);
            }
        }

        Quaternion targetRotation = isOpen ? Quaternion.Euler(0f, openAngle, 0f) : Quaternion.identity;
        doorHinge.localRotation = Quaternion.Slerp(doorHinge.localRotation, targetRotation, Time.deltaTime * openSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) isPlayerNear = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) isPlayerNear = false;

        //離れたらUIを消す
        if(openEKeyHintUI != null) openEKeyHintUI.SetActive(false);
        if (closeEKeyHintUI != null)closeEKeyHintUI.SetActive(false);
    }
}
