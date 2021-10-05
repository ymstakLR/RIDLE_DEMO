using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// InputManagerの編集
/// 更新日時:20211004
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
        _nameList.AddRange(SaveManager.inputDataStruct.nameList);
        _negativeButtonList = new List<string>();
        _negativeButtonList.AddRange(SaveManager.inputDataStruct.negativeButtonList);
        _positiveButtonList = new List<string>();
        _positiveButtonList.AddRange(SaveManager.inputDataStruct.positiveButtonList);
        _altPositionButtonList = new List<string>();
        _altPositionButtonList.AddRange(SaveManager.inputDataStruct.altPositionButtonList);
        _invertList = new List<string>();
        _invertList.AddRange(SaveManager.inputDataStruct.invertList);
        _typeList = new List<string>();
        _typeList.AddRange(SaveManager.inputDataStruct.typeList);
        _axisList = new List<string>();
        _axisList.AddRange(SaveManager.inputDataStruct.axisList);
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
        inputText = inputText.Replace("joystick ", "");
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


    /// <summary>
    /// 文字編集処理(入力したキー文字 → axesButoon用文字)
    /// </summary>
    /// <param name="inputKeyCodeText">入力したキー文字/param>
    /// <returns>編集した入力ボタン文字</returns>
    public static string EditText_InputKeyCodeText_To_AxesButtonText(string inputKeyCodeText) {
        string text = inputKeyCodeText;
        text = text.Replace("arrow", "");//矢印キー
        text = text.Replace("alpha", "");//数字キー
        text = text.Replace("page", "page ");//pageup,pagedown
        text = text.Replace("shift", " shift");//Shiftキー
        text = text.Replace("control", " ctrl");//Controlキー
        text = text.Replace("alt", " alt");//Altキー
        text = text.Replace("command", " cmd");//Commandキー
        text = text.Replace("sys", "sys ");//PRTSCキー
        text = text.Replace("scroll", "scroll ");//SCRLKキー
        text = text.Replace("joystickbutton", "joystick button ");//コントローラキー
        if (text.Contains("keypad")){
            text = text.Replace("keypad","");
            switch (text) {
                case "plus":
                    text = "+";
                    break;
                case "minus":
                    text = "-";
                    break;
                case "multiply":
                    text = "*";
                    break;
                case "divide":
                    text = "/";
                    break;
                case "period":
                    text = ".";
                    break;
                default:
                    break;
            }//switch
            if(text != "enter")
                text = "[" + text + "]";
            return text;
        }//if//テンキー
        if (text.Contains("bracket")) {
            text = text.Replace("bracket", "");
            switch (text) {
                case "left":
                    text = "[";
                    break;
                case "right":
                    text = "]";
                    break;
            }//switch
            return text;
        }//if//[]キー
        switch (text) {
            case "minus":
                text = "-";
                break;
            case "quote":
                text = "'";
                break;
            case "backquote":
                text = "`";
                break;
            case "equals":
                text = "=";
                break;
            case "semicolon":
                text = ";";
                break;
            case "comma":
                text = ",";
                break;
            case "period":
                text = ".";
                break;
            case "slash":
                text = "/";
                break;
            case "backslash":
                text="\\";
                break;
        }//switch//その他のキー
        return text;
    }//EditText_InputKeyCodeText_To_AxesButtonText


    /// <summary>
    /// AxesButtonの対象外文字をチェックする処理
    /// </summary>
    /// <param name="checkText">調べる文字</param>
    /// <returns>対象外の文字化の判定</returns>
    public static bool GetNonTargetTextCheck_AxesButton(string checkText) {
        checkText = checkText.Replace(" ", "");
        if (checkText.Contains("return"))
            return false;
        if (checkText.Contains("[") && checkText.Contains("]"))
            return false;
        if (checkText.Contains("button"))
            return false;
        if (checkText.Length == 1)
            return false;
        for (int i = 0; i <= 12; i++) {
            if (checkText == i.ToString()||checkText =="f"+i.ToString())
                return false;
        }//for//数値・Fキーのチェック
        foreach (TargetTextData_AxesButton data in Enum.GetValues(typeof(TargetTextData_AxesButton))){
            string targetText = Enum.GetName(typeof(TargetTextData_AxesButton), data);
            if (checkText == targetText)
                return false;
        }//foreach//対象文字列挙体のチェック
        return true;
    }//GetNonTartetText

    /// <summary>
    /// AxesButtonの対象文字の列挙体
    /// </summary>
    private enum TargetTextData_AxesButton {
        tab,escape,leftshift,leftctrl,leftcmd,leftalt,
        space,rightalt,menu,rightctrl,rightshift,backspace,
        up,down,left,right,insert,delete,home,end,pageup,pagedown,
        sysreq,scrolllock,pause,enter,numlock,
    }//TargetTextData_AxesButton

    public enum InputTextDuplicationType {
        posButton,
        negaButton,
        altPosButton,
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="changeInputValue">入力したテキスト</param>
    /// <param name="nowInputValue">入力する前のボタンテキスト</param>
    public static void InputTextDuplicationCheack(string changeInputValue,string nowInputValue,string axesName,InputDataType nowInputValueType) {
        int changeInputValueCount = -1;
        int nowInputValueCount = -1;
        InputDataType changeInputValueType = InputDataType.KeyPositive;
        nowInputValue = nowInputValue.Replace("button", "joystick button");
        if (changeInputValue == nowInputValue)//変更前・後共に同じ入力値の場合
            return;

        for (int i = 0; i < 12; i++) {//変更するInputManager配列を選択
            if (axesName == _nameList[i]) {
                nowInputValueCount = i;
                break;
            }//if
        }//for

        for (int i = 0; i < 12; i++) {//重複したキーの確認
            if (changeInputValue == _positiveButtonList[i]) {
                changeInputValueCount = i;
                changeInputValueType = InputDataType.KeyPositive;
            }else if (changeInputValue == _negativeButtonList[i]) {
                changeInputValueCount = i;
                changeInputValueType = InputDataType.KeyNegative;
            }else if (changeInputValue == _altPositionButtonList[i]) {
                changeInputValueCount = i;
                changeInputValueType = InputDataType.JoystickPositive;
            }//if

            if (changeInputValueCount != -1) {//重複したキーの入れ替え必要確認
                if ((nowInputValueCount < 10 && changeInputValueCount < 10)|| 
                    (nowInputValueCount > 9 && nowInputValueCount < 12 && changeInputValueCount > 9 && changeInputValueCount < 12)) {
                    InputDataUpdate(axesName, changeInputValue, nowInputValueType);
                    InputDataUpdate(_nameList[changeInputValueCount], nowInputValue, changeInputValueType);
                    InputManagerUpdate();
                    return;
                }//if
            }//if
        }//for
        InputDataUpdate(axesName, changeInputValue, nowInputValueType);
        InputManagerUpdate();
    }//InputTextDuplicationCheack

    public static void KeySwap() {

    }

}//InputManagerEdit
