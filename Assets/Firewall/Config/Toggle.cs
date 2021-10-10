using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System;

public class Toggle : MonoBehaviour {
    public GameObject DebugWindow;

    // Start と Updateは省略
    private bool _val;

    public Text _text;

    public void OnValueChanged(bool val) {

        // デバッグウインドウの表示切り替え
        // ToggleがONの時表示（Active）
        // ToggleがOFFの時非表示（Deactive）
        DebugWindow.SetActive(val);
        _val = val;
    }

    private void Update() {

        if (Input.anyKeyDown) {
            foreach (KeyCode code in Enum.GetValues(typeof(KeyCode))) {//入力キー取得
                if (Input.GetKeyDown(code)) {
                }//if
            }//foreach

        }
        if (Input.GetKeyDown(KeyCode.Alpha9)) {
            DebugWindow.SetActive(_val);
            _val = !_val;
        }
        if (Input.GetKeyDown(KeyCode.Alpha8)) {
            _text.text = "";
        }
    }

}