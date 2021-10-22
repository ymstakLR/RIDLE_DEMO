using MBLDefine;
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

    [SerializeField]
    private GameObject _axesButtonCanvas;
    [SerializeField]
    private GameObject _keyButtonCanvas;

    private bool _isInputKeyUpdatePossible;

    private enum InputType {
        keyButton= 0,
        joystickButton= 1,
        axesPositiveButton = 2,
        axesNegativeButton = 3,
        none
    }
    private InputType _isInputType;

    private void Awake() {
        _isInputType = InputType.none;
        InputManagerDataEdit.ConfigButtonsTextUpdate(_axesButtonCanvas);
        InputManagerDataEdit.ConfigButtonsTextUpdate(_keyButtonCanvas);
    }//Awake

    private void Update() {
        if (_isInputType!=InputType.none) {
            if (InputManager.Instance.keyConfig.GetKeyUp(Key.Submit.String)) {
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
        ConfigButton(inputButton, InputType.keyButton);
    }//KeyConfigButton

    /// <summary>
    /// コントローラ用コンフィグボタンが押された際の処理
    /// </summary>
    /// <param name="inputButton"></param>
    public void ControllerConfigButton(GameObject inputButton) {
        ConfigButton(inputButton, InputType.joystickButton);
    }//ControllerCOnfigButton

    /// <summary>
    /// コントローラ用コンフィグボタンが押された際の処理
    /// </summary>
    /// <param name="inputButton"></param>
    public void AxesPositiveConfigButton(GameObject inputButton) {
        ConfigButton(inputButton, InputType.axesPositiveButton);
    }//ControllerCOnfigButton

    /// <summary>
    /// コントローラ用コンフィグボタンが押された際の処理
    /// </summary>
    /// <param name="inputButton"></param>
    public void AxesNegativeConfigButton(GameObject inputButton) {
        ConfigButton(inputButton, InputType.axesNegativeButton);
    }//ControllerCOnfigButton

    /// <summary>
    /// コンフィグボタンが押された際の処理
    /// </summary>
    /// <param name="inputButton">押されたボタンオブジェクト</param>
    /// <param name="isKeyBoard">入力されたコンフィグタイプ(KeyBoardで入力された場合にTrue)</param>
    private void ConfigButton(GameObject inputButton, InputType inputType) {
        ConfigStart(inputButton, inputType, GetInputKeyCode());
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
        return KeyCode.None;
    }//GetInputKeyCode

    /// <summary>
    /// コンフィグを開始する処理
    /// </summary>
    /// <param name="inputButton">押されたボタンオブジェクト</param>
    /// <param name="inputType">入力されたコンフィグタイプ</param>
    /// <param name="code">入力されたキーコード</param>
    private void ConfigStart(GameObject inputButton,InputType inputType,KeyCode code) {
        if (((inputType !=InputType.joystickButton && inputType != InputType.none) && code.ToString().Contains("Joystick")) ||
            inputType == InputType.joystickButton && !code.ToString().Contains("Joystick"))
            return;
        _isInputType = inputType;
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
        if (((_isInputType != InputType.joystickButton && _isInputType != InputType.none) && code.ToString().Contains("Joystick")) ||
            _isInputType == InputType.joystickButton && !code.ToString().Contains("Joystick"))
            return;
        string axesButtonText = InputManagerDataEdit.EditText_InputKeyCodeText_To_AxesButtonText(code.ToString().ToLower());//入力文字変換
        if (InputManagerDataEdit.GetNonTargetTextCheck_AxesButton(axesButtonText))//対象外文字の選別
            return;
        //Debug.Log("axesButtonText__" + axesButtonText+"__code__"+code);
        ConfigUpdate(code);
    }//ConfigUpdateCheck

    /// <summary>
    /// コンフィグを更新する処理
    /// </summary>
    /// <param name="changeInputValue">AxesButtonで入力できる対象文字</param>
    private void ConfigUpdate(KeyCode changeKeyCode) {
        string axesNameText;
        InputManagerDataEdit.InputDataType nowInputValueType;
        (axesNameText, nowInputValueType) = InputManagerDataEdit.ConfigButtonInfoSelect(_inputButton.name.ToString());
        KeyCode nowInputKeyCode = InputManagerDataEdit.GetConficButtonKeyCode(axesNameText, nowInputValueType);

        InputManagerDataEdit.ConfigDataUpdate(axesNameText, changeKeyCode,nowInputKeyCode, (int)_isInputType);
        //InputManagerDataEdit.ConfigButtonsTextUpdate(this.gameObject);

        _inputButton.GetComponent<Animator>().enabled = true;
        _inputButton.GetComponent<Image>().color = Color.red;
        _inputButton.GetComponent<Button>().interactable = true;
        _inputButton.GetComponent<Selectable>().Select();
        _isInputType = InputType.none;
        _isInputKeyUpdatePossible = false;
        InputManagerDataEdit.ConfigButtonsTextUpdate(_axesButtonCanvas);
        InputManagerDataEdit.ConfigButtonsTextUpdate(_keyButtonCanvas);
    }//ConfigUpdate

    /// <summary>
    /// Defaultボタンが押された際の処理
    /// </summary>
    public void DefaultButton() {
        SaveManager.InputDataDelete();
        //InputManagerDataEdit.InputDataUpdate();
        InputManagerDataEdit.ConfigButtonsTextUpdate(this.gameObject);
    }//DefaultButton

}//KeyConfigButtonMove