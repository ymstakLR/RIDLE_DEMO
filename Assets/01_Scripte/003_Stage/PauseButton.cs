using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// pause関連の処理
/// 更新日時:0414
/// </summary>
public class PauseButton : MonoBehaviour {
    private readonly string PAUSE = "Pause";

    private GameObject _pauseUI;
    private GameObject _gameManager;

    private StageStatusManagement _stageClearManagement;

    private float _isContinueTimer;

    void Start() {
        _pauseUI = GameObject.Find("UI/ButtonCanvas");
        _pauseUI.SetActive(false);
        _gameManager = GameObject.Find("GameManager");
        _stageClearManagement = GameObject.Find("Stage").GetComponent<StageStatusManagement>();
    }//Start

    void Update() {
        ContinueWaitTime();
        PauseJudge();
    }//Update

    /// <summary>
    /// 再開する時に一定時間待ち状態にしてから再開する処理
    /// </summary>
    private void ContinueWaitTime() {
        if (_isContinueTimer > 0.25f && _isContinueTimer < 1) {
            Continue();
            _isContinueTimer += 1;
        } else if (_isContinueTimer < 1) {
            _isContinueTimer += 0.01f;
        }//if
    }//ContinueWaitTime

    /// <summary>
    /// pauseを行うための判定取得処理
    /// </summary>
    private void PauseJudge() {
        if (!Input.GetButtonDown(PAUSE) ||
            _stageClearManagement.StageStatus == EnumStageStatus.Clear)
            return;
        PauseInit();
    }//PauseJudge

    /// <summary>
    /// pause時の各種初期化処理
    /// </summary>
    private void PauseInit() {
        if (Time.timeScale == 0) {
            Continue();
        } else {
            Time.timeScale = 0;
            _pauseUI.SetActive(true);
            _gameManager.GetComponent<ButtonInfo>().ButtonInteractable(true);
            _pauseUI.transform.Find("CONTINUEButton").gameObject.GetComponent<Selectable>().Select();
        }//if
    }//PauseInit

    public void Continue() {
        Time.timeScale = 1;
        _pauseUI.transform.Find("EXITButton").gameObject.GetComponent<Selectable>().Select();
        _pauseUI.SetActive(false);
    }//Continue

    public void ContinueButton() {
        if (_isContinueTimer > 0.25) {
            _isContinueTimer = 0;
            _gameManager.GetComponent<AudioManager>().PlaySE("ButtonClick");
            _gameManager.GetComponent<ButtonInfo>().ButtonInteractable(false);
        }//if
    }//ContinueButton

    public void RestartButton() {
        this.GetComponent<Retray>().SceneRetray(3f);
        _gameManager.GetComponent<AudioManager>().PlaySE("ButtonClick");
        _gameManager.GetComponent<ButtonInfo>().ButtonInteractable(false);
    }//RestartButton

    public void ExitButton() {
        _gameManager.GetComponent<SceneChange>().BackSceneChange(isBackSE: false);
    }//ExitButton

}//PauseButton
