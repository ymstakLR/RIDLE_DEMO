//éQçlURL:https://12px.com/blog/2015/08/unityevent/#toc-3
using UnityEngine;
using System.Collections;
using System;

public class EventTest_EH : MonoBehaviour {
    [SerializeField] public event EventHandler OnEventHandler;
    public void OnEvent() {
        if (OnEventHandler != null) {
            OnEventHandler(this, EventArgs.Empty);
        }
    }

}