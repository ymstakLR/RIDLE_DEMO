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
    /// �O���[�o���ȓ��͐ݒ��ǉ�����iOK�A�L�����Z���Ȃǁj
    /// </summary>
    /// <param name="inputManagerGenerator">Input manager generator.</param>
    private static void AddGlobalInputSettings(InputManagerGenerator inputManagerGenerator) {

        // ������
        {
            var name = "Horizontal";
            inputManagerGenerator.AddAxis(InputAxis.CreatePadAxis(name, 0, 1));
            inputManagerGenerator.AddAxis(InputAxis.CreateKeyAxis(name, "a", "d", "left", "right"));
        }

        // �c����
        {
            var name = "Vertical";
            inputManagerGenerator.AddAxis(InputAxis.CreatePadAxis(name, 0, 2));
            inputManagerGenerator.AddAxis(InputAxis.CreateKeyAxis(name, "s", "w", "down", "up"));
        }

        // ����
        {
            var name = "OK";
            inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, "z", "joystick button 0"));
        }

        // �L�����Z��
        {
            var name = "Cancel";
            inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, "x", "joystick button 1"));
        }

        // �|�[�Y
        {
            var name = "Pause";
            inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, "escape", "joystick button 7"));
        }
    }
}
