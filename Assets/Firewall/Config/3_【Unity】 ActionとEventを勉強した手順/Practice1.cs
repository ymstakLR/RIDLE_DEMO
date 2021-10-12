//QlURL:https://hk-ryukyu.club/hideto/archives/49
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Practice1 : MonoBehaviour {

    private void Start() {
        Method();
    }


    public void Method() {
        Practice practice = new Practice();

        Practice.Dele deleMethod = new Practice.Dele(A);
        deleMethod += B;
        deleMethod += C;
        practice.Method(deleMethod);
        Action actionMethod = A;
        actionMethod += B;
        actionMethod += C;
        practice.Method(actionMethod);
        Action<string> actionStMethod = A;
        actionStMethod += B;
        practice.Method(actionStMethod);
        Action deleMethod3 = delegate {
            Debug.Log("ˆ—Š®—¹");
        };
        practice.Method(deleMethod3);
        Action deleMethod4 = () => Debug.Log("ƒ‰ƒ€ƒ_");
        practice.Method(deleMethod4);

        practice.Ev += () => Debug.Log("Event");
        practice.Ev += () => Debug.Log("Event2");
        practice.Method();
    }

    public void A() {
        Debug.Log("kannryou");
    }
    public void B() {
        Debug.Log("ˆ—Š®—¹B");
    }
    public void C() {
        Debug.Log("ˆ—Š®—¹C");
    }
    public void A(string st) {
        Debug.Log("kannryou");
    }
    public void B(string st) {
        Debug.Log("ˆ—Š®—¹B");
    }
    public void C(string st) {
        Debug.Log("ˆ—Š®—¹C");
    }


}
