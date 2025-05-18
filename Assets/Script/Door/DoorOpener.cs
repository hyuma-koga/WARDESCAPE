using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    public Transform doorHinge;    //扉を回転させる部分    
    public float openAngle = 90f;  //どれくらい開くか
    public float openSpeed = 3f;   //数値が大きい程素早く動く

    private bool isPlayerNear = false;  //プレイヤーがTrigger内に入っているかのフラグ
    private bool isOpen = false;        //扉が現在開いているかどうかのフラグ

    public AudioSource openaudioSource;
    public AudioSource closeaudioSource;
    public AudioClip dooropenSound;
    public AudioClip doorcloseSound;

    public GameObject openEKeyHintUI;
    public GameObject closeEKeyHintUI;


    private void Start()
    {
        if (openEKeyHintUI != null) openEKeyHintUI.SetActive(false);
        if (closeEKeyHintUI != null) closeEKeyHintUI.SetActive(false);
    }
    private void Update()
    {
        //ヒントUI切り替え
        UpdateHintUI();

        // プレイヤーが近くにいて「Eキー」を押したとき
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            isOpen = !isOpen;  // true ⇔ false を切り替え

            // 状態が切り替わったタイミングで音を鳴らす
            if (isOpen)
            {
                if (openaudioSource != null && dooropenSound != null)
                {
                    openaudioSource.PlayOneShot(dooropenSound);
                }
            }
            else
            {
                if (closeaudioSource != null && doorcloseSound != null)
                {
                    closeaudioSource.PlayOneShot(doorcloseSound);
                }
            }
        }

        // 扉の回転処理
        Quaternion targetRotation = isOpen
            ? Quaternion.Euler(0f, openAngle, 0f)
            : Quaternion.identity;

        doorHinge.localRotation = Quaternion.Slerp(
            doorHinge.localRotation, targetRotation, Time.deltaTime * openSpeed);
    }

    private void UpdateHintUI()
    {
        if (!isPlayerNear)
        {
            //離れたら両方非表示
            if (openEKeyHintUI != null) openEKeyHintUI.SetActive(false);
            if (closeEKeyHintUI != null) closeEKeyHintUI.SetActive(false);
            return;
        }

        if (isOpen)
        {
            if (openEKeyHintUI != null) openEKeyHintUI.SetActive(false);
            if (closeEKeyHintUI != null) closeEKeyHintUI.SetActive(true);
        }
        else
        {
            if (openEKeyHintUI != null) openEKeyHintUI.SetActive(true);
            if (closeEKeyHintUI != null) closeEKeyHintUI.SetActive(false);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        //プレイヤーがTriggerに入ったときに呼ばれる
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //プレイヤーがTriggerから出たときに呼ばれる
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;

            //プレイヤーが離れたらUIを消す
            if (openEKeyHintUI != null) openEKeyHintUI.SetActive(false);
            if (closeEKeyHintUI != null) closeEKeyHintUI.SetActive(false);
        }
    }
}