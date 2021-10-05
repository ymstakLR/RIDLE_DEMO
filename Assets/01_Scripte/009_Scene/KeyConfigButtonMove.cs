using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// キーコンフィグ画面上のボタン処理
/// 更新日時:20211004
/// </summary>
public class KeyConfigButtonMove : MonoBehaviour {
    private GameObject _inputButton;

    private bool _isKeyBoardConfigButton;
    private bool _isControllerConfigButton;
    private bool _isSubmitButtonUp;

    private void Awake() {
        InputManagerEdit.InputManagerUpdate();
        ConfigButtonsTextUpdate();
    }//Awake

    private void Update() {
        if (_isKeyBoardConfigButton || _isControllerConfigButton) {
            if (Input.GetButtonUp("Submit")) {
                _isSubmitButtonUp = true;
            }//if
        }//if
        if (_isSubmitButtonUp && Input.anyKeyDown) {
            InputKeyCheck();
        }//if
    }//Update

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
            _isKeyBoardConfigButton = true;
        }//if
        if (!isKeyBoard && code.ToString().Contains("Joystick")) {
            _isControllerConfigButton = true;
        }//if
        _inputButton = inputButton;
        _inputButton.GetComponent<Animator>().enabled = false;
        _inputButton.GetComponent<Button>().interactable = false;
        _inputButton.GetComponent<Image>().color = new Color(0, 180, 255, 255);
    }//ConfigStart

    /// <summary>
    /// 入力されたキーの確認処理
    /// </summary>
    private void InputKeyCheck() {
        ConfigUpdateCheck(GetInputKeyCode());
    }//InputKeyCheack

    /// <summary>
    /// コンフィグ更新の実行チェック処理
    /// </summary>
    /// <param name="code">入力されたキーコード</param>
    private void ConfigUpdateCheck(KeyCode code) {
        if ((_isControllerConfigButton && !code.ToString().Contains("Joystick")) ||
            _isKeyBoardConfigButton && code.ToString().Contains("Joystick"))
            return;
        string axesButtonText = InputManagerEdit.EditText_InputKeyCodeText_To_AxesButtonText(code.ToString().ToLower());//入力文字変換
        if (InputManagerEdit.GetNonTargetTextCheck_AxesButton(axesButtonText))//対象外文字の選別
            return;
        ConfigUpdate(axesButtonText);
    }//ConfigUpdateCheck

    /// <summary>
    /// コンフィグを更新する処理
    /// </summary>
    /// <param name="changeInputValue">AxesButtonで入力できる対象文字</param>
    private void ConfigUpdate(string changeInputValue) {
        string axesNameText;
        InputManagerEdit.InputDataType nowInputValueType;
        (axesNameText, nowInputValueType) = ConfigButtonInfoSelect(_inputButton.name.ToString());
        string nowInputValue = _inputButton.transform.GetChild(0).GetComponent<Text>().text.ToLower();
        InputManagerEdit.InputTextDuplicationCheack(changeInputValue,nowInputValue, axesNameText, nowInputValueType);
        ConfigButtonsTextUpdate();

        _inputButton.GetComponent<Animator>().enabled = true;
        _inputButton.GetComponent<Image>().color = Color.red;
        _inputButton.GetComponent<Button>().interactable = true;
        _inputButton.GetComponent<Selectable>().Select();
        _isKeyBoardConfigButton = false;
        _isControllerConfigButton = false;
        _isSubmitButtonUp = false;
    }

    /// <summary>
    /// Defaultボタンが押された際の処理
    /// </summary>
    public void DefaultButton() {
        SaveManager.InputDataDelete();
        InputManagerEdit.InputManagerUpdate();
        ConfigButtonsTextUpdate();
    }//DefaultButton

    /// <summary>
    /// コンフィグボタン全ての文字更新を行う処理
    /// </summary>
    private void ConfigButtonsTextUpdate() {
        Transform buttonsInfo = GameObject.Find("ButtonCanvas").transform;
        foreach (Transform childTransform in buttonsInfo) {
            if (childTransform.name.ToString() == "Default")//キーボード・コントローラボタンを全て設定するまで繰り返す
                break;
            string fixName;
            InputManagerEdit.InputDataType type;
            (fixName, type) = ConfigButtonInfoSelect(childTransform.name.ToString());
            childTransform.GetChild(0).GetComponent<Text>().text = InputManagerEdit.InputTextUpdate(fixName, type);
        }//foreach
    }//ConfigButtonsTextUpdate

    /// <summary>
    /// コンフィグボタンの情報選択処理
    /// </summary>
    /// <param name="checkButtonName">調べるボタンオブジェクト名</param>
    /// <returns>
    /// string 修正した名(調べるボタン名→InputManagerの対象Axes名)
    /// InputDataType 対象のInputDataTypeタイプ
    /// </returns>
    private (string,InputManagerEdit.InputDataType) ConfigButtonInfoSelect(string checkButtonName) {
        string editText = checkButtonName;
        InputManagerEdit.InputDataType type = InputManagerEdit.InputDataType.JoystickNegative;
        if (editText.Contains("Key")) {//キーボードボタンの場合
            editText = editText.Replace("Key", "");
            type = InputManagerEdit.InputDataType.KeyPositive;
        }//if
        if (editText.Contains("Controller")) {//コントローラボタンの場合
            editText = editText.Replace("Controller", "");
            type = InputManagerEdit.InputDataType.JoystickPositive;
        }//if
        switch (editText) {//editTextの変更
            case "Up":
                editText = "Vertical";
                break;
            case "Down":
                editText = "Vertical";
                type = InputManagerEdit.InputDataType.KeyNegative;
                break;
            case "Left":
                editText = "Horizontal";
                type = InputManagerEdit.InputDataType.KeyNegative;
                break;
            case "Right":
                editText = "Horizontal";
                break;
            default:
                break;
        }//switch
        return (editText, type);
    }//ConfigButtonInfoSelect



}//KeyConfigButtonMove
