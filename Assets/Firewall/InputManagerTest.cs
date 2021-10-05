using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

public class InputManagerTest : MonoBehaviour {
    SerializedObject serializedObject;
    SerializedProperty axesProperty;


    // Start is called before the first frame update
    void Start() {
        serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
        axesProperty = serializedObject.FindProperty("m_Axes");
        axesProperty.ClearArray();
        
        Debug.Log("kakunin");
        SaveManager.DataInit();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKey(KeyCode.Q)) {
            serializedObject.ApplyModifiedProperties();
            
        }
        if (Input.GetKey(KeyCode.Alpha1)) {
            AddAxis(CreatePadAxis("TestKey", 0, 1));
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)){
            Debug.Log("kakunin");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            InputManagerEdit.InputDataUpdate();
            Debug.Log("kakunin3");
        }
        DownKeyCheck();
    }

    /// <summary>
    /// 入力されたキーの確認処理
    /// </summary>
    void DownKeyCheck() {
        if (Input.anyKeyDown) {
            foreach (KeyCode code in Enum.GetValues(typeof(KeyCode))) {
                if (Input.GetKeyDown(code)) {
                    //処理を書く
                    Debug.Log(code);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// キー情報の生成処理
    /// </summary>
    /// <param name="name"></param>
    /// <param name="joystickNum"></param>
    /// <param name="axisNum"></param>
    /// <returns></returns>
    private InputAxis CreatePadAxis(string name, int joystickNum, int axisNum) {
        var axis = new InputAxis();
        axis.name = name;
        axis.gravity = 1000;
        axis.dead = 0.2f;
        axis.sensitivity = 1;
        axis.invert = true;
        axis.type = AxisType.JoystickAxis;
        axis.axis = axisNum;
        axis.joyNum = joystickNum;

        return axis;
    }

    /// <summary>
    /// キー情報の反映処理
    /// </summary>
    /// <param name="axis"></param>
    private void AddAxis(InputAxis axis) {
        SerializedProperty axesProperty = serializedObject.FindProperty("m_Axes");

        axesProperty.arraySize++;
        serializedObject.ApplyModifiedProperties();

        SerializedProperty axisProperty = axesProperty.GetArrayElementAtIndex(axesProperty.arraySize - 1);

        GetChildProperty(axisProperty, "m_Name").stringValue = axis.name;
        GetChildProperty(axisProperty, "descriptiveName").stringValue = axis.descriptiveName;
        GetChildProperty(axisProperty, "descriptiveNegativeName").stringValue = axis.descriptiveNegativeName;
        GetChildProperty(axisProperty, "negativeButton").stringValue = axis.negativeButton;
        GetChildProperty(axisProperty, "positiveButton").stringValue = axis.positiveButton;
        GetChildProperty(axisProperty, "altNegativeButton").stringValue = axis.altNegativeButton;
        GetChildProperty(axisProperty, "altPositiveButton").stringValue = axis.altPositiveButton;
        GetChildProperty(axisProperty, "gravity").floatValue = axis.gravity;
        GetChildProperty(axisProperty, "dead").floatValue = axis.dead;
        GetChildProperty(axisProperty, "sensitivity").floatValue = axis.sensitivity;
        GetChildProperty(axisProperty, "snap").boolValue = axis.snap;
        GetChildProperty(axisProperty, "invert").boolValue = axis.invert;
        GetChildProperty(axisProperty, "type").intValue = (int)axis.type;
        GetChildProperty(axisProperty, "axis").intValue = axis.axis - 1;
        GetChildProperty(axisProperty, "joyNum").intValue = axis.joyNum;

        serializedObject.ApplyModifiedProperties();
    }


    private SerializedProperty GetChildProperty(SerializedProperty parent,string name) {
        SerializedProperty child = parent.Copy();
        child.Next(true);
        do {
            if (child.name == name) {
                return child;
            }
        } while (child.Next(false));
        return null;
    }
}
