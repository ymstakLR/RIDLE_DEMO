//éQçlURL:https://12px.com/blog/2015/08/unityevent/#toc-3
using UnityEngine;
using System.Collections;
using System;

public class CustomEventArgs : EventArgs {
    public string message;
}
public delegate void CustomEventHandler(object sender, CustomEventArgs e);

public class EventTest_EH1 : MonoBehaviour {
    [SerializeField] public event CustomEventHandler OnCustomEventHandler;
    public void OnEvent() {
        CustomEventArgs args = new CustomEventArgs();
        args.message = "Fuga";
        if (OnCustomEventHandler != null) {
            Debug.Log("kaishi");
            OnCustomEventHandler(this, args);
        }
            
    }
}