//�Q�lURL:https://www.hanachiru-blog.com/entry/2019/05/09/194400
using UnityEngine;

/// <summary>
/// event�𔭍s���鑤(�C�x���g�̑��M��)
/// </summary>
public class Sender : MonoBehaviour {
    /// <summary>
    /// �C�x���g�n���h��
    /// </summary>
    public delegate void SomeEventHandler();

    /// <summary>
    /// �C�x���g
    /// </summary>
    public static event SomeEventHandler OnSomeChanged;

    public static void Execute() {
        Debug.LogError("Excute_Start");
        //�C�x���g���s
        if (OnSomeChanged != null) {
            OnSomeChanged();
        } else {
            Debug.Log("nashi");
        }
            
    }
}