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
    /// �����ҏW����(���͂����L�[���� �� axesButoon�p����)
    /// </summary>
    /// <param name="inputKeyCodeText">���͂����L�[����/param>
    /// <returns>�ҏW�������̓{�^������</returns>
    public static string EditText_InputKeyCodeText_To_AxesButtonText(string inputKeyCodeText) {
        string text = inputKeyCodeText;
        text = text.Replace("arrow", "");//���L�[
        text = text.Replace("alpha", "");//�����L�[
        text = text.Replace("page", "page ");//pageup,pagedown
        text = text.Replace("shift", " shift");//Shift�L�[
        text = text.Replace("control", " ctrl");//Control�L�[
        text = text.Replace("alt", " alt");//Alt�L�[
        text = text.Replace("command", " cmd");//Command�L�[
        text = text.Replace("sys", "sys ");//PRTSC�L�[
        text = text.Replace("scroll", "scroll ");//SCRLK�L�[
        text = text.Replace("joystickbutton", "joystick button ");//�R���g���[���L�[
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
        }//if//�e���L�[
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
        }//if//[]�L�[
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
        }//switch//���̑��̃L�[
        return text;
    }//EditText_InputKeyCodeText_To_AxesButtonText


    /// <summary>
    /// AxesButton�̑ΏۊO�������`�F�b�N���鏈��
    /// </summary>
    /// <param name="checkText">���ׂ镶��</param>
    /// <returns>�ΏۊO�̕������̔���</returns>
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
        }//for//���l�EF�L�[�̃`�F�b�N
        foreach (TargetTextData_AxesButton data in Enum.GetValues(typeof(TargetTextData_AxesButton))){
            string targetText = Enum.GetName(typeof(TargetTextData_AxesButton), data);
            if (checkText == targetText)
                return false;
        }//foreach//�Ώە����񋓑̂̃`�F�b�N
        return true;
    }//GetNonTartetText

    /// <summary>
    /// AxesButton�̑Ώە����̗񋓑�
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
    /// <param name="changeInputValue">���͂����e�L�X�g</param>
    /// <param name="nowInputValue">���͂���O�̃{�^���e�L�X�g</param>
    public static void InputTextDuplicationCheack(string changeInputValue,string nowInputValue,string axesName,InputDataType nowInputValueType) {
        int changeInputValueCount = -1;
        int nowInputValueCount = -1;
        InputDataType changeInputValueType = InputDataType.KeyPositive;
        nowInputValue = nowInputValue.Replace("button", "joystick button");
        if (changeInputValue == nowInputValue)//�ύX�O�E�㋤�ɓ������͒l�̏ꍇ
            return;

        for (int i = 0; i < 12; i++) {//�ύX����InputManager�z���I��
            if (axesName == _nameList[i]) {
                nowInputValueCount = i;
                break;
            }//if
        }//for

        for (int i = 0; i < 12; i++) {//�d�������L�[�̊m�F
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

            if (changeInputValueCount != -1) {//�d�������L�[�̓���ւ��K�v�m�F
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
