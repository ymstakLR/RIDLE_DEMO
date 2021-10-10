using UnityEngine;
using System.Collections;

public class KeyConfigTest2 : MonoBehaviour {
    // キーコンフィグ設定
    void Start() {
        KeyConfig2.Config["Left"] = KeyCode.LeftArrow;
        KeyConfig2.Config["Right"] = KeyCode.RightArrow;
        KeyConfig2.Config["Shoot"] = KeyCode.Z;
    }

    // キーコンフィグ使用例
    void Update() {
        // 自機の移動
        if (KeyConfig2.GetKeyDown("Left")) {
            Debug.Log("Move Left");
        }
        if (KeyConfig2.GetKeyDown("Right")) {
            Debug.Log("Move Right");
        }

        // 自機の停止
        if (KeyConfig2.GetKeyUp("Left")) {
            Debug.Log("Stop Left");
        }
        if (KeyConfig2.GetKeyUp("Right")) {
            Debug.Log("Stop Right");
        }

        // ショットの発射
        if (KeyConfig2.GetKey("Shoot")) {
            Debug.Log("Shooting!");
        }
    }
}