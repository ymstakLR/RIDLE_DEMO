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
    private Dictionary<Key, EventHandler> testDic = new Dictionary<Key, EventHandler>();
    private void Start() {
        Key key= Key.Action;
        testDic.Add(key, (o, a) => { Debug.Log("ddd"); });
        Debug.LogError(testDic[Key.Action]);
    }
}
