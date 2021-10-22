using MBLDefine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// �L�[�R���t�B�O��ʏ�̃{�^������
/// �X�V����:20211006
/// </summary>
public class KeyConfigButtonMove : MonoBehaviour {
    private GameObject _inputButton;//�R���t�B�O�Ώۂ̃{�^���I�u�W�F�N�g

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

///�{�^�����͊֘A�̏���

    /// <summary>
    /// �L�[�{�[�h�p�R���t�B�O�{�^���������ꂽ�ۂ̏���
    /// </summary>
    /// <param name="inputButton"></param>
    public void KeyBoardConfigButton(GameObject inputButton) {
        ConfigButton(inputButton, InputType.keyButton);
    }//KeyConfigButton

    /// <summary>
    /// �R���g���[���p�R���t�B�O�{�^���������ꂽ�ۂ̏���
    /// </summary>
    /// <param name="inputButton"></param>
    public void ControllerConfigButton(GameObject inputButton) {
        ConfigButton(inputButton, InputType.joystickButton);
    }//ControllerCOnfigButton

    /// <summary>
    /// �R���g���[���p�R���t�B�O�{�^���������ꂽ�ۂ̏���
    /// </summary>
    /// <param name="inputButton"></param>
    public void AxesPositiveConfigButton(GameObject inputButton) {
        ConfigButton(inputButton, InputType.axesPositiveButton);
    }//ControllerCOnfigButton

    /// <summary>
    /// �R���g���[���p�R���t�B�O�{�^���������ꂽ�ۂ̏���
    /// </summary>
    /// <param name="inputButton"></param>
    public void AxesNegativeConfigButton(GameObject inputButton) {
        ConfigButton(inputButton, InputType.axesNegativeButton);
    }//ControllerCOnfigButton

    /// <summary>
    /// �R���t�B�O�{�^���������ꂽ�ۂ̏���
    /// </summary>
    /// <param name="inputButton">�����ꂽ�{�^���I�u�W�F�N�g</param>
    /// <param name="isKeyBoard">���͂��ꂽ�R���t�B�O�^�C�v(KeyBoard�œ��͂��ꂽ�ꍇ��True)</param>
    private void ConfigButton(GameObject inputButton, InputType inputType) {
        ConfigStart(inputButton, inputType, GetInputKeyCode());
    }//ConfigButton

    /// <summary>
    /// ���͂��ꂽKeyCode���擾���鏈��
    /// </summary>
    /// <returns></returns>
    private KeyCode GetInputKeyCode() {
        foreach (KeyCode code in Enum.GetValues(typeof(KeyCode))) {//���̓L�[�擾
            if (Input.GetKeyDown(code)) {
                return code;
            }//if
        }//foreach
        return KeyCode.None;
    }//GetInputKeyCode

    /// <summary>
    /// �R���t�B�O���J�n���鏈��
    /// </summary>
    /// <param name="inputButton">�����ꂽ�{�^���I�u�W�F�N�g</param>
    /// <param name="inputType">���͂��ꂽ�R���t�B�O�^�C�v</param>
    /// <param name="code">���͂��ꂽ�L�[�R�[�h</param>
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
    /// �R���t�B�O�X�V�̎��s�`�F�b�N����
    /// </summary>
    /// <param name="code">���͂��ꂽ�L�[�R�[�h</param>
    private void ConfigUpdateCheck(KeyCode code) {
        if (((_isInputType != InputType.joystickButton && _isInputType != InputType.none) && code.ToString().Contains("Joystick")) ||
            _isInputType == InputType.joystickButton && !code.ToString().Contains("Joystick"))
            return;
        string axesButtonText = InputManagerDataEdit.EditText_InputKeyCodeText_To_AxesButtonText(code.ToString().ToLower());//���͕����ϊ�
        if (InputManagerDataEdit.GetNonTargetTextCheck_AxesButton(axesButtonText))//�ΏۊO�����̑I��
            return;
        //Debug.Log("axesButtonText__" + axesButtonText+"__code__"+code);
        ConfigUpdate(code);
    }//ConfigUpdateCheck

    /// <summary>
    /// �R���t�B�O���X�V���鏈��
    /// </summary>
    /// <param name="changeInputValue">AxesButton�œ��͂ł���Ώە���</param>
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
    /// Default�{�^���������ꂽ�ۂ̏���
    /// </summary>
    public void DefaultButton() {
        SaveManager.InputDataDelete();
        //InputManagerDataEdit.InputDataUpdate();
        InputManagerDataEdit.ConfigButtonsTextUpdate(this.gameObject);
    }//DefaultButton

}//KeyConfigButtonMove