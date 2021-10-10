using MBLDefine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class Test : MonoBehaviour {
    private InputManager im;
    // Start is called before the first frame update
    void Start() {
        im = this.GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.A)) {
            
        }
    }
}
