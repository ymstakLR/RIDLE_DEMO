using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class InputManagerEdit {
    public static List<List<string>> _inputData = new List<List<string>>();

    public static List<string> _nameList;
    public static List<string> _negativeButtonList;
    public static List<string> _positiveButtonList;
    public static List<string> _altPositionButtonList;
    public static List<string> _invertList;
    public static List<string> _typeList;
    public static List<string> _axisList;

    public static void InputDataLoad() {
        Debug.Log("InputManagerEdit.InputDataLoad");
        _nameList = new List<string>();
        _nameList.AddRange(SaveManager.inputData.nameList);
        _negativeButtonList = new List<string>();
        _negativeButtonList.AddRange(SaveManager.inputData.negativeButtonList);
        _positiveButtonList = new List<string>();
        _positiveButtonList.AddRange(SaveManager.inputData.positiveButtonList);
        _altPositionButtonList = new List<string>();
        _altPositionButtonList.AddRange(SaveManager.inputData.altPositionButtonList);
        _invertList = new List<string>();
        _invertList.AddRange(SaveManager.inputData.invertList);
        _typeList = new List<string>();
        _typeList.AddRange(SaveManager.inputData.typeList);
        _axisList = new List<string>();
        _axisList.AddRange(SaveManager.inputData.axisList);
    }//InputDataLoad

    private static void InputDataSave() {
        Debug.Log("InputManagerEdit.InputDataSave");
        _inputData.Insert(0, _nameList);
        _inputData.Insert(1,_negativeButtonList);
        _inputData.Insert(2,_positiveButtonList);
        _inputData.Insert(3,_altPositionButtonList);
        _inputData.Insert(4,_invertList);
        _inputData.Insert(5,_typeList);
        _inputData.Insert(6,_axisList);
        SaveManager.InputDataUpdate(_inputData);
    }

    public static void InputDataUpdate(string inputName,string keyButton,string joystickButton) {
        Debug.Log("InputManagerEdit.InputDataUpdate");
        InputDataLoad();
        int inputNum = InputDataIdentification(inputName);
        if(keyButton != "") {
            _positiveButtonList[inputNum] = keyButton;
        }//if
        if(joystickButton != "") {
            _altPositionButtonList[inputNum] = joystickButton;
        }//if
        InputDataSave();
    }//InputDataUpdate

    public static int InputDataIdentification(string inputName) {
        int inputNum = 0;
        while(_nameList[inputNum].ToString() != inputName) {
            inputNum++;
        }//while
        return inputNum;
    }

    public static void InputManagerUpdate() {
        for(int i = 0; i < 10; i++) {
            AddAxis(CreatePadAxis(i));
        }
    }


    private static InputAxis CreatePadAxis(int i) {
        var axis = new InputAxis();
        axis.name = _nameList[i];
        axis.negativeButton = _negativeButtonList[i];
        axis.positiveButton = _positiveButtonList[i];
        axis.altPositiveButton = _altPositionButtonList[i];
        if (_invertList[i] == "true") {
            axis.invert = true;
        }
        if(_typeList[i]== "KeyOrMouseButton") {
            axis.type = AxisType.KeyOrMouseButton;
        }
        axis.axis = int.Parse(_axisList[i]);
        return axis;
    }

    private static void AddAxis(InputAxis axis) {
        SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
        SerializedProperty axesProperty = serializedObject.FindProperty("m_Axes");
        axesProperty.ClearArray();

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

    private static SerializedProperty GetChildProperty(SerializedProperty parent, string name) {
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
