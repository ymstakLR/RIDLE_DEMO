//éQçlURL:https://hk-ryukyu.club/hideto/archives/49
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Practice : MonoBehaviour {

    public delegate void Dele();

    public void Method(Dele deleMethod) {
        deleMethod();
    }

    public void Method(Action actionMethod) {
        actionMethod();
    }
    public void Method(Action<string> actionStMethod) {
        actionStMethod(actionStMethod.ToString());
    }

    public event Action Ev = null;
    public void Method() {
        if(Ev != null) {
            Ev();
        }
    }

}
