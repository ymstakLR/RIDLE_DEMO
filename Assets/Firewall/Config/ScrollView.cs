using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollView : MonoBehaviour {
    public GameObject DebugText;
    public GameObject DebugWindow;

    void Awake() {
        Application.logMessageReceived += LoggedCb;  // ログ出力時のコールバックを登録
    }

    // Start と Updateは省略

    public void LoggedCb(string logstr, string stacktrace, LogType type) {
        DebugText.GetComponent<Text>().text += "["+DateTime.Now.ToLongTimeString()+"]"+logstr;
        DebugText.GetComponent<Text>().text += "\n";
        // 常にTextの最下部（最新）を表示するように強制スクロール
        DebugWindow.GetComponent<ScrollRect>().verticalNormalizedPosition = 0;
    }
}