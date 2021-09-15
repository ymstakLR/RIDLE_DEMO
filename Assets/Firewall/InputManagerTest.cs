using UnityEngine;
using UnityEditor;
using System.Collections;

public class InputManagerTest : MonoBehaviour {
    SerializedObject serializedObject;
    SerializedProperty axesProperty;


    // Start is called before the first frame update
    void Start() {
        serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
        axesProperty = serializedObject.FindProperty("m_Axes");
        axesProperty.ClearArray();
        
        Debug.Log("kakunin");
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKey(KeyCode.Q)) {
            serializedObject.ApplyModifiedProperties();
            Debug.Log("key");
        }
       // AddGlobalInputSettings(inputManagerGenerator);
    }

    /// <summary>
    /// グローバルな入力設定を追加する（OK、キャンセルなど）
    /// </summary>
    /// <param name="inputManagerGenerator">Input manager generator.</param>
    private static void AddGlobalInputSettings(InputManagerGenerator inputManagerGenerator) {

        // 横方向
        {
            var name = "Horizontal";
            inputManagerGenerator.AddAxis(InputAxis.CreatePadAxis(name, 0, 1));
            inputManagerGenerator.AddAxis(InputAxis.CreateKeyAxis(name, "a", "d", "left", "right"));
        }

        // 縦方向
        {
            var name = "Vertical";
            inputManagerGenerator.AddAxis(InputAxis.CreatePadAxis(name, 0, 2));
            inputManagerGenerator.AddAxis(InputAxis.CreateKeyAxis(name, "s", "w", "down", "up"));
        }

        // 決定
        {
            var name = "OK";
            inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, "z", "joystick button 0"));
        }

        // キャンセル
        {
            var name = "Cancel";
            inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, "x", "joystick button 1"));
        }

        // ポーズ
        {
            var name = "Pause";
            inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, "escape", "joystick button 7"));
        }
    }
}
