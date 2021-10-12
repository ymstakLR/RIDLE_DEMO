//éQçlURL:https://12px.com/blog/2015/08/unityevent/#toc-3
using UnityEngine;
using System.Collections;
using System;

public class Receiver_EH : MonoBehaviour {
    void Start() {
        Debug.Log("Receiver_EH_Start");
        this.gameObject.GetComponent<EventTest_EH>().OnEventHandler += OnEventHandler;
        this.gameObject.GetComponent<EventTest_EH>().OnEventHandler += delegate (object sender, EventArgs e) {
            Debug.Log("OnEventHandler: " + e);
        };
        this.gameObject.GetComponent<EventTest_EH>().OnEvent();
    }
    void OnEventHandler(object sender, EventArgs e) {
        Debug.Log("OnEventHandler: " + e);
    }
}