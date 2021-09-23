using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class KeyConfigButtonMove : MonoBehaviour {
    private GameObject _inputButton;

    private bool _isKeyConfigButton;
    private bool _isSubmitButtonUp;

    private void Update() {
        if (_isKeyConfigButton) {
            if (Input.GetButtonUp("Submit")) {
                _isSubmitButtonUp = true;
            }
        }
        if (_isSubmitButtonUp) {
            DownKeyCheck();
            test();
        }
            
    }//Update
    
    public void KeyConfigButton(GameObject inputButton) {
        _inputButton = inputButton;
        _inputButton.GetComponent<Animator>().enabled = false;
        _inputButton.GetComponent<Button>().interactable = false;
        _inputButton.GetComponent<Image>().color = new Color(0, 180, 255, 255);
        _isKeyConfigButton = true;
    }//KeyConfigButton

    /// <summary>
    /// 入力されたキーの確認処理
    /// </summary>
    void DownKeyCheck() {
        if (Input.anyKeyDown) {
            foreach (KeyCode code in Enum.GetValues(typeof(KeyCode))) {
                if (Input.GetKeyDown(code)) {
                    Debug.Log(code);
                    _inputButton.transform.GetChild(0).GetComponent<Text>().text = code.ToString();
                    break;
                }//if
            }//foreach
            _inputButton.GetComponent<Animator>().enabled = true;
            _inputButton.GetComponent<Image>().color = Color.red;
            _inputButton.GetComponent<Button>().interactable = true;

            _inputButton.GetComponent<Selectable>().Select();
            _isKeyConfigButton = false;
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
