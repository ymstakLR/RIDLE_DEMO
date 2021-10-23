///一部参考サイト参考 http://wordpress.notargs.com/blog/blog/2015/01/23/92/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;

/// <summary>
/// InputManagerの編集
/// 更新日時:20211006
/// </summary>
public static class InputManagerDataEdit {
    public static List<List<string>> _inputData = new List<List<string>>();

    public static List<string> _nameList;
    public static List<string> _negativeButtonList;
    public static List<string> _positiveButtonList;
    public static List<string> _altPositionButtonList;
    public static List<string> _invertList;
    public static List<string> _typeList;
    public static List<string> _axisList;

///保存データ関連の処理についての処理

    /// <summary>
    /// InputDataの読み込み処理
    /// </summary>
    private static void InputDataLoad() {
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
    /// Axesキーを作成する処理
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
    /// 入力の情報
    /// </summary>
    public class InputAxis {
        /// <summary>
        /// inputManager各操作コマンドの情報
        /// </summary>
        public string name = "";
        public string descriptiveName = "";
        public string descriptiveNegativeName = "";
        public string negativeButton = "";
        public string positiveButton = "";
        public string altNegativeButton = "";
        public string altPositiveButton = "";

        public float gravity = 0;
        public float dead = 0;
        public float sensitivity = 0;

        public bool snap = false;
        public bool invert = false;

        public AxisType type = AxisType.KeyOrMouseButton;

        // 1から始まる。
        public int axis = 1;
        // 0なら全てのゲームパッドから取得される。1以降なら対応したゲームパッド。
        public int joyNum = 0;
    }

    /// <summary>
    /// InputManagerにキーを追加する処理
    /// </summary>
    /// <param name="axis">追加するAxis名</param>
    /// <param name="serializedObject"></param>
    /// <param name="axesProperty"></param>
    //private static void AddAxis(InputAxis axis, SerializedObject serializedObject, SerializedProperty axesProperty) {
    //    axesProperty.arraySize++;
    //    serializedObject.ApplyModifiedProperties();
    //    SerializedProperty axisProperty = axesProperty.GetArrayElementAtIndex(axesProperty.arraySize - 1);

    //    GetChildProperty(axisProperty, "m_Name").stringValue = axis.name;
    //    GetChildProperty(axisProperty, "descriptiveName").stringValue = axis.descriptiveName;
    //    GetChildProperty(axisProperty, "descriptiveNegativeName").stringValue = axis.descriptiveNegativeName;
    //    GetChildProperty(axisProperty, "negativeButton").stringValue = axis.negativeButton;
    //    GetChildProperty(axisProperty, "positiveButton").stringValue = axis.positiveButton;
    //    GetChildProperty(axisProperty, "altNegativeButton").stringValue = axis.altNegativeButton;
    //    GetChildProperty(axisProperty, "altPositiveButton").stringValue = axis.altPositiveButton;
    //    GetChildProperty(axisProperty, "gravity").floatValue = axis.gravity;
    //    GetChildProperty(axisProperty, "dead").floatValue = axis.dead;
    //    GetChildProperty(axisProperty, "sensitivity").floatValue = axis.sensitivity;
    //    GetChildProperty(axisProperty, "snap").boolValue = axis.snap;
    //    GetChildProperty(axisProperty, "invert").boolValue = axis.invert;
    //    GetChildProperty(axisProperty, "type").intValue = (int)axis.type;
    //    GetChildProperty(axisProperty, "axis").intValue = axis.axis - 1;
    //    GetChildProperty(axisProperty, "joyNum").intValue = axis.joyNum;

    //    serializedObject.ApplyModifiedProperties();
    //}//AddAxis

    //private static SerializedProperty GetChildProperty(SerializedProperty parent, string name) {
    //    SerializedProperty child = parent.Copy();
    //    child.Next(true);
    //    do {
    //        if (child.name == name) {
    //            return child;
    //        }
    //    } while (child.Next(false));
    //    return null;
    //}//SerializedProperty

    ///保存データ関連の処理についての処理///

    /// <summary>
    /// 入力する値の対応操作タイプの列挙体
    /// </summary>
    public enum InputDataType {
        KeyNegative,
        KeyPositive,
        JoystickNegative,
        JoystickPositive,
        AxesPositive,
        AxesNegative
    }//InputDataType


    /// <summary>
    /// コンフィグ値を変更する処理
    /// </summary>
    /// <param name="targetAxesName">対象のAxes名</param>
    /// <param name="changeKeyCode">変更予定の入力値</param>
    /// <param name="nowInputKeyCode">現在の入力値</param>
    /// <param name="inputType">現在の入力値のInputDataType</param>
    public static void ConfigDataUpdate(
        string targetAxesName, KeyCode changeKeyCode, KeyCode nowInputKeyCode, int inputType) {
        if (changeKeyCode == nowInputKeyCode) {//変更前・後共に同じ入力値の場合
            return;
        }
        switch (inputType) {
            case 0:
            case 1:
                InputManager.Instance.keyConfig.DuplicationKeyCheck(targetAxesName, changeKeyCode, nowInputKeyCode, inputType);
                break;
            case 2:
            case 3:
                InputManager.Instance.keyConfig.DuplicationKeyCheck(targetAxesName, changeKeyCode, nowInputKeyCode, inputType-2);
                //InputManager.Instance.axesConfig.DuplicationAxesCheck(targetAxesName, changeKeyCode, nowInputKeyCode);
                break;
        }
    }//InputTextDuplicationCheack   


    /// <summary>
    /// コンフィグボタン全ての文字更新を行う処理
    /// </summary>
    public static void ConfigButtonsTextUpdate(GameObject buttonCanvas) {
        foreach (Transform childTransform in buttonCanvas.transform) {
            string axesName;
            InputManagerDataEdit.InputDataType inputDataType;
            (axesName, inputDataType) = ConfigButtonInfoSelect(childTransform.name.ToString());
            axesName = InputManagerDataEdit.GetConficButtonKeyCode(axesName, inputDataType).ToString();
            childTransform.GetChild(0).GetComponent<Text>().text = EditText_InputKeyCodeText_To_AxesButtonText(axesName.ToLower()).ToUpper();
        }//foreach
    }//ConfigButtonsTextUpdate

    /// <summary>
    /// コンフィグボタンのキーコードを取得する処理
    /// </summary>
    /// <param name="targetName">対象のAxes名</param>
    /// <param name="targetInputDataType">対象のInputDataType</param>
    /// <returns>対象コンフィグボタンのキーコード</returns>
    public static KeyCode GetConficButtonKeyCode(string targetName, InputDataType targetInputDataType) {
        KeyCode targetKeyCode = KeyCode.None;
        switch (targetInputDataType) {//TypeごとにtargetKeyTextを呼び出し更新
            case InputDataType.KeyPositive:
                targetKeyCode = InputManager.Instance.keyConfig.GetInputKeyCodeCheck(targetName, KeyConfig.KeyType.KeyBoard);
                break;
            case InputDataType.JoystickPositive:
                targetKeyCode = InputManager.Instance.keyConfig.GetInputKeyCodeCheck(targetName, KeyConfig.KeyType.JoyStick);
                break;
            case InputDataType.AxesPositive:
                targetKeyCode = InputManager.Instance.keyConfig.GetInputAxesCodeCheck(targetName, KeyConfig.AxesType.Positive);
                break;
            case InputDataType.AxesNegative:
                targetKeyCode = InputManager.Instance.keyConfig.GetInputAxesCodeCheck(targetName, KeyConfig.AxesType.Negative);
                break;
        }//switch
        return targetKeyCode;
    }//GetConficButtonTex

    /// <summary>
    /// 更新したいキー名と等しいリスト配列番号取得処理
    /// </summary>
    /// <param name="inputName"></param>
    /// <returns></returns>
    private static int GetInputDataID(string inputName) {
        int inputNum = 0;
        while (_nameList[inputNum].ToString() != inputName) {
            inputNum++;
        }//while
        return inputNum;
    }//GetInputDataID

    /// <summary>
    /// コンフィグボタンの情報選択処理
    /// </summary>
    /// <param name="checkButtonName">調べるボタンオブジェクト名</param>
    /// <returns>
    /// string 修正した名(調べるボタン名→InputManagerの対象Axes名)
    /// InputDataType 対象のInputDataTypeタイプ
    /// </returns>
    public static (string, InputManagerDataEdit.InputDataType) ConfigButtonInfoSelect(string checkButtonName) {//別スクリプトに移動
        string editText = checkButtonName;
        InputManagerDataEdit.InputDataType type = InputManagerDataEdit.InputDataType.JoystickNegative;
        if (editText.Contains("Key")) {//キーボードボタンの場合
            editText = editText.Replace("Key", "");
            type = InputManagerDataEdit.InputDataType.KeyPositive;
        } else if (editText.Contains("Controller")) {//コントローラボタンの場合
            editText = editText.Replace("Controller", "");
            type = InputManagerDataEdit.InputDataType.JoystickPositive;
        }//if

        switch (editText) {//移動キーのテキスト変更
            case "Up":
                editText = "Vertical";
                type = InputDataType.AxesPositive;
                break;
            case "Down":
                editText = "Vertical";
                type = InputDataType.AxesNegative;
                break;
            case "Left":
                editText = "Horizontal";
                type = InputDataType.AxesNegative;
                break;
            case "Right":
                editText = "Horizontal";
                type = InputDataType.AxesPositive;
                break;
            default:
                break;
        }//switch
        return (editText, type);
    }//ConfigButtonInfoSelect


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
        text = text.Replace("joystickbutton", "button");//コントローラキー
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

}//InputManagerEdit