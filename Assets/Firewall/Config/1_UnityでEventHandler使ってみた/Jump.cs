//éQçlURL:https://anchan828.hatenablog.jp/entry/2013/02/27/184519
using UnityEngine;
using System;
using System.Collections;

public class Jump : MonoBehaviour {
    public float jumpTimeout = 0.15f;
    public KeyCode jumpKeyCode = KeyCode.Space;

    public event EventHandler Jumping, JumpEnd;

    private bool jumping = false;

    void Update() {
        if (Input.GetKey(jumpKeyCode) && !jumping) {
            StartCoroutine(JumpTimeout(jumpTimeout));
        }
    }

    IEnumerator JumpTimeout(float timeout) {
        jumping = true;
        float start = Time.timeSinceLevelLoad;

        while (Time.timeSinceLevelLoad - start <= timeout) {
            Jumping(this, EventArgs.Empty);
            yield return new WaitForEndOfFrame();
        }
        JumpEnd(this, EventArgs.Empty);
        jumping = false;
    }

}