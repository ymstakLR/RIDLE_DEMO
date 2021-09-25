using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class KeyConfigButtonMove : MonoBehaviour {
    private GameObject _inputButton;

    private bool _isKeyBoardConfigButton;
    private bool _isControllerConfigButton;
    private bool _isSubmitButtonUp;

    private void Update() {
        if (_isKeyBoardConfigButton||_isControllerConfigButton) {
            if (Input.GetButtonUp("Submit")) {
                _isSubmitButtonUp = true;
            }//if
        }//if
        if (_isSubmitButtonUp) {
            DownKeyCheck();
        }//if
    }//Update
    
    private void ConfigButton(GameObject inputButton,bool isKeyBoard) {
        foreach (KeyCode code in Enum.GetValues(typeof(KeyCode))) {
            if (Input.GetKeyDown(code)) {
                if((isKeyBoard && code.ToString().Contains("Joystick"))||
                    !isKeyBoard && !code.ToString().Contains("Joystick"))
                    return;
                if(isKeyBoard && !code.ToString().Contains("Joystick")) {
                    Debug.Log("�L�[�ݒ�̊J�n");
                    _isKeyBoardConfigButton = true;
                }//if
                if (!isKeyBoard && code.ToString().Contains("Joystick")) {
                    Debug.Log("�R���g���[���ݒ�̊J�n");
                    _isControllerConfigButton = true;
                }//if
                _inputButton = inputButton;
                _inputButton.GetComponent<Animator>().enabled = false;
                _inputButton.GetComponent<Button>().interactable = false;
                _inputButton.GetComponent<Image>().color = new Color(0, 180, 255, 255);
            }//if
        }//foreach
    }//ConfigButton

    public void KeyBoardConfigButton(GameObject inputButton) {
        ConfigButton(inputButton,true);
    }//KeyConfigButton

    public void ControllerConfigButton(GameObject inputButton) {
        ConfigButton(inputButton,false);
    }//ControllerCOnfigButton

    private void UpdateInputSelect(string inputButtonName,string code) {
        string fixName = inputButtonName;
        InputManagerEdit.InputDataUpdateType type = InputManagerEdit.InputDataUpdateType.JoystickNegative;
        if (fixName.Contains("Key")) {
            fixName = fixName.Replace("Key", "");
            type = InputManagerEdit.InputDataUpdateType.KeyPositive;
        }//if
        if (fixName.Contains("Controller")) {
            fixName = fixName.Replace("Controller", "");
            type = InputManagerEdit.InputDataUpdateType.JoystickPositive;
        }//if
        Debug.Log(fixName);
        switch (fixName) {//fixName�̕ύX
            case "UP":
                fixName = "Horizontal";
                type = InputManagerEdit.InputDataUpdateType.KeyNegative;
                break;
            case "DOWN":
                fixName = "Horizontal";
                break;
            case "LEFT":
                fixName = "Vertical";
                break;
            case "RIGHT":
                fixName = "Vertical";
                type = InputManagerEdit.InputDataUpdateType.KeyNegative;
                break;
            default:
                Debug.Log("Default");
                break;
        }//switch

        InputManagerEdit.InputDataUpdate(fixName,code, type);

    }//UpdateInputSelect

    /// <summary>
    /// ���͂��ꂽ�L�[�̊m�F����
    /// </summary>
    void DownKeyCheck() {
        if (Input.anyKeyDown) {
            foreach (KeyCode code in Enum.GetValues(typeof(KeyCode))) {
                if (Input.GetKeyDown(code)) {
                    if (code.ToString().Contains("Mouse")){//�}�E�X���͔���
                        return;
                    }
                    if ((_isControllerConfigButton && !code.ToString().Contains("Joystick"))||
                        _isKeyBoardConfigButton && code.ToString().Contains("Joystick")) {
                        Debug.Log("�G���[");
                        return;
                    }
                    if (_isControllerConfigButton &&code.ToString().Contains("Joystick")) {
                        Debug.Log("�R���g���[���ݒ����");
                    }
                    if(_isKeyBoardConfigButton&& !code.ToString().Contains("Joystick")) {
                        Debug.Log("�L�[�ݒ����");
                    }
                    Debug.Log(code);
                    UpdateInputSelect(_inputButton.name.ToString(), code.ToString());
                    _inputButton.transform.GetChild(0).GetComponent<Text>().text = code.ToString();//�{�^�������̕ύX
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
        }//if
    }//DownKeyCheack

    void test() {
        float horizontal = Input.GetAxis("Horizontal");
        if (horizontal == 0)
            return;
        if (horizontal == 1)
            Debug.Log("�\���L�[�E");
        else if(horizontal ==-1)
            Debug.Log("�\���L�[��");
        else if(horizontal <0)
            Debug.Log("�X�e�B�b�N��");
        else
            Debug.Log("�X�e�B�b�N�[�E");
    }
}//KeyConfigButtonMove
