//参考URL:https://www.hanachiru-blog.com/entry/2019/05/09/194400
using UnityEngine;

/// <summary>
/// eventを発行する側(イベントの送信元)
/// </summary>
public class Sender : MonoBehaviour {
    /// <summary>
    /// イベントハンドラ
    /// </summary>
    public delegate void SomeEventHandler();

    /// <summary>
    /// イベント
    /// </summary>
    public static event SomeEventHandler OnSomeChanged;

    public static void Execute() {
        Debug.LogError("Excute_Start");
        //イベント発行
        if (OnSomeChanged != null) {
            OnSomeChanged();
        } else {
            Debug.Log("nashi");
        }
            
    }
}