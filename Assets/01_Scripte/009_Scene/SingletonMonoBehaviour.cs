///webサイトからの引用　https://qiita.com/Teach/items/c146c7939db7acbd7eee
using System;
using UnityEngine;


public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour {

    private static T instance;
    public static T Instance {
        get {
            if (instance == null) {
                Type t = typeof(T);
                instance = (T)FindObjectOfType(t);
                if (instance == null) {
                    Debug.LogError(t + " をアタッチしているGameObjectはありません");
                }//if
            }//if
            return instance;
        }//get
    }//Instance

    virtual protected void Awake() {
        // 他のゲームオブジェクトにアタッチされているか調べる
        // アタッチされている場合は破棄する。
        CheckInstance();
    }//Awake

    private bool CheckInstance() {
        if (instance == null) {
            instance = this as T;
            return true;
        } else if (Instance == this) {
            return true;
        }//if
        Destroy(this);
        return false;
    }//CheckInstance

}//SingletonMonoBehaviour
