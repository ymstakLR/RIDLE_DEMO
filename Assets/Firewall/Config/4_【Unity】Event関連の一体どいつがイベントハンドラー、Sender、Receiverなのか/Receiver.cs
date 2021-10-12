//�Q�lURL:https://www.hanachiru-blog.com/entry/2019/05/09/194400
using UnityEngine;

/// <summary>
/// event���󂯂鑤
/// </summary>
public class Receiver : MonoBehaviour {
    private void Start() {
        //�C�x���g�̍w��(subscribe)
        Sender.OnSomeChanged += SomeMethod;
        Sender.Execute();
        Sender.OnSomeChanged += SomeMethod;
        Sender.Execute();
        //�C�x���g�̍w�ǂ�����(unsubscribe)
        Sender.OnSomeChanged -= SomeMethod;
        Sender.Execute();
    }

    private void SomeMethod() {
        Debug.Log("SomeMethod");
    }
}