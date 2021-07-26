using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FilePath : MonoBehaviour {
    public Text _text;
    public string path;
    // Start is called before the first frame update
    void Start() {
        //string path = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
#if UNITY_EDITOR
        path = System.IO.Directory.GetCurrentDirectory();
#else
        path = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');    
#endif
        _text.text = path;
    }

    // Update is called once per frame
    void Update() {

    }
}
