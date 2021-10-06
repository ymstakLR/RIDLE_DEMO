using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// キーコンフィグ画面上のボタン処理
/// 更新日時:20211006
/// </summary>
public class KeyConfigButtonMove : MonoBehaviour {
    private GameObject _inputButton;//コンフィグ対象のボタンオブジェクト

    private bool _isControllerConfig;//コントローラ設定の判定
    private bool _isKeyBoardConfig;//キーボード設定の判定

    private bool _isInputKeyUpdatePossible;

    private void Awake() {
        InputManagerDataEdit.InputDataUpdate();
        InputManagerDataEdit.ConfigButtonsTextUpdate(this.gameObject);
    }//Awake

    private void Update() {
        if (_isKeyBoardConfig || _isControllerConfig) {
            if (Input.GetButtonUp("Submit")) {
                _isInputKeyUpdatePossible = true;
            }//if
        }//if
        if (_isInputKeyUpdatePossible && Input.anyKeyDown) {
            ConfigUpdateCheck(GetInputKeyCode());
        }//if
    }//Update

///ボタン入力関連の処理

    /// <summary>
    /// キーボード用コンフィグボタンが押された際の処理
    /// </summary>
    /// <param name="inputButton"></param>
    public void KeyBoardConfigButton(GameObject inputButton) {
        ConfigButton(inputButton, true);
    }//KeyConfigButton

    /// <summary>
    /// コントローラ用コンフィグボタンが押された際の処理
    /// </summary>
    /// <param name="inputButton"></param>
    public void ControllerConfigButton(GameObject inputButton) {
        ConfigButton(inputButton, false);
    }//ControllerCOnfigButton

    /// <summary>
    /// コンフィグボタンが押された際の処理
    /// </summary>
    /// <param name="inputButton">押されたボタンオブジェクト</param>
    /// <param name="isKeyBoard">入力されたコンフィグタイプ(KeyBoardで入力された場合にTrue)</param>
    private void ConfigButton(GameObject inputButton, bool isKeyBoard) {
        ConfigStart(inputButton, isKeyBoard, GetInputKeyCode());
    }//ConfigButton

    /// <summary>
    /// 入力されたKeyCodeを取得する処理
    /// </summary>
    /// <returns></returns>
    private KeyCode GetInputKeyCode() {
        foreach (KeyCode code in Enum.GetValues(typeof(KeyCode))) {//入力キー取得
            if (Input.GetKeyDown(code)) {
                return code;
            }//if
        }//foreach
        return KeyCode.Space;
    }//GetInputKeyCode

    /// <summary>
    /// コンフィグを開始する処理
    /// </summary>
    /// <param name="inputButton">押されたボタンオブジェクト</param>
    /// <param name="isKeyBoard">入力されたコンフィグタイプ</param>
    /// <param name="code">入力されたキーコード</param>
    private void ConfigStart(GameObject inputButton,bool isKeyBoard,KeyCode code) {
        if ((isKeyBoard && code.ToString().Contains("Joystick")) ||
            !isKeyBoard && !code.ToString().Contains("Joystick"))
            return;
        if (isKeyBoard && !code.ToString().Contains("Joystick")) {
            _isKeyBoardConfig = true;
        }//if
        if (!isKeyBoard && code.ToString().Contains("Joystick")) {
            _isControllerConfig = true;
        }//if
        _inputButton = inputButton;
        _inputButton.GetComponent<Animator>().enabled = false;
        _inputButton.GetComponent<Button>().interactable = false;
        _inputButton.GetComponent<Image>().color = new Color(0, 180, 255, 255);
    }//ConfigStart

    /// <summary>
    /// コンフィグ更新の実行チェック処理
    /// </summary>
    /// <param name="code">入力されたキーコード</param>
    private void ConfigUpdateCheck(KeyCode code) {
        if ((_isControllerConfig && !code.ToString().Contains("Joystick")) ||
            _isKeyBoardConfig && code.ToString().Contains("Joystick"))
            return;
        string axesButtonText = InputManagerDataEdit.EditText_InputKeyCodeText_To_AxesButtonText(code.ToString().ToLower());//入力文字変換
        if (InputManagerDataEdit.GetNonTargetTextCheck_AxesButton(axesButtonText))//対象外文字の選別
            return;
        ConfigUpdate(axesButtonText);
    }//ConfigUpdateCheck

    /// <summary>
    /// コンフィグを更新する処理
    /// </summary>
    /// <param name="changeInputValue">AxesButtonで入力できる対象文字</param>
    private void ConfigUpdate(string changeInputValue) {
        string axesNameText;
        InputManagerDataEdit.InputDataType nowInputValueType;
        (axesNameText, nowInputValueType) = InputManagerDataEdit.ConfigButtonInfoSelect(_inputButton.name.ToString());
        string nowInputValue = _inputButton.transform.GetChild(0).GetComponent<Text>().text.ToLower();
        InputManagerDataEdit.ConfigDataUpdate(axesNameText, changeInputValue,nowInputValue, nowInputValueType);
        InputManagerDataEdit.ConfigButtonsTextUpdate(this.gameObject);

        _inputButton.GetComponent<Animator>().enabled = true;
        _inputButton.GetComponent<Image>().color = Color.red;
        _inputButton.GetComponent<Button>().interactable = true;
        _inputButton.GetComponent<Selectable>().Select();
        _isKeyBoardConfig = false;
        _isControllerConfig = false;
        _isInputKeyUpdatePossible = false;
    }//ConfigUpdate

    /// <summary>
    /// Defaultボタンが押された際の処理
    /// </summary>
    public void DefaultButton() {
        SaveManager.InputDataDelete();
        InputManagerDataEdit.InputDataUpdate();
        InputManagerDataEdit.ConfigButtonsTextUpdate(this.gameObject);
    }//DefaultButton

}//KeyConfigButtonMove
