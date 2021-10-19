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
        Debug.Log(InputManager.Instance.keyConfig.GetInputKeyCodeCheck(Key.NormalJump.String,KeyConfig.KeyType.KeyBoard));
        Debug.Log(InputManager.Instance.keyConfig.GetInputKeyCodeCheck(Key.NormalJump.String, KeyConfig.KeyType.JoyStick));
        Debug.Log(InputManager.Instance.axesConfig.GetInputAxesCodeCheck(Axes.Horizontal.String,AxesConfig.AxesType.Positive));
        Debug.Log(InputManager.Instance.axesConfig.GetInputAxesCodeCheck(Axes.Horizontal.String, AxesConfig.AxesType.Negative));
       
    }

    private void Update() {
        //if (Input.GetKeyDown(InputManager.Instance.keyConfig.GetInputKeyCodeCheck(Key.Cancel.String))) {
        //    Debug.Log("kakunin");
        //}
        Debug.Log(InputManager.Instance.axesConfig.GetAxesDown(Axes.Horizontal.String));
    }

}
