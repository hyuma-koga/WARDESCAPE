using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class GameIntroManager : MonoBehaviour
{
    public Animator playerAnimator;
    public GameObject ruleUIPanel;  //ルール説明用UI
    public PlayerController playerController;  //プレイヤースクリプト
    public GameObject startButton;
    public HintUIController hintUIController; //インスペクターでアサイン



    public bool isIntroPlaying { get; private set; } = false;  // Intro中かどうか

    private void Start()
    {
        Debug.Log("GameIntroManager Start 呼び出された");

        //カーソルを表示・解放（UI操作のため）
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

    }
   
    IEnumerator PlayOpeningSequence()
    {
 
        //アニメーション再生
        playerAnimator.SetTrigger("Intro");

        //アニメーション待機(必要ならAnimatorの状態確認でもOK)
        yield return new WaitForSeconds(2.5f); //Introアニメの時間に応じて調整

        //UI表示(ルール説明など）
        ruleUIPanel.SetActive(true);

        //スタートボタンを表示して、操作解禁を待つ
        startButton.SetActive(true);
    }

    //この関数をボタンにアサインする
    public void OnStartGameButtonPressed()
    {
        startButton.SetActive(false);
        ruleUIPanel.SetActive(false);

        // カーソルを非表示＆ロック（FPS操作用）
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        playerController.enabled = true;
        isIntroPlaying = false; // ★Introが終わったときに解除

        hintUIController?.ShowHints();
    }

    public void TriggerIntroSequence()
    {
        Debug.Log("紙を読み込んだのでイントロ開始");
        isIntroPlaying = true; // ★イントロ中とする

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        playerController.enabled = false;
        startButton.SetActive(false);

        StartCoroutine(PlayOpeningSequence());
    }
}
