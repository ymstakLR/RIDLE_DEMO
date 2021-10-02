using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// InputManagerの編集
/// 更新日時:20210922
/// </summary>
public static class InputManagerEdit {
    public static List<List<string>> _inputData = new List<List<string>>();

    public static List<string> _nameList;
    public static List<string> _negativeButtonList;
    public static List<string> _positiveButtonList;
    public static List<string> _altPositionButtonList;
    public static List<string> _invertList;
    public static List<string> _typeList;
    public static List<string> _axisList;

    /// <summary>
    /// InputDataの読み込み処理
    /// </summary>
    public static void InputDataLoad() {
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

    /// <summary>
    /// InputDataの保存処理
    /// </summary>
    private static void InputDataSave() {
        _inputData.Insert(0, _nameList);
        _inputData.Insert(1, _negativeButtonList);
        _inputData.Insert(2, _positiveButtonList);
        _inputData.Insert(3, _altPositionButtonList);
        _inputData.Insert(4, _invertList);
        _inputData.Insert(5, _typeList);
        _inputData.Insert(6, _axisList);
        SaveManager.InputDataUpdate(_inputData);
    }//InputDataSave

    /// <summary>
    /// InputManagerの更新処理
    /// </summary>
    public static void InputManagerUpdate() {
        InputDataLoad();
        SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
        SerializedProperty axesProperty = serializedObject.FindProperty("m_Axes");
        axesProperty.ClearArray();
        for (int i = 0; i < 12; i++) {
            AddAxis(CreateInputAxis(i), serializedObject, axesProperty);
        }//for
    }//InputManagerUpdate

    /// <summary>
    /// キーそれぞれに指定した値を設定する
    /// </summary>
    private static InputAxis CreateInputAxis(int i) {
        var axis = new InputAxis();
        axis.name = _nameList[i];
        axis.negativeButton = _negativeButtonList[i];
        axis.positiveButton = _positiveButtonList[i];
        axis.altPositiveButton = _altPositionButtonList[i];
        axis.gravity = 1000;
        axis.dead = 0.2f;
        axis.sensitivity = 1;
        if (_invertList[i].ToLower() == "true") {
            axis.invert = true;
        }//if
        if (_typeList[i] == "JoystickAxis") {
            axis.type = AxisType.JoystickAxis;
        }//if
        axis.axis = int.Parse(_axisList[i]);
        return axis;
    }//CreateInputAxis

    /// <summary>
    /// InputManagerにキーを追加する処理
    /// </summary>
    /// <param name="axis"></param>
    /// <param name="serializedObject"></param>
    /// <param name="axesProperty"></param>
    private static void AddAxis(InputAxis axis, SerializedObject serializedObject, SerializedProperty axesProperty) {
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
    }//AddAxis

    private static SerializedProperty GetChildProperty(SerializedProperty parent, string name) {
        SerializedProperty child = parent.Copy();
        child.Next(true);
        do {
            if (child.name == name) {
                return child;
            }
        } while (child.Next(false));
        return null;
    }//SerializedProperty

    public static string InputTextUpdate(string inputName,InputDataType type) {
        InputDataLoad();
        int inputNum = InputDataIdentification(inputName);
        string inputText="";
        switch (type) {
            case InputDataType.KeyNegative:
                inputText = _negativeButtonList[inputNum];
                break;
            case InputDataType.KeyPositive:
                inputText = _positiveButtonList[inputNum];
                break;
            case InputDataType.JoystickNegative:
                break;
            case InputDataType.JoystickPositive:
                inputText = _altPositionButtonList[inputNum];
                break;
        }
        inputText = inputText.Replace("joystick", "");
        return inputText.ToUpper();
    }

    ///この下のキー情報更新処理をキーコンフィグ画面の処理に実装

    /// <summary>
    /// キー情報を変更する処理
    /// </summary>
    /// <param name="inputName"></param>
    /// <param name="keyButton"></param>
    /// <param name="joystickButton"></param>
    public static void InputDataUpdate(string inputName, string inputCode,InputDataType type) {
        InputDataLoad();
        int inputNum = InputDataIdentification(inputName);
        switch (type) {
            case InputDataType.KeyNegative:
                _negativeButtonList[inputNum] = inputCode;
                break;
            case InputDataType.KeyPositive:
                _positiveButtonList[inputNum] = inputCode;
                break;
            case InputDataType.JoystickNegative:
                break;
            case InputDataType.JoystickPositive:
                _altPositionButtonList[inputNum] = inputCode;
                break;
            default:
                break;
        }//switch
        InputDataSave();
    }//InputDataUpdate

    /// <summary>
    /// 更新したいキー名と等しいリスト配列番号取得処理
    /// </summary>
    /// <param name="inputName"></param>
    /// <returns></returns>
    public static int InputDataIdentification(string inputName) {
        int inputNum = 0;
        while (_nameList[inputNum].ToString() != inputName) {
            inputNum++;
        }//while
        return inputNum;
    }

    public enum InputDataType {
        KeyNegative,
        KeyPositive,
        JoystickNegative,
        JoystickPositive
    }



    public static string InputTextEdit(string inputText) {
        inputText = inputText.Replace("arrow", "");//矢印キー
        inputText = inputText.Replace("alpha", "");//数字キー
        inputText = inputText.Replace("page", "page ");//pageup,pagedown
        inputText = inputText.Replace("shift", " shift");//Shiftキー
        inputText = inputText.Replace("control", " ctrl");//Controlキー
        inputText = inputText.Replace("alt", " alt");//Altキー
        inputText = inputText.Replace("command", " cmd");//Commandキー
        inputText = inputText.Replace("sys", "sys ");//PRTSCキー
        inputText = inputText.Replace("scroll", "scroll ");//SCRLKキー
        inputText = inputText.Replace("joystickbutton", "joystick button ");
        if (inputText.Contains("keypad")){//テンキー
            inputText = inputText.Replace("keypad","");
            switch (inputText) {
                case "plus":
                    inputText = "+";
                    break;
                case "minus":
                    inputText = "-";
                    break;
                case "multiply":
                    inputText = "*";
                    break;
                case "divide":
                    inputText = "/";
                    break;
                case "period":
                    inputText = ".";
                    break;
                default:
                    break;
            }
            if(inputText != "enter")
                inputText = "[" + inputText + "]";
            return inputText;
        }
        if (inputText.Contains("bracket")) {//[]キー
            inputText = inputText.Replace("bracket", "");
            switch (inputText) {
                case "left":
                    inputText = "[";
                    break;
                case "right":
                    inputText = "]";
                    break;
            }
            return inputText;
        }
        switch (inputText) {//その他のキー
            case "minus":
                inputText = "-";
                break;
            case "quote":
                inputText = "'";
                break;
            case "backquote":
                inputText = "`";
                break;
            case "equals":
                inputText = "=";
                break;
            case "semicolon":
                inputText = ";";
                break;
            case "comma":
                inputText = ",";
                break;
            case "period":
                inputText = ".";
                break;
            case "slash":
                inputText = "/";
                break;
            case "backslash":
                inputText="\\";
                break;
        }
        return inputText;
    }

    public static bool InputTextCheack(string inputText) {
        inputText = inputText.Replace(" ", "");
        if (inputText.Contains("return"))
            return true;
        if (inputText.Contains("[") && inputText.Contains("]"))
            return true;
        if (inputText.Contains("button"))
            return true;
        if (inputText.Length == 1)
            return true;
        for (int i = 0; i <= 12; i++) {//数値・Fキーのチェック
            if (inputText == i.ToString()||inputText =="f"+i.ToString()) {
                return true;
            }
        }//for
        foreach (InputTextCheackData data in Enum.GetValues(typeof(InputTextCheackData))){
            string str = Enum.GetName(typeof(InputTextCheackData), data);
            if (inputText == str) {
                return true;
            }
        }
        return false;
    }

    enum InputTextCheackData {
        tab,escape,leftshift,leftctrl,leftcmd,leftalt,
        space,rightalt,menu,rightctrl,rightshift,backspace,
        up,down,left,right,insert,delete,home,end,pageup,pagedown,
        sysreq,scrolllock,pause,enter,numlock,
    }

    public enum InputTextDuplicationType {
        posButton,
        negaButton,
        altPosButton,
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="inputText">入力したテキスト</param>
    /// <param name="beforeInputText">入力する前のボタンテキスト</param>
    public static void InputTextDuplicationCheack(string inputText,string beforeInputText,string fixname,InputDataType type) {
        int inputTextListNum = -1;
        int inputButtonTextListNum=-1;
        InputDataType inputDataType=InputDataType.KeyPositive;
        InputDataLoad();
        if (inputText == beforeInputText) {
            Debug.LogError("同じキー入力確認");
            return;
        }

        for (int i = 0; i < 12; i++) {//変更するInputManager配列を選択
            if (fixname == _nameList[i]) {
                inputButtonTextListNum = i;
                break;
            }
        }
        //Debug.LogWarning("変更元InputManegerは" + _nameList[inputButtonTextListNum] + "__ボタン__" + beforeInputText+"__"+inputButtonTextListNum);
        Debug.Log("変更したいキー名__" + inputText);

        //Debug.Log("入力値と同じボタンの値を求める__" + inputText);
        for (int i = 0; i < 12; i++) {
            if (inputText == _positiveButtonList[i]) {
                inputTextListNum = i;
                Debug.LogError(inputText+"と重複するInputManegerは" + _nameList[inputTextListNum] +"__" + inputTextListNum);
            }
            if (inputText == _negativeButtonList[i]) {
                inputTextListNum = i;
                Debug.LogError(inputText + "と重複するInputManegerは" + _nameList[inputTextListNum] +"__" + inputTextListNum);
            }
            if (inputText == _altPositionButtonList[i]) {
                inputTextListNum = i;
                Debug.LogError(inputText + "と重複するInputManegerは" + _nameList[inputTextListNum] + "__重複したボタン文字__" + inputText + "__" + inputDataType);
            }
            if (inputTextListNum != -1) {
                if (inputButtonTextListNum < 10 && inputTextListNum < 10) {
                    Debug.LogWarning("ゲーム操作キーで重複あり__" + inputButtonTextListNum + "__" + inputTextListNum);
                    Debug.LogError(_nameList[inputButtonTextListNum] + "と" + _nameList[inputTextListNum] + "で重複している");

                }
                if (inputButtonTextListNum > 9 && inputButtonTextListNum < 12 &&
                    inputTextListNum > 9 && inputTextListNum < 12 ) {
                    Debug.LogWarning("Ok,Cancelキーで重複あり__" + inputTextListNum + "__" + inputButtonTextListNum);
                    Debug.LogError(_nameList[inputTextListNum] + "と" + _nameList[inputButtonTextListNum] + "で重複している");
                }
                inputTextListNum = -1;
            }

        }
    }

}//InputManagerEdit
