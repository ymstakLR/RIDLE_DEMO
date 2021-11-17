///一部参考サイト参考 http://wordpress.notargs.com/blog/blog/2015/01/23/92/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

/// <summary>
/// InputManagerの編集
/// 更新日時:20211006
/// </summary>
public static class InputManagerDataEdit {

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
        if (changeKeyCode == nowInputKeyCode)//変更前・後共に同じ入力値の場合
            return;
        switch (inputType) {
            case 0:
            case 1:
                ConfigManager.Instance.config.DuplicationCodeCheck(targetAxesName, changeKeyCode, nowInputKeyCode, inputType);
                break;
            case 2:
            case 3:
                ConfigManager.Instance.config.DuplicationCodeCheck(targetAxesName, changeKeyCode, nowInputKeyCode, inputType-2);
                break;
        }//switch
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
            childTransform.GetChild(0).GetComponent<Text>().text = EditText_InputCodeText_To_AxesButtonText(axesName.ToLower()).ToUpper();
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
                targetKeyCode = ConfigManager.Instance.config.GetInputKeyCodeCheck(targetName, Config.KeyType.KeyBoard);
                break;
            case InputDataType.JoystickPositive:
                targetKeyCode = ConfigManager.Instance.config.GetInputKeyCodeCheck(targetName, Config.KeyType.JoyStick);
                break;
            case InputDataType.AxesPositive:
                targetKeyCode = ConfigManager.Instance.config.GetInputAxisCodeCheck(targetName, Config.AxisType.Positive);
                break;
            case InputDataType.AxesNegative:
                targetKeyCode = ConfigManager.Instance.config.GetInputAxisCodeCheck(targetName, Config.AxisType.Negative);
                break;
        }//switch
        return targetKeyCode;
    }//GetConficButtonTex

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
    public static string EditText_InputCodeText_To_AxesButtonText(string inputKeyCodeText) {
        string text = inputKeyCodeText;
        text = text.Replace("arrow", "");//矢印キー
        text = text.Replace("alpha", "");//数字キー
        text = text.Replace("page", "page ");//pageup,pagedown
        text = text.Replace("shift", " shift");//Shiftキー
        text = text.Replace("control", " ctrl");//Controlキー
        text = text.Replace("alt", " alt");//Altキー
        text = text.Replace("command", " cmd");//Commandキー
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
            case "scrolllock":
                text = "scrlk";
                break;
            case "sysreq":
                text = "prtsc";
                break;
        }//switch//その他のキー
        return text;
    }//EditText_InputKeyCodeText_To_AxesButtonText

    /// <summary>
    /// AxesButtonの対象外文字をチェックする処理
    /// </summary>
    /// <param name="checkText">調べる文字</param>
    /// <returns>対象外の文字化の判定</returns>
    public static bool GetNonTargetTextCheck(string checkText) {
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
        foreach (TargetTextData data in Enum.GetValues(typeof(TargetTextData))){
            string targetText = Enum.GetName(typeof(TargetTextData), data);
            if (checkText == targetText)
                return false;
        }//foreach//対象文字列挙体のチェック
        return true;
    }//GetNonTartetText

    /// <summary>
    /// AxesButtonの対象文字の列挙体
    /// </summary>
    private enum TargetTextData {
        tab,escape,leftshift,leftctrl,leftcmd,leftalt,
        space,rightalt,menu,rightctrl,rightshift,backspace,
        up,down,left,right,insert,delete,home,end,pageup,pagedown,
        prtsc, scrlk, pause,enter,numlock,
    }//TargetTextData_AxesButton

}//InputManagerEdit