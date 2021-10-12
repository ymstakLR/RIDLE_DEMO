//éQçlURL:https://anchan828.hatenablog.jp/entry/2013/02/27/184519
using UnityEngine;
using System;

public class Move : MonoBehaviour {
    public KeyCode runKeyCode = KeyCode.LeftShift;
    public EventHandler Walk, Run;

    void Update() {
        if (Input.GetKey(runKeyCode))
            Run(this, EventArgs.Empty);
        else
            Walk(this, EventArgs.Empty);
    }
}