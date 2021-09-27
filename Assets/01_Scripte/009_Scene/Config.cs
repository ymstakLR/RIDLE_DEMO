using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class Config : MonoBehaviour {
    private GameObject _inputButton;

    private bool _isKeyBoardConfigButton;
    private bool _isControllerConfigButton;
    private bool _isSubmitButtonUp;

    private void Awake() {
        InputManagerEdit.InputManagerUpdate();
        InputButtonTextUpdate();
    }

    /// <summary>
    /// 画面遷移時に各種設定の文字更新を行う処理
    /// </summary>
    private void InputButtonTextUpdate() {
        Transform buttonInfo = GameObject.Find("ButtonCanvas").transform;
        foreach(Transform childTransform in buttonInfo) {
            if (childTransform.name.ToString() == "Default")
                break;
            string fixName;
            InputManagerEdit.InputDataType type;
            (fixName,type)=InputTypeSelect(childTransform.name.ToString());
            childTransform.GetChild(0).GetComponent<Text>().text= InputManagerEdit.InputTextUpdate(fixName, type);
        }//foreach
    }//InputButtonTextUpdate


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


    public void DefaultButton() {
        SaveManager.InputDataDelete();
        InputManagerEdit.InputManagerUpdate();
        InputButtonTextUpdate();
    }
    
    private void ConfigButton(GameObject inputButton,bool isKeyBoard) {
        foreach (KeyCode code in Enum.GetValues(typeof(KeyCode))) {
            if (Input.GetKeyDown(code)) {
                if((isKeyBoard && code.ToString().Contains("Joystick"))||
                    !isKeyBoard && !code.ToString().Contains("Joystick"))
                    return;
                if(isKeyBoard && !code.ToString().Contains("Joystick")) 
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

    public void KeyBoardConfigButton(GameObject inputButton) {
        ConfigButton(inputButton,true);
    }//KeyConfigButton

    public void ControllerConfigButton(GameObject inputButton) {
        ConfigButton(inputButton,false);
    }//ControllerCOnfigButton

    private (string,InputManagerEdit.InputDataType) InputTypeSelect(string inputButtonName) {
        string fixName = inputButtonName;
        InputManagerEdit.InputDataType type = InputManagerEdit.InputDataType.JoystickNegative;
        if (fixName.Contains("Key")) {
            fixName = fixName.Replace("Key", "");
            type = InputManagerEdit.InputDataType.KeyPositive;
        }//if
        if (fixName.Contains("Controller")) {
            fixName = fixName.Replace("Controller", "");
            type = InputManagerEdit.InputDataType.JoystickPositive;
        }//if
        switch (fixName) {//fixNameの変更
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
        
    }//UpdateInputSelect

    /// <summary>
    /// 入力されたキーの確認処理
    /// </summary>
    void DownKeyCheck() {
        if (Input.anyKeyDown) {
            foreach (KeyCode code in Enum.GetValues(typeof(KeyCode))) {
                if (Input.GetKeyDown(code)) {
                    if ((_isControllerConfigButton && !code.ToString().Contains("Joystick"))||
                        _isKeyBoardConfigButton && code.ToString().Contains("Joystick")) 
                        return;
                    string editText = InputManagerEdit.InputTextEdit(code.ToString().ToLower());//入力文字変換
                    if (!InputManagerEdit.InputTextCheack(editText))//対象外文字の選別
                        return;
                    string fixName;
                    InputManagerEdit.InputDataType type;
                    (fixName,type) = InputTypeSelect(_inputButton.name.ToString());

                    //InputManagerEdit.InputDataUpdate(fixName, code.ToString().ToLower(), type);
                    //キーが重複化した場合のそれぞれの入力キーの交換
                    InputManagerEdit.InputDataUpdate(fixName,editText, type);
                    InputManagerEdit.InputManagerUpdate();
                    //表示文字の変更
                    _inputButton.transform.GetChild(0).GetComponent<Text>().text = InputManagerEdit.InputTextUpdate(fixName, type);//ボタン文字の変更

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
            Debug.Log("十字キー右");
        else if(horizontal ==-1)
            Debug.Log("十字キー左");
        else if(horizontal <0)
            Debug.Log("スティック左");
        else
            Debug.Log("スティックー右");
    }
}//KeyConfigButtonMove
