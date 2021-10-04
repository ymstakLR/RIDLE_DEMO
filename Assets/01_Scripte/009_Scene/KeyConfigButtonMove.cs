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
        if (_isSubmitButtonUp) {
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
        foreach (KeyCode code in Enum.GetValues(typeof(KeyCode))) {
            if (Input.GetKeyDown(code)) {
                if ((isKeyBoard && code.ToString().Contains("Joystick")) ||
                    !isKeyBoard && !code.ToString().Contains("Joystick"))
                    return;
                if (isKeyBoard && !code.ToString().Contains("Joystick"))
                    _isKeyBoardConfigButton = true;

                if (!isKeyBoard && code.ToString().Contains("Joystick"))
                    _isControllerConfigButton = true;

                _inputButton = inputButton;
                _inputButton.GetComponent<Animator>().enabled = false;
                _inputButton.GetComponent<Button>().interactable = false;
                _inputButton.GetComponent<Image>().color = new Color(0, 180, 255, 255);
            }//if
        }//foreach
    }//ConfigButton


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
        string fixName = checkButtonName;
        InputManagerEdit.InputDataType type = InputManagerEdit.InputDataType.JoystickNegative;
        if (fixName.Contains("Key")) {//�L�[�{�[�h�{�^���̏ꍇ
            fixName = fixName.Replace("Key", "");
            type = InputManagerEdit.InputDataType.KeyPositive;
        }//if
        if (fixName.Contains("Controller")) {//�R���g���[���{�^���̏ꍇ
            fixName = fixName.Replace("Controller", "");
            type = InputManagerEdit.InputDataType.JoystickPositive;
        }//if
        switch (fixName) {//fixName�̕ύX
            case "Up":
                fixName = "Vertical";
                break;
            case "Down":
                fixName = "Vertical";
                type = InputManagerEdit.InputDataType.KeyNegative;
                break;
            case "Left":
                fixName = "Horizontal";
                type = InputManagerEdit.InputDataType.KeyNegative;
                break;
            case "Right":
                fixName = "Horizontal";
                break;
            default:
                break;
        }//switch
        return (fixName, type);
    }//ConfigButtonInfoSelect

    /// <summary>
    /// ���͂��ꂽ�L�[�̊m�F����
    /// </summary>
    void InputKeyCheck() {
        if (!Input.anyKeyDown)
            return;
        foreach (KeyCode code in Enum.GetValues(typeof(KeyCode))) {
            if (Input.GetKeyDown(code)) {
                if ((_isControllerConfigButton && !code.ToString().Contains("Joystick"))||
                    _isKeyBoardConfigButton && code.ToString().Contains("Joystick")) 
                    return;
                string editText = InputManagerEdit.InputButtonTextEdit(code.ToString().ToLower());//���͕����ϊ�
                if (!InputManagerEdit.InputTextCheack(editText))//�ΏۊO�����̑I��
                    return;
  
                string fixName;
                InputManagerEdit.InputDataType type;
                (fixName,type) = ConfigButtonInfoSelect(_inputButton.name.ToString());
                InputManagerEdit.InputTextDuplicationCheack(editText, _inputButton.transform.GetChild(0).GetComponent<Text>().text.ToLower(),fixName,type);
                ConfigButtonsTextUpdate();
                break;
            }//if
        }//foreach
        _inputButton.GetComponent<Animator>().enabled = true;
        _inputButton.GetComponent<Image>().color = Color.red;
        _inputButton.GetComponent<Button>().interactable = true;
        _inputButton.GetComponent<Selectable>().Select();
        _isKeyBoardConfigButton = false;
        _isControllerConfigButton = false;
        _isSubmitButtonUp = false;
    }//DownKeyCheack

}//KeyConfigButtonMove
