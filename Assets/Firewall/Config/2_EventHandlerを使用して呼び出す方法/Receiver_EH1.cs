//éQçlURL:https://12px.com/blog/2015/08/unityevent/#toc-3
using UnityEngine;
using System.Collections;
using System;

public class Receiver_EH1 : MonoBehaviour {
    void Start() {
        this.gameObject.GetComponent<EventTest_EH1>().OnCustomEventHandler += OnCustomEventHandler;
        this.gameObject.GetComponent<EventTest_EH1>().OnCustomEventHandler += delegate (object sender, CustomEventArgs e) {
            print("OnCustomEventHandler: " + e.message);
        };
        this.gameObject.GetComponent<EventTest_EH1>().OnEvent();
    }
    void OnCustomEventHandler(object sender, CustomEventArgs e) {
        print("OnCustomEventHandler: " + e.message);
    }
}