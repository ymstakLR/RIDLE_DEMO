using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// InputManager�̕ҏW
/// �X�V����:20211004
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
    /// InputData�̓ǂݍ��ݏ���
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
    /// InputData�̕ۑ�����
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
    /// InputManager�̍X�V����
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
    /// �L�[���ꂼ��Ɏw�肵���l��ݒ肷��
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
    /// InputManager�ɃL�[��ǉ����鏈��
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

    ///���̉��̃L�[���X�V�������L�[�R���t�B�O��ʂ̏����Ɏ���

    /// <summary>
    /// �L�[����ύX���鏈��
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
    /// �X�V�������L�[���Ɠ��������X�g�z��ԍ��擾����
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
    /// ���͂����{�^�������̕ҏW����
    /// </summary>
    /// <param name="inputButtonText">���͂����{�^������</param>
    /// <returns>�ҏW�������̓{�^������</returns>
    public static string InputButtonTextEdit(string inputButtonText) {
        inputButtonText = inputButtonText.Replace("arrow", "");//���L�[
        inputButtonText = inputButtonText.Replace("alpha", "");//�����L�[
        inputButtonText = inputButtonText.Replace("page", "page ");//pageup,pagedown
        inputButtonText = inputButtonText.Replace("shift", " shift");//Shift�L�[
        inputButtonText = inputButtonText.Replace("control", " ctrl");//Control�L�[
        inputButtonText = inputButtonText.Replace("alt", " alt");//Alt�L�[
        inputButtonText = inputButtonText.Replace("command", " cmd");//Command�L�[
        inputButtonText = inputButtonText.Replace("sys", "sys ");//PRTSC�L�[
        inputButtonText = inputButtonText.Replace("scroll", "scroll ");//SCRLK�L�[
        inputButtonText = inputButtonText.Replace("joystickbutton", "joystick button ");//�R���g���[���L�[
        if (inputButtonText.Contains("keypad")){
            inputButtonText = inputButtonText.Replace("keypad","");
            switch (inputButtonText) {
                case "plus":
                    inputButtonText = "+";
                    break;
                case "minus":
                    inputButtonText = "-";
                    break;
                case "multiply":
                    inputButtonText = "*";
                    break;
                case "divide":
                    inputButtonText = "/";
                    break;
                case "period":
                    inputButtonText = ".";
                    break;
                default:
                    break;
            }//switch
            if(inputButtonText != "enter")
                inputButtonText = "[" + inputButtonText + "]";
            return inputButtonText;
        }//if//�e���L�[
        if (inputButtonText.Contains("bracket")) {
            inputButtonText = inputButtonText.Replace("bracket", "");
            switch (inputButtonText) {
                case "left":
                    inputButtonText = "[";
                    break;
                case "right":
                    inputButtonText = "]";
                    break;
            }//switch
            return inputButtonText;
        }//if//[]�L�[
        switch (inputButtonText) {
            case "minus":
                inputButtonText = "-";
                break;
            case "quote":
                inputButtonText = "'";
                break;
            case "backquote":
                inputButtonText = "`";
                break;
            case "equals":
                inputButtonText = "=";
                break;
            case "semicolon":
                inputButtonText = ";";
                break;
            case "comma":
                inputButtonText = ",";
                break;
            case "period":
                inputButtonText = ".";
                break;
            case "slash":
                inputButtonText = "/";
                break;
            case "backslash":
                inputButtonText="\\";
                break;
        }//switch//���̑��̃L�[
        return inputButtonText;
    }//InputButtonTextEdit

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
        for (int i = 0; i <= 12; i++) {//���l�EF�L�[�̃`�F�b�N
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
    /// <param name="nowInputValue">���͂����e�L�X�g</param>
    /// <param name="beforeInputValue">���͂���O�̃{�^���e�L�X�g</param>
    public static void InputTextDuplicationCheack(string nowInputValue,string beforeInputValue,string inputAxesName,InputDataType beforeInputValueType) {
        int nowInputValueCount = -1;
        int beforeInputValueCount = -1;
        InputDataType nowInputDataType = InputDataType.KeyPositive;
        beforeInputValue = beforeInputValue.Replace("button", "joystick button");
        if (nowInputValue == beforeInputValue)//�ύX�O�E�㋤�ɓ������͒l�̏ꍇ
            return;

        for (int i = 0; i < 12; i++) {//�ύX����InputManager�z���I��
            if (inputAxesName == _nameList[i]) {
                beforeInputValueCount = i;
                break;
            }//if
        }//for

        for (int i = 0; i < 12; i++) {//�d�������L�[�̊m�F
            if (nowInputValue == _positiveButtonList[i]) {
                nowInputValueCount = i;
                nowInputDataType = InputDataType.KeyPositive;
            }else if (nowInputValue == _negativeButtonList[i]) {
                nowInputValueCount = i;
                nowInputDataType = InputDataType.KeyNegative;
            }else if (nowInputValue == _altPositionButtonList[i]) {
                nowInputValueCount = i;
                nowInputDataType = InputDataType.JoystickPositive;
            }//if

            if (nowInputValueCount != -1) {//�d�������L�[�̓���ւ��K�v�m�F
                if ((beforeInputValueCount < 10 && nowInputValueCount < 10)|| 
                    (beforeInputValueCount > 9 && beforeInputValueCount < 12 && nowInputValueCount > 9 && nowInputValueCount < 12)) {
                    InputDataUpdate(_nameList[beforeInputValueCount], nowInputValue, beforeInputValueType);
                    InputDataUpdate(_nameList[nowInputValueCount], beforeInputValue, nowInputDataType);
                    InputManagerUpdate();
                    return;
                }//if
            }//if
        }//for
        InputDataUpdate(inputAxesName, nowInputValue, beforeInputValueType);
        InputManagerUpdate();
    }//InputTextDuplicationCheack

}//InputManagerEdit
