using MBLDefine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class KeyConfigTest : MonoBehaviour {

    private void Start() {
        Debug.Log(InputManager.Instance.keyConfig.GetInputKeyCodeCheck(Key.VerticalP.String));
        Debug.Log(InputManager.Instance.axesConfig.GetInputAxesCodeCheck(Axes.Horizontal.String,AxesConfig.AxesType.Positive));
    }

    private void Update() {
        //if (Input.GetKeyDown(InputManager.Instance.keyConfig.GetInputKeyCodeCheck(Key.Cancel.String))) {
        //    Debug.Log("kakunin");
        //}


    }

}
