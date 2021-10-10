using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KeyConfig2 : MonoBehaviour {
    // �L�[�R���t�B�O�̐ݒ���
    public static Dictionary<string, KeyCode> Config
        = new Dictionary<string, KeyCode>();

    // �L�[��Ԏ擾���\�b�h
    public static bool GetKey(string key) {
        return Input.GetKey(Config[key]);
    }
    public static bool GetKeyDown(string key) {
        return Input.GetKeyDown(Config[key]);
    }
    public static bool GetKeyUp(string key) {
        return Input.GetKeyUp(Config[key]);
    }
}