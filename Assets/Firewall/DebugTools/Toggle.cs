using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System;

public class Toggle : MonoBehaviour {
    public GameObject DebugWindow;

    // Start �� Update�͏ȗ�
    private bool _val;

    public Text _text;

    public void OnValueChanged(bool val) {

        // �f�o�b�O�E�C���h�E�̕\���؂�ւ�
        // Toggle��ON�̎��\���iActive�j
        // Toggle��OFF�̎���\���iDeactive�j
        DebugWindow.SetActive(val);
        _val = val;
    }

    private void Update() {

        if (Input.anyKeyDown) {
            foreach (KeyCode code in Enum.GetValues(typeof(KeyCode))) {//���̓L�[�擾
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