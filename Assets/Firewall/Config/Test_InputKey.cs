using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Test_InputKey : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("NormalJump")) {
            //Debug.Log(GetInputKeyCode());
        }
        if (Input.GetButtonDown("Submit")) {
            //Debug.Log(GetInputKeyCode());
        }
        //Debug.Log();
    }

    private KeyCode GetInputKeyCode() {
        foreach (KeyCode code in Enum.GetValues(typeof(KeyCode))) {//ì¸óÕÉLÅ[éÊìæ
            if (Input.GetKeyDown(code)) {
                return code;
            }//if
        }//foreach
        return KeyCode.Space;
    }//GetInputKeyCode
}
