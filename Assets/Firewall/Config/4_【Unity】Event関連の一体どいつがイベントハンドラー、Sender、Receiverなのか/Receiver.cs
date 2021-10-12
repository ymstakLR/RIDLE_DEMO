//参考URL:https://www.hanachiru-blog.com/entry/2019/05/09/194400
using UnityEngine;

/// <summary>
/// eventを受ける側
/// </summary>
public class Receiver : MonoBehaviour {
    private void Start() {
        //イベントの購読(subscribe)
        Sender.OnSomeChanged += SomeMethod;
        Sender.Execute();
        Sender.OnSomeChanged += SomeMethod;
        Sender.Execute();
        //イベントの購読を解除(unsubscribe)
        Sender.OnSomeChanged -= SomeMethod;
        Sender.Execute();
    }

    private void SomeMethod() {
        Debug.Log("SomeMethod");
    }
}