using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// �L�[�R���t�B�O��ʏ�̃{�^������
/// �X�V����:20211004
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
    /// �L�[�{�[�h�p�R���t�B�O�{�^���������ꂽ�ۂ̏���
    /// </summary>
    /// <param name="inputButton"></param>
    public void KeyBoardConfigButton(GameObject inputButton) {
        ConfigButton(inputButton, true);
    }//KeyConfigButton

    /// <summary>
    /// �R���g���[���p�R���t�B�O�{�^���������ꂽ�ۂ̏���
    /// </summary>
    /// <param name="inputButton"></param>
    public void ControllerConfigButton(GameObject inputButton) {
        ConfigButton(inputButton, false);
    }//ControllerCOnfigButton

    /// <summary>
    /// �R���t�B�O�{�^���������ꂽ�ۂ̏���
    /// </summary>
    /// <param name="inputButton">�����ꂽ�{�^���I�u�W�F�N�g</param>
    /// <param name="isKeyBoard">���͂��ꂽ�R���t�B�O�^�C�v(KeyBoard�œ��͂��ꂽ�ꍇ��True)</param>
    private void ConfigButton(GameObject inputButton, bool isKeyBoard) {
        ConfigStart(inputButton, isKeyBoard, GetInputKeyCode());
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
        return KeyCode.Space;
    }//GetInputKeyCode

    /// <summary>
    /// �R���t�B�O���J�n���鏈��
    /// </summary>
    /// <param name="inputButton">�����ꂽ�{�^���I�u�W�F�N�g</param>
    /// <param name="isKeyBoard">���͂��ꂽ�R���t�B�O�^�C�v</param>
    /// <param name="code">���͂��ꂽ�L�[�R�[�h</param>
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
    /// ���͂��ꂽ�L�[�̊m�F����
    /// </summary>
    private void InputKeyCheck() {
        ConfigUpdateCheck(GetInputKeyCode());
    }//InputKeyCheack

    /// <summary>
    /// �R���t�B�O�X�V�̎��s�`�F�b�N����
    /// </summary>
    /// <param name="code">���͂��ꂽ�L�[�R�[�h</param>
    private void ConfigUpdateCheck(KeyCode code) {
        if ((_isControllerConfigButton && !code.ToString().Contains("Joystick")) ||
            _isKeyBoardConfigButton && code.ToString().Contains("Joystick"))
            return;
        string axesButtonText = InputManagerEdit.EditText_InputKeyCodeText_To_AxesButtonText(code.ToString().ToLower());//���͕����ϊ�
        if (InputManagerEdit.GetNonTargetTextCheck_AxesButton(axesButtonText))//�ΏۊO�����̑I��
            return;
        ConfigUpdate(axesButtonText);
    }//ConfigUpdateCheck

    /// <summary>
    /// �R���t�B�O���X�V���鏈��
    /// </summary>
    /// <param name="changeInputValue">AxesButton�œ��͂ł���Ώە���</param>
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
    /// Default�{�^���������ꂽ�ۂ̏���
    /// </summary>
    public void DefaultButton() {
        SaveManager.InputDataDelete();
        InputManagerEdit.InputManagerUpdate();
        ConfigButtonsTextUpdate();
    }//DefaultButton

    /// <summary>
    /// �R���t�B�O�{�^���S�Ă̕����X�V���s������
    /// </summary>
    private void ConfigButtonsTextUpdate() {
        Transform buttonsInfo = GameObject.Find("ButtonCanvas").transform;
        foreach (Transform childTransform in buttonsInfo) {
            if (childTransform.name.ToString() == "Default")//�L�[�{�[�h�E�R���g���[���{�^����S�Đݒ肷��܂ŌJ��Ԃ�
                break;
            string fixName;
            InputManagerEdit.InputDataType type;
            (fixName, type) = ConfigButtonInfoSelect(childTransform.name.ToString());
            childTransform.GetChild(0).GetComponent<Text>().text = InputManagerEdit.InputTextUpdate(fixName, type);
        }//foreach
    }//ConfigButtonsTextUpdate

    /// <summary>
    /// �R���t�B�O�{�^���̏��I������
    /// </summary>
    /// <param name="checkButtonName">���ׂ�{�^���I�u�W�F�N�g��</param>
    /// <returns>
    /// string �C��������(���ׂ�{�^������InputManager�̑Ώ�Axes��)
    /// InputDataType �Ώۂ�InputDataType�^�C�v
    /// </returns>
    private (string,InputManagerEdit.InputDataType) ConfigButtonInfoSelect(string checkButtonName) {
        string editText = checkButtonName;
        InputManagerEdit.InputDataType type = InputManagerEdit.InputDataType.JoystickNegative;
        if (editText.Contains("Key")) {//�L�[�{�[�h�{�^���̏ꍇ
            editText = editText.Replace("Key", "");
            type = InputManagerEdit.InputDataType.KeyPositive;
        }//if
        if (editText.Contains("Controller")) {//�R���g���[���{�^���̏ꍇ
            editText = editText.Replace("Controller", "");
            type = InputManagerEdit.InputDataType.JoystickPositive;
        }//if
        switch (editText) {//editText�̕ύX
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
