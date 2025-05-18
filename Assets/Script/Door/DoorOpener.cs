using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    public Transform doorHinge;    //������]�����镔��    
    public float openAngle = 90f;  //�ǂꂭ�炢�J����
    public float openSpeed = 3f;   //���l���傫�����f��������

    private bool isPlayerNear = false;  //�v���C���[��Trigger���ɓ����Ă��邩�̃t���O
    private bool isOpen = false;        //�������݊J���Ă��邩�ǂ����̃t���O

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
        //�q���gUI�؂�ւ�
        UpdateHintUI();

        // �v���C���[���߂��ɂ��āuE�L�[�v���������Ƃ�
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            isOpen = !isOpen;  // true �� false ��؂�ւ�

            // ��Ԃ��؂�ւ�����^�C�~���O�ŉ���炷
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

        // ���̉�]����
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
            //���ꂽ�痼����\��
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
        //�v���C���[��Trigger�ɓ������Ƃ��ɌĂ΂��
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //�v���C���[��Trigger����o���Ƃ��ɌĂ΂��
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;

            //�v���C���[�����ꂽ��UI������
            if (openEKeyHintUI != null) openEKeyHintUI.SetActive(false);
            if (closeEKeyHintUI != null) closeEKeyHintUI.SetActive(false);
        }
    }
}