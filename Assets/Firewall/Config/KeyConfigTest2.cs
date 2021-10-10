using UnityEngine;
using System.Collections;

public class KeyConfigTest2 : MonoBehaviour {
    // �L�[�R���t�B�O�ݒ�
    void Start() {
        KeyConfig2.Config["Left"] = KeyCode.LeftArrow;
        KeyConfig2.Config["Right"] = KeyCode.RightArrow;
        KeyConfig2.Config["Shoot"] = KeyCode.Z;
    }

    // �L�[�R���t�B�O�g�p��
    void Update() {
        // ���@�̈ړ�
        if (KeyConfig2.GetKeyDown("Left")) {
            Debug.Log("Move Left");
        }
        if (KeyConfig2.GetKeyDown("Right")) {
            Debug.Log("Move Right");
        }

        // ���@�̒�~
        if (KeyConfig2.GetKeyUp("Left")) {
            Debug.Log("Stop Left");
        }
        if (KeyConfig2.GetKeyUp("Right")) {
            Debug.Log("Stop Right");
        }

        // �V���b�g�̔���
        if (KeyConfig2.GetKey("Shoot")) {
            Debug.Log("Shooting!");
        }
    }
}