using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class GameIntroManager : MonoBehaviour
{
    public Animator playerAnimator;
    public GameObject ruleUIPanel;  //���[�������pUI
    public PlayerController playerController;  //�v���C���[�X�N���v�g
    public GameObject startButton;
    public HintUIController hintUIController; //�C���X�y�N�^�[�ŃA�T�C��



    public bool isIntroPlaying { get; private set; } = false;  // Intro�����ǂ���

    private void Start()
    {
        Debug.Log("GameIntroManager Start �Ăяo���ꂽ");

        //�J�[�\����\���E����iUI����̂��߁j
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

    }
   
    IEnumerator PlayOpeningSequence()
    {
 
        //�A�j���[�V�����Đ�
        playerAnimator.SetTrigger("Intro");

        //�A�j���[�V�����ҋ@(�K�v�Ȃ�Animator�̏�Ԋm�F�ł�OK)
        yield return new WaitForSeconds(2.5f); //Intro�A�j���̎��Ԃɉ����Ē���

        //UI�\��(���[�������Ȃǁj
        ruleUIPanel.SetActive(true);

        //�X�^�[�g�{�^����\�����āA������ւ�҂�
        startButton.SetActive(true);
    }

    //���̊֐����{�^���ɃA�T�C������
    public void OnStartGameButtonPressed()
    {
        startButton.SetActive(false);
        ruleUIPanel.SetActive(false);

        // �J�[�\�����\�������b�N�iFPS����p�j
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        playerController.enabled = true;
        isIntroPlaying = false; // ��Intro���I������Ƃ��ɉ���

        hintUIController?.ShowHints();
    }

    public void TriggerIntroSequence()
    {
        Debug.Log("����ǂݍ��񂾂̂ŃC���g���J�n");
        isIntroPlaying = true; // ���C���g�����Ƃ���

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        playerController.enabled = false;
        startButton.SetActive(false);

        StartCoroutine(PlayOpeningSequence());
    }
}
