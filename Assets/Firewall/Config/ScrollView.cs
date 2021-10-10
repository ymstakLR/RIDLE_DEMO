using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollView : MonoBehaviour {
    public GameObject DebugText;
    public GameObject DebugWindow;

    void Awake() {
        Application.logMessageReceived += LoggedCb;  // ���O�o�͎��̃R�[���o�b�N��o�^
    }

    // Start �� Update�͏ȗ�

    public void LoggedCb(string logstr, string stacktrace, LogType type) {
        DebugText.GetComponent<Text>().text += "["+DateTime.Now.ToLongTimeString()+"]"+logstr;
        DebugText.GetComponent<Text>().text += "\n";
        // ���Text�̍ŉ����i�ŐV�j��\������悤�ɋ����X�N���[��
        DebugWindow.GetComponent<ScrollRect>().verticalNormalizedPosition = 0;
    }
}