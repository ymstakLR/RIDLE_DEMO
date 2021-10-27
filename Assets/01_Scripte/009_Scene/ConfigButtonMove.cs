using ConfigDataDefine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// キーコンフィグ画面上のボタン処理
/// 更新日時:20211026
/// </summary>
public class ConfigButtonMove : MonoBehaviour {
    private GameObject _inputButton;//コンフィグ対象のボタンオブジェクト

    [SerializeField]
    private GameObject _axisButtonCanvas;
    [SerializeField]
    private GameObject _keyButtonCanvas;

    private bool _isInputKeyUpdatePossible;

    private CustomStandaloneInputModule _inputModule;
    private KeyCode _changeCode = KeyCode.None;


    private enum InputType {
        keyButton= 0,
        joystickButton= 1,
        axisPositiveButton = 2,
        axisNegativeButton = 3,
        none
    }//InputType
    private InputType _isInputType;

    private void Awake() {
        _inputModule = GameObject.Find("EventSystem").GetComponent<CustomStandaloneInputModule>();
        _isInputType = InputType.none;
        InputManagerDataEdit.ConfigButtonsTextUpdate(_axisButtonCanvas);
        InputManagerDataEdit.ConfigButtonsTextUpdate(_keyButtonCanvas);
    }//Awake

    private void Update() {
        if(_isInputType ==InputType.none && Input.GetKeyUp(_changeCode)) {
            _inputModule.isKeyInvalid = false;
            _inputButton.GetComponent<Button>().interactable = true;
            _inputButton.GetComponent<Selectable>().Select();
            _changeCode = KeyCode.None;
        }//if
        if (_isInputType!=InputType.none) {
            if (ConfigManager.Instance.config.GetKeyUp(ConfigData.Submit.String)) {
                _isInputKeyUpdatePossible = true;
            }//if
        }//if
        if (_isInputKeyUpdatePossible && Input.anyKeyDown) {
            ConfigUpdateCheck(ConfigManager.Instance.config.GetInputKeyCode());
        }//if
    }//Update

    #region ConfigStart
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
    }//ControllerConfigButton

    /// <summary>
    /// コントローラ用コンフィグボタンが押された際の処理
    /// </summary>
    /// <param name="inputButton"></param>
    public void AxisPositiveConfigButton(GameObject inputButton) {
        ConfigButton(inputButton, InputType.axisPositiveButton);
    }//AxisPositveButton

    /// <summary>
    /// コントローラ用コンフィグボタンが押された際の処理
    /// </summary>
    /// <param name="inputButton"></param>
    public void AxisNegativeConfigButton(GameObject inputButton) {
        ConfigButton(inputButton, InputType.axisNegativeButton);
    }//AxisNegativeConfigButton

    /// <summary>
    /// コンフィグボタンが押された際の処理
    /// </summary>
    /// <param name="inputButton">押されたボタンオブジェクト</param>
    /// <param name="isKeyBoard">入力されたコンフィグタイプ(KeyBoardで入力された場合にTrue)</param>
    private void ConfigButton(GameObject inputButton, InputType inputType) {
        ConfigStart(inputButton, inputType, ConfigManager.Instance.config.GetInputKeyCode());
    }//ConfigButton

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
    #endregion ConfigStart

    #region ConfigUpdate
    /// <summary>
    /// コンフィグ更新の実行チェック処理
    /// </summary>
    /// <param name="code">入力されたキーコード</param>
    private void ConfigUpdateCheck(KeyCode code) {
        if (((_isInputType != InputType.joystickButton && _isInputType != InputType.none) && code.ToString().Contains("Joystick")) ||
            _isInputType == InputType.joystickButton && !code.ToString().Contains("Joystick"))
            return;
        string axisButtonText = InputManagerDataEdit.EditText_InputCodeText_To_AxesButtonText(code.ToString().ToLower());//入力文字変換
        if (InputManagerDataEdit.GetNonTargetTextCheck(axisButtonText))//対象外文字の選別
            return;
        ConfigUpdate(code);
    }//ConfigUpdateCheck

    /// <summary>
    /// コンフィグを更新する処理
    /// </summary>
    /// <param name="changeInputValue">AxesButtonで入力できる対象文字</param>
    private void ConfigUpdate(KeyCode changeCode) {
        string configNameText;
        InputManagerDataEdit.InputDataType nowInputValueType;
        (configNameText, nowInputValueType) = InputManagerDataEdit.ConfigButtonInfoSelect(_inputButton.name.ToString());
        KeyCode nowInputKeyCode = InputManagerDataEdit.GetConficButtonKeyCode(configNameText, nowInputValueType);

        InputManagerDataEdit.ConfigDataUpdate(configNameText, changeCode,nowInputKeyCode, (int)_isInputType);
        InputManagerDataEdit.ConfigButtonsTextUpdate(_axisButtonCanvas);
        InputManagerDataEdit.ConfigButtonsTextUpdate(_keyButtonCanvas);

        _inputButton.GetComponent<Animator>().enabled = true;
        _inputButton.GetComponent<Image>().color = Color.red;
        _inputButton.GetComponent<Button>().interactable = true;
        _inputButton.GetComponent<Selectable>().Select();
        _isInputType = InputType.none;
        _isInputKeyUpdatePossible = false;
        _inputModule.isKeyInvalid = true;
        _changeCode = changeCode;
    }//ConfigUpdate
    #endregion ConfigUpdate

    /// <summary>
    /// Defaultボタンが押された際の処理
    /// </summary>
    public void DefaultButton(GameObject inputObject) {
        Config config = new Config();
        config.SetDefaultConfig();
        config.SaveConfigFile();
        InputManagerDataEdit.ConfigButtonsTextUpdate(_axisButtonCanvas);
        InputManagerDataEdit.ConfigButtonsTextUpdate(_keyButtonCanvas);
        _inputButton = inputObject;
        _inputButton.GetComponent<Button>().interactable = false;
        _changeCode = ConfigManager.Instance.config.GetInputKeyCode();
        _isInputType = InputType.none;
    }//DefaultButton



}//ConfigButtonMove